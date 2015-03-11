using System;

namespace MCDomain.Model
{
    /// <summary>
    /// Получает текущее системное время 
    /// </summary>
    class DefaultTimeResolver : ITimeResolver
    {
        /// <summary>
        /// Возвращает экземпляр DefaultTimeResolver
        /// </summary>
        public static readonly DefaultTimeResolver Instance = new DefaultTimeResolver();

        /// <summary>
        /// Получает текущее системное время
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Today; }
        }
    }
}