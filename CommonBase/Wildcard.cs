using System.Text.RegularExpressions;

namespace CommonBase
{
    /// <summary>
    /// Представляет поиск по шаблонам (wildcards) основанный на
    /// <see cref="System.Text.RegularExpressions"/>.
    /// </summary>
    public class Wildcard : Regex
    {
        /// <summary>
        /// Создаёт новый экземпляр Wildcard для проверки соответствия
        /// </summary>
        /// <param name="pattern">Шаблон</param>
        public Wildcard(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }

        /// <summary>
        /// Создаёт новый экземпляр Wildcard с указанием опции поиска
        /// </summary>
        /// <param name="pattern">Шаблон</param>
        /// <param name="options">Флаги
        /// <see cref="RegexOptions"/>.</param>
        public Wildcard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {
        }

        /// <summary>
        /// Преобразует строку шаблона в строку регулярного выражения
        /// </summary>
        /// <param name="pattern">Шаблон для конвертирования.</param>
        /// <returns>Строка регулярного выражения, соответтсвующая шаблону</returns>
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
             Replace("\\*", ".*").
             Replace("\\?", ".") + "$";
        }
    }

}
