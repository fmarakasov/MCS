using System;
using System.Diagnostics.Contracts;



namespace CommonBase
{
    public static class Customperson
    {
        /// <summary>
        /// Получает имя персоны в формате И. О. Фамилия
        /// </summary>
        public static string GetShortFullName(this IPerson source)
        {
            Contract.Requires(source != null);
            Contract.Assert(IsValidPersonName(source));

            if (!string.IsNullOrWhiteSpace(source.Firstname) && !string.IsNullOrWhiteSpace(source.Middlename))
                return string.Format("{0}. {1}. {2}", Char.ToUpper(source.Firstname[0]), Char.ToUpper(source.Middlename[0]),
                                     source.Familyname);
            return source.Familyname;

        }

        /// <summary>
        /// Получает имя персоны в формате Фамилия И.О.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetShortFullNameRev(this IPerson source)
        {
            Contract.Requires(source != null);
            Contract.Assert(IsValidPersonName(source));
            if (!string.IsNullOrWhiteSpace(source.Firstname) && !string.IsNullOrWhiteSpace(source.Middlename))
                return string.Format("{0} {1}. {2}.", source.Familyname, Char.ToUpper(source.Firstname[0]), Char.ToUpper(source.Middlename[0]));
            return source.Familyname;
        }



        /// <summary>
        /// Возвращает имя человека в формате И. О. Фамилия, 
        /// Если нет фамилии возвращает пустую строку. Если нет И или О - строку без И или О.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Строка с именем человека в формате И. О. Фамилия или пустую строку, если нет фамилии</returns>
        public static string GetShortNameForReports(this IPerson source)
        {
            if (source == null) return string.Empty;
            string firstName = string.Empty;
            string middleName = string.Empty;
            if (source.Firstname != null)  firstName = string.Format("{0}. ", Char.ToUpper(source.Firstname[0]));
            if (source.Middlename != null) middleName= string.Format("{0}. ", Char.ToUpper(source.Middlename[0]));
            if (source.Familyname != null) return firstName + middleName + source.Familyname;
            return string.Empty;
        }

        public static bool IsValidPersonName(this IPerson source)
        {
            return IsValidPersonName(source.Firstname, source.Middlename, source.Familyname);
        }

        /// <summary>
        /// Получает полное имя персоны
        /// </summary>        
        public static string GetFullName(this IPerson source)
        {
            Contract.Requires(source != null);
            Contract.Assert(IsValidPersonName(source), "Имя персоны задано не корректно.");
            return string.Format("{0} {1} {2}", source.Firstname, source.Middlename, source.Familyname);
        }

        /// <summary>
        /// Получает признак, что переданное ФИО являются корректными
        /// </summary>
        /// <param name="firstname">Имя</param>
        /// <param name="middlename">Отчество</param>
        /// <param name="familyname">Фамилия</param>
        /// <returns>Признак корректности</returns>
        public static bool IsValidPersonName(string firstname, string middlename, string familyname)
        {
            return //!string.IsNullOrWhiteSpace(firstname) && !string.IsNullOrWhiteSpace(middlename) &&
                   !string.IsNullOrWhiteSpace(familyname);
        }            
    }
}
