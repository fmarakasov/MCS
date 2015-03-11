using System;
using System.ComponentModel;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace McReports.DTO
{
    /// <summary>
    /// Базовый класс DTO, которые умеют преобразовывать себя в объект модели предметной области
    /// </summary>
    /// <typeparam name="T">Тип сущности модели предметной области</typeparam>
    abstract public class EntityDto<T>: NotifyPropertyChangedBase, IDataErrorInfo where T : class, new()
    {
        private T _entity;

        /// <summary>
        /// Преобразует DTO в объект модели предметной области
        /// </summary>
        public T AsEntity
        {
            get
            {
                if (_entity == null)
                {
                    _entity = new T();
                }
                InitializeEntity(_entity);
                return _entity;
            }
        }
        /// <summary>
        /// Переопределите в производном классе для инициализации объекта
        /// </summary>
        /// <param name="entity">Объект предметной области</param>
        public abstract void InitializeEntity(T entity);

        public string this[string columnName]
        {
            get
            {
                var ei = AsEntity as IDataErrorInfo;
                if (ei != null) return ei[columnName];
                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                var ei = AsEntity as IDataErrorInfo;
                if (ei != null) return ei.Error;
                return string.Empty;
            }
        }
    }
}
