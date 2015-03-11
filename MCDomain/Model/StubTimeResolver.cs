using System;

namespace MCDomain.Model
{
    /// <summary>
    /// Заглушка для тестирования типов,зависимых от текущего времени 
    /// </summary>
    public class StubTimeResolver : ITimeResolver
    {
        private DateTime _now;
        /// <summary>
        /// Создаёт экземпляр заглушки.
        /// </summary>
        /// <param name="now">Время, которое должно возвращать заглушка</param>
        public StubTimeResolver(DateTime now)
        {
            _now = now;
        }
        /// <summary>
        /// Получает время, которое задано при создании заглушки.
        /// </summary>
        public DateTime Now
        {
            get { return _now; }
        }
    }
}