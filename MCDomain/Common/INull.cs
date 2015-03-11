namespace MCDomain.Common
{
    /// <summary>
    /// Интерфейс объектов, которые могут быть помечены как являющиеся Null-объектами
    /// </summary>
    /// <remarks>
    /// Использование этого интерфейса оправдано в тех случаях, когда существует сложная условная логика в системе, основанная на 
    ///  том, имеет ли некоторый объект значение null или нет. Для улучшения общей структуры приложения можно избавиться от значительного количества операторов
    ///  сравнения объектов со значением null, если принять, что методы (свойства), которые возвращают объекты не могут вернуть null, а вместо этого возвращают специальный Null-тип. 
    ///  Null-типы - это объекты, которые релизуют тот же интерфейс, что и реальные типы, но реализуют его в минимальном объёме. Таким образом узнать является ли объект реальным или он является Null-типом можно, использовав свойство IsNull.
    /// </remarks>
    /// <example>
    /// Представленный код показывает использование интерфейса для реализации метода поиска персоны, который никогда не возвращает значение null.
    /// <code>
    /// class Person : INull
    /// {
    ///     bool INull.IsNull
    ///     {
    ///        get
    ///        {
    ///           return false;
    ///        }
    ///     }
    ///     string override ToString()
    ///     {
    ///        return string.Format("{0} {1} {2}", FamilyName, MiddleName, FirstName);
    ///     }
    /// }
    /// class NullPerson : Person, INull
    /// {
    ///     public static readonly Instance = new NullPerson();
    ///     
    ///     bool INull.IsNull
    ///     {
    ///        get
    ///        {
    ///           return true;
    ///        }
    ///     }
    ///     string override ToString()
    ///     {
    ///        return "(Не найден)";
    ///     }
    /// }
    /// class Persons
    /// {
    ///    public Person FindPerson(int id)
    ///    {
    ///       Person result = DatabaseGateway<![CDATA[<Person>]]>.Find(id);
    ///       return result??NullPerson.Instance;
    ///    }
    /// }
    /// </code>
    /// </example>
    public interface INull
    {
        /// <summary>
        /// Получает состояние объекта. Возвращает значение ture, если объект является Null-объектом, в противном случае объект является реальным объектом.
        /// </summary>
        bool IsNull { get; }
    }
}