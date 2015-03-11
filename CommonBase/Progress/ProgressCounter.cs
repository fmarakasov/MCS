using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CommonBase.Progress
{
    /// <summary>
    /// Класс для расчёта счётчика процентов выполненой работы от заданного полного числа шагов
    /// </summary>
    public class ProgressCounter
    {
        /// <summary>
        /// Получает общее число по работе
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Получает выполняемый шаг
        /// </summary>
        public int CurrenctStep { get; private set; }
        
        /// <summary>
        /// Получает прогресс выполнения в процентах [0-100]
        /// </summary>
        public int CurrentProgress
        {
            get { return (int)Percent.GetPercent(CurrenctStep, Count); }
        }
        /// <summary>
        /// Увеличивает текущий шаг на единицу
        /// </summary>
        public void Next()
        {
            Contract.Ensures(CurrenctStep <= Count);
            ++CurrenctStep;
        }

        public void Reset()
        {
            CurrenctStep = 0;
        }

       /// <summary>
       /// Создаёт экземпляр ProgressCounter для расчёта прогресса выполненной работы
       /// </summary>
       /// <param name="count">Полное число шагов</param>
        public ProgressCounter(int count)
        {            
            Count = count;
            CurrenctStep = 0;
        }
    }
}
