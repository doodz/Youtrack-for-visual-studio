using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.Serialization;
using ReactiveUI;
using Splat;

namespace YouTrackClientVS.Infrastructure
{
    [DataContract]
    public class ReactiveValidatedObject : ReactiveObject, IDataErrorInfo
    {
        /// <summary>
        ///
        /// </summary>
        public ReactiveValidatedObject()
        {
            Changing.Subscribe(x =>
            {
                if (x.Sender != this) return;

                if (_validationCache.ContainsKey(x.PropertyName)) _validationCache.Remove(x.PropertyName);
            });

            _validatedPropertyCount = new Lazy<int>(() =>
            {
                lock (allValidatedProperties)
                {
                    return allValidatedProperties.Get(GetType()).Count;
                }
            });
        }


        [IgnoreDataMember] public string Error => null;

        [IgnoreDataMember]
        public string this[string columnName]
        {
            get
            {
                if (_validationCache.TryGetValue(columnName, out var ret)) return ret;

                this.Log().Debug("Checking {0:X}.{1}...", GetHashCode(), columnName);
                ret = GetPropertyValidationError(columnName);
                this.Log().Debug("Validation result: {0}", ret);

                _validationCache[columnName] = ret;

                var getter = MakeGetter(() => columnName);
                _validationObservable.OnNext(new ObservedChange<object, bool>(this, getter, ret != null));

                return ret;
            }
        }

        private static Expression MakeGetter<T>(Expression<Func<T>> propertyLambda)
        {
            var result = Expression.Lambda(propertyLambda.Body);
            return result;
        }

        public IEnumerable<string> Errors
        {
            get { return _validationCache.Values.Where(x => x != null); }
        }


        public bool IsObjectValid()
        {
            if (_validationCache.Count == _validatedPropertyCount.Value)
            {
                //return _validationCache.Values.All(x => x == null);
                foreach (var v in _validationCache.Values)
                    if (v != null)
                        return false;

                return true;
            }

            IEnumerable<string> allProps;
            lock (allValidatedProperties)
            {
                allProps = allValidatedProperties.Get(GetType()).Keys;
            }

            ;

            //return allProps.All(x => this[x] == null);
            foreach (var v in allProps)
                if (this[v] != null)
                    return false;

            return true;
        }

        protected void InvalidateValidationCache()
        {
            _validationCache.Clear();
        }

        public void ForcePropertyValidation(string property)
        {
            this.RaisePropertyChanging(property);
            this.RaisePropertyChanged(property);
        }

        [IgnoreDataMember]
        private readonly Subject<IObservedChange<object, bool>> _validationObservable =
            new Subject<IObservedChange<object, bool>>();

        [IgnoreDataMember]
        public IObservable<IObservedChange<object, bool>> ValidationObservable => _validationObservable;

        [IgnoreDataMember] private readonly Lazy<int> _validatedPropertyCount;

        [IgnoreDataMember]
        private readonly Dictionary<string, string> _validationCache = new Dictionary<string, string>();

        private static readonly MemoizingMRUCache<Type, Dictionary<string, PropertyExtraInfo>> allValidatedProperties =
            new MemoizingMRUCache<Type, Dictionary<string, PropertyExtraInfo>>((x, _) =>
                    PropertyExtraInfo.CreateFromType(x).ToDictionary(k => k.PropertyName, v => v),
                5);

        private string GetPropertyValidationError(string propName)
        {
            PropertyExtraInfo pei;

            lock (allValidatedProperties)
            {
                if (!allValidatedProperties.Get(GetType()).TryGetValue(propName, out pei)) return null;
            }

            foreach (var v in pei.ValidationAttributes)
                try
                {
                    var ctx = new ValidationContext(this, null, null) { MemberName = propName };
                    var pi = pei.Type.GetProperty(pei.PropertyName,
                        BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                    v.Validate(pi.GetValue(this, null), ctx);
                }
                catch (Exception ex)
                {
                    this.Log().Info("{0:X}.{1} failed validation: {2}",
                        GetHashCode(), propName, ex.Message);
                    return ex.Message;
                }

            return null;
        }
    }

    internal class PropertyExtraInfo : IComparable
    {
        private string _typeFullName;
        private Type _type;

        public Type Type
        {
            get => _type;
            set
            {
                _type = value;
                _typeFullName = value.FullName;
            }
        }

        public string PropertyName { get; set; }
        public ValidationAttribute[] ValidationAttributes { get; set; }

        public static PropertyExtraInfo CreateFromTypeAndName(Type type, string propertyName,
            bool nullOnEmptyValidationAttrs = false)
        {
            var pi = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

            if (pi == null) throw new ArgumentException("Property not found on type");

            var attrs = pi.GetCustomAttributes(typeof(ValidationAttribute), true) ?? new ValidationAttribute[0];
            if (nullOnEmptyValidationAttrs && attrs.Length == 0) return null;

            return new PropertyExtraInfo()
            {
                Type = type,
                PropertyName = propertyName,
                ValidationAttributes = attrs.Cast<ValidationAttribute>().ToArray(),
            };
        }

        public static PropertyExtraInfo[] CreateFromType(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => CreateFromTypeAndName(type, x.Name, true))
                .Where(x => x != null)
                .ToArray();
        }

        public int CompareTo(object obj)
        {
            if (!(obj is PropertyExtraInfo rhs)) throw new ArgumentException();

            var ret = 0;
            if ((ret = _typeFullName.CompareTo(rhs._typeFullName)) != 0) return ret;

            return PropertyName.CompareTo(rhs.PropertyName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class ValidationBase : ValidationAttribute
    {
        public bool AllowNull = false;
        public bool AllowBlanks = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ret = base.IsValid(value, validationContext);
            if (ret == null || ret.ErrorMessage == null)
                return null;
            return getStandardMessage(validationContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool isValidViaNullOrBlank(object value)
        {
            if (value == null && !AllowNull)
                return false;

            return !(value is string s && !AllowBlanks && string.IsNullOrWhiteSpace(s));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected ValidationResult isValidViaNullOrBlank(object value, ValidationContext ctx)
        {
            if (isValidViaNullOrBlank(value))
                return null;

            return new ValidationResult($"{ctx.DisplayName ?? "The value"} is blank");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected virtual ValidationResult getStandardMessage(ValidationContext ctx)
        {
            return new ValidationResult(ErrorMessage ??
                                        $"{ctx.DisplayName ?? "The value"} is incorrect");
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValidatesViaMethodAttribute : ValidationBase
    {
        public string Name;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isBlank = isValidViaNullOrBlank(value, validationContext);
            if (isBlank != null)
                return isBlank;

            var func = Name ?? $"Is{validationContext.MemberName}Valid";
            var mi = validationContext.ObjectType.GetMethod(func, BindingFlags.Public | BindingFlags.Instance);
            var result = (bool)mi.Invoke(validationContext.ObjectInstance, new[] { value });

            return result ? null : getStandardMessage(validationContext);
        }
    }
}