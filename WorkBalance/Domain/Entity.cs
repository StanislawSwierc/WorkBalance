using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using ReactiveUI;

namespace WorkBalance.Domain
{
    public abstract class Entity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Methods

        protected bool Set<T>(ref T field, T value, string propertyName)
        {
            Contract.Requires(propertyName != null);
            VerifyPropertyName(propertyName);
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            RaisePropertyChanging(propertyName);
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) 
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RaisePropertyChanging(string propertyName)
        {
            var handler = PropertyChanging;
            if (handler != null) 
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // If you raise PropertyChanged and do not specify a property name,
            // all properties on the object are considered to be changed by 
            // the binding system.
            if (String.IsNullOrEmpty(propertyName))
                return;

            // Verify that the property name matches a real, public, instance
            // property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("propertyName");
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion
    }
}
