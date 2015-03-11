using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace MCDomain.Common
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public object Source { get; private set; }
        
        public NotifyPropertyChangedBase()
        {
            Init(this);
        }

        public NotifyPropertyChangedBase(object source)
        {
            Init(source);
        }

        private void Init(object source)
        {
            Contract.Requires(source !=null);
            Contract.Ensures(source == Source);
            Source = source;
        }

        [Conditional("DEBUG")]
        //[DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            var t = TypeDescriptor.GetProperties(this.Source)[propertyName];           

            if (t == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        private readonly Dictionary<string, PropertyChangedEventArgs> _propertyChangedEventArgsCache = 
            new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                PropertyChangedEventArgs eventArgs;

                if (!_propertyChangedEventArgsCache.TryGetValue(propertyName, out eventArgs))
                {
                    eventArgs = new PropertyChangedEventArgs(propertyName);
                    _propertyChangedEventArgsCache.Add(propertyName, eventArgs);
                }

                handler(this.Source, eventArgs);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}