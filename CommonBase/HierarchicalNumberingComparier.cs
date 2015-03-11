using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonBase
{
    /// <summary>
    /// Компаратор многоуровневых списков
    /// </summary>
    public class HierarchicalNumberingComparier: IComparer<string>
    {
        private readonly Regex _regex;

        /// <summary>
        /// Создаёт экземпляр HierarchicalNumberingComparier
        /// </summary>
        public HierarchicalNumberingComparier()
        {
            _regex = new Regex(@"(?<num>\d+)");
        }
        /// 
        /// <summary>
        /// Получает коллекцию многоуровневых элементов из строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string[] Matches(string input)
        {
            return _regex.Matches(input)
                .Cast<Match>().Where(m => m.Success)
                .Select(m=>m.Groups["num"])
                .Where(m=>m.Success)
                .Select(m=>m.Value).ToArray();
        }

        /// <summary>
        /// Преобразует коллекцию строковых представлений чисел в целочисленную коллекцию 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[] Digits(IEnumerable<string> input)
        {
            return input.Select(x => Convert.ToInt32(x)).ToArray();
        }
        
        /// <summary>
        /// Сравнивает два многоуровневых элемента списка
        /// </summary>
        /// <param name="x">Первый элемент</param>
        /// <param name="y">Второй элемент</param>
        /// <returns>Результат сравнения</returns>
        public int Compare(string x, string y)
        {
            var xMatches = Digits(Matches(x));
            var yMatches = Digits(Matches(y));
            var xLen = xMatches.Length;
            var yLen = yMatches.Length;

            if (xLen == 0)
                return (yLen == 0) ? 0 : -1;
            if (yLen == 0)
                return 1;


            for (var i = 0; i < xLen; ++i)
            {
                if (i >= yLen) return 1;
                var cmp = Comparer.Default.Compare(xMatches[i], yMatches[i]);
                if (cmp != 0) return cmp;
            }

            if (yLen > xLen) return -1;

            return 0;
        }

        /// <summary>
        /// Получает единственный экземпляр компаратора многоуровневых списокв
        /// </summary>
        public static readonly HierarchicalNumberingComparier Instance = new HierarchicalNumberingComparier();

       
    }
}
