using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;

namespace MCDomain.Model
{
    public class EditableObject<T> : IEditableObject where T : class, ICloneable, new()
    {
        public readonly T  _wrappedObject;
        public T _backupObject;
        private bool _inTx = false;

        public Action DeleteCopy { get; set; }
        
        public EditableObject(T wrappedObject)
        {
            Contract.Requires(wrappedObject != null);
            _wrappedObject = wrappedObject;
        }

        public void BeginEdit()
        {
            if (!_inTx)
            {
                //_backupObject = _wrappedObject.Clone() as T;
                _inTx = true;
            }
        }

        public void EndEdit()
        {
            if (_inTx)
            {
                if (DeleteCopy != null)
                    DeleteCopy.Invoke();

                _backupObject = null;

                _inTx = false;
            }
        }

        public void CancelEdit()
        {
            if (_inTx)
            {
                //Copy(_backupObject, _wrappedObject);
                _inTx = false;

                if (DeleteCopy != null)
                    DeleteCopy.Invoke();
            }
        }

        private void Copy(T source, T target)
        {
            Contract.Assert(source != null);
            Contract.Assert(target != null);
            if (source.Equals(target)) return;

            var pi = source.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.Name != "Id")
                {
                    if ((!(propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.IsGenericType)) || propertyInfo.PropertyType.IsValueType)
                        propertyInfo.SetValue(target, propertyInfo.GetValue(source, null), null);
                }
            }
        }
    }
}