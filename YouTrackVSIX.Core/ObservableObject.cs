using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YouTrackVSIX.Core.System.ComponentModel;

namespace YouTrackVSIX.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the new value.
        /// </summary>
        /// <typeparam name="T">The type of the property to modify.</typeparam>
        /// <param name="currentValue">The current value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns><c>Ture</c>, if there has been a change.</returns>
        public bool SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            return PropertyChanged.SetProperty(this, ref currentValue, newValue, propertyName);
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    namespace System.ComponentModel
    {
        public static class ObservableObject
        {
            //Just adding some new funk.tionality to System.ComponentModel
            public static bool SetProperty<T>(this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
            {
                if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                    return false;

                currentValue = newValue;

                handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
                return true;
            }
        }
    }
}