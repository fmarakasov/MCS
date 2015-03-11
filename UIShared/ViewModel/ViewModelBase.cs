using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using MediatorLib;
using CommonBase;


namespace UIShared.ViewModel
{
    /// <summary>
    ///  Base class for all ViewModel classes in the application.
    /// It provides support for property change notifications 
    /// and has a DisplayName property.  This class is abstract.
    /// </summary>   
    public class ViewModelBase : DynamicObject, INotifyPropertyChanged,
        IDisposable, IDataErrorInfo
    {

        /// <summary>
        /// Получает уникальный идентификатор экземпляра модели представления
        /// </summary>
        public Guid InstanceId { get; private set; }


        private object _wrappedDomainObject;

        private bool _isBusy;
        /// <summary>
        /// Получает или устанавливает признак, что модель представления занята обработкой данных
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public string BusyContent
        {
            get { return _busyContent; }
            set
            {
                if (_busyContent == value) return;
                _busyContent = value;
                OnPropertyChanged(() => BusyContent);
            }
        }

        private string _busyState;
        /// <summary>
        /// Получает или устанавливает статусную строку для состояния обработки данных в модели представления
        /// </summary>
        public string BusyState
        {
            get { return _busyState; }
            set
            {
                if (_busyState == value) return;
                _busyState = value;
                OnPropertyChanged(() => BusyState);
            }
        }

        public object WrappedDomainObject
        {
            get { return _wrappedDomainObject; }
            set
            {
                Contract.Ensures(_wrappedDomainObject == value);
                if (_wrappedDomainObject == value) return;

                Debug.WriteLine("WrappedDomainObject для модели представления InstanceId={0} ({1}) был изменён с {2} на {3}.",
                    InstanceId, this, WrappedDomainObject ?? "null", value ?? "null");

                _wrappedDomainObject = value;
                OnWrappedDomainObjectChanged();

            }
        }
        /// <summary>
        /// Вызывает событие OnPropertyChanged. Имя свойства задаётся выражением вида ()=>Имя свойства
        /// </summary>
        /// <param name="expression">Выражение</param>
        protected void OnPropertyChanged(Expression<Func<object>> expression)
        {
            Contract.Requires(expression != null);
            string propertyName = GetPropertyName(expression);
            if (propertyName != null)
                OnPropertyChanged(propertyName);
        }

        string GetPropertyName(Expression<Func<object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }
        public void RefreshWrappedDomainObject()
        {
            OnWrappedDomainObjectChanged();

        }

        protected virtual void OnWrappedDomainObjectChanged()
        {
            OnPropertyChanged(() => WrappedDomainObject);
        }

        /// <summary>
        /// Получает глобальный экземпляр объекта-медиатора моделей представления
        /// </summary>
        public static readonly Mediator ViewMediator = new Mediator();

        #region Constructor

        protected ViewModelBase(ViewModelBase owner = null)
        {
            InstanceId = Guid.NewGuid();
            Owner = owner;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (WrappedDomainObject == null)
            {
                result = null;
                return false;
            }

            string propertyName = binder.Name;
            PropertyInfo property =
                this.WrappedDomainObject.GetType().GetProperty(propertyName);

            if (property == null || property.CanRead == false)
            {
                result = null;
                return false;
            }

            result = property.GetValue(this.WrappedDomainObject, null);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (WrappedDomainObject == null)
            {
                return false;
            }

            string propertyName = binder.Name;
            PropertyInfo property =
              this.WrappedDomainObject.GetType().GetProperty(propertyName);

            if (property == null || property.CanWrite == false)
                return false;

            property.SetValue(this.WrappedDomainObject, value, null);
            OnPropertyChanged(propertyName);
            return true;
        }




        #endregion // Constructor

        #region DisplayName

        /// <summary>
        /// Returns the user-friendly name of this object.
        /// Child classes can set this property to a new value,
        /// or override it to determine the value on-demand.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        #endregion // DisplayName

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            var hasProperty = IsValidPropertyName(propertyName);

            if (!hasProperty)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }
        public bool IsValidPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            var t = TypeDescriptor.GetProperties(this)[propertyName];
            if (t == null && WrappedDomainObject != null)
                t = TypeDescriptor.GetProperties(WrappedDomainObject)[propertyName];
            return (t != null);

        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, PropertyChangedEventArgs> _propertyChangedEventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
        private string _busyContent;

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

                handler(this, eventArgs);
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

#if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif

        #endregion // IDisposable Members

        public virtual string Error
        {
            get
            {
                IDataErrorInfo dataErrorInfo = (WrappedDomainObject as IDataErrorInfo);
                if (dataErrorInfo != null)
                {
                    return dataErrorInfo.Error;
                }
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                IDataErrorInfo dataErrorInfo = (WrappedDomainObject as IDataErrorInfo);
                if (dataErrorInfo != null)
                {
                    return dataErrorInfo[columnName];
                }
                return null;
            }
        }

        /// <summary>
        /// Получает или устанавливает родительское "модель-представление"
        /// </summary>
        public ViewModelBase Owner { get; protected set; }

        /// <summary>
        /// Используется для возврата диагностической информации, связанной с этой моделью представления
        /// Переопределите в производных классах для получения дополнительных данных.
        /// Диагностика записывается в свойство Exception.Data
        /// </summary>
        /// <param name="exception">Исключение, для которого требуется собрать диагностические данные</param>
        public virtual void CollectDiagnosticsData(Exception exception)
        {
            Contract.Requires(exception != null);
            CollectPropertiesData(exception.Data);

        }

        private void CollectPropertiesData(IDictionary data)
        {
            data.Add(this.ToString(), this.ToString());

            var pis = GetType().GetProperties();

            foreach (var propertyInfo in pis)
            {
                try
                {

                    if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
                        data.Add(propertyInfo.Name, propertyInfo.GetValue(this, null).Return(x => x.ToString(), "null"));
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }



}