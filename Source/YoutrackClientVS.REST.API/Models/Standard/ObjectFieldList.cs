using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using YouTrack.REST.API.Serializers.Json;

namespace YouTrack.REST.API.Models.Standard
{
    public abstract class ObjectFieldList : DynamicObject
    {
        private readonly IDictionary<string, Field> _fields =
            new Dictionary<string, Field>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Issue fields.
        /// </summary>
        public ICollection<Field> Fields => _fields.Values;

        /// <summary>
        /// Gets a specific <see cref="Field"/> from the <see cref="Fields"/> collection.
        /// </summary>
        /// <param name="fieldName">The name of the <see cref="Field"/> to get.</param>
        /// <returns><see cref="Field"/> matching the <paramref name="fieldName"/>; null when not found.</returns>
        public Field GetField([CallerMemberName] string fieldName = "")
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException(nameof(fieldName));

            _fields.TryGetValue(fieldName, out var field);
            return field;
        }

        /// <summary>
        /// Sets a specific <see cref="Field"/> in the <see cref="Fields"/> collection.
        /// </summary>
        /// <param name="fieldName">The name of the <see cref="Field"/> to set.</param>
        /// <param name="value">The value to set for the <see cref="Field"/>.</param>
        public void SetField(object value, [CallerMemberName] string fieldName = "")
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException(nameof(fieldName));

            if (_fields.TryGetValue(fieldName, out var field))
                field.Value = value;
            else
                _fields.Add(fieldName, new Field { Name = fieldName, Value = value });
        }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var field = GetField(binder.Name)
                        ?? GetField(binder.Name.Replace("_",
                            " ")); // support fields with space in the name by using underscore in code

            if (field != null)
            {
                result = field.Value;
                return true;
            }

            result = null;
            return true;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // "field" setter when deserializing JSON into Issue object
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray)
            {
                var fieldElements = ((JArray)value).ToObject<List<Field>>();
                foreach (var fieldElement in fieldElements)
                {
                    if (fieldElement.Value is JArray fieldElementAsArray)
                        if (string.Equals(fieldElement.Name, "assignee", StringComparison.OrdinalIgnoreCase))
                        {
                            // For assignees, we can do a strong-typed list.
                            fieldElement.Value = fieldElementAsArray.ToObject<List<Assignee>>();
                        }
                        else
                        {
                            if (fieldElementAsArray.First is JValue &&
                                JTokenTypeUtil.IsSimpleType(fieldElementAsArray.First.Type))
                                fieldElement.Value = fieldElementAsArray.ToObject<List<string>>();
                            else
                                fieldElement.Value = fieldElementAsArray;
                        }

                    // Set the actual field
                    _fields[fieldElement.Name] = fieldElement;
                }

                return true;
            }

            // Regular setter
            if (_fields.TryGetValue(binder.Name, out var field) ||
                _fields.TryGetValue(binder.Name.Replace("_", " "), out field))
                field.Value = value;
            else
                _fields.Add(binder.Name, new Field { Name = binder.Name, Value = value });

            return true;
        }
        /// <summary>
        /// Returns the current <see cref="Issue" /> as a <see cref="DynamicObject" />.
        /// </summary>
        /// <returns>The current <see cref="Issue" /> as a <see cref="DynamicObject" />.</returns>
        public dynamic AsDynamic()
        {
            return this;
        }
    }
}