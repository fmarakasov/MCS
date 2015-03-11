using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace MContracts.Classes
{
    public class DefaultTelerikLocalizationManager : LocalizationManager
    {

        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "GridViewGroupPanelTopTextGrouped":
                    return "Группировка:";
                case "GridViewGroupPanelText":
                    return "Группировка";
                    //---------------------- RadGridView Filter Dropdown items texts:
                case "GridViewClearFilter":
                    return "Очистить фильтр";
                case "GridViewFilterShowRowsWithValueThat":
                    return "Показать строки со значением";
                case "GridViewFilterSelectAll":
                    return "Выделить всё";
                case "GridViewFilterContains":
                    return "Содержит";
                case "GridViewFilterEndsWith":
                    return "Заканчивающиеся";
                case "GridViewFilterDoesNotContain":
                    return "Не содержит";
                case "GridViewFilterIsNotContainedIn":
                    return "Не содержится в";
                case "GridViewFilterIsContainedIn":
                    return "Содержится в";
                case "GridViewFilterMatchCase":
                    return "С учётом регистра";
                case "GridViewFilterIsEqualTo":
                    return "Равно";
                case "GridViewFilterIsGreaterThan":
                    return "Больше";
                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "Больше или равно";
                case "GridViewFilterIsLessThan":
                    return "Меньше";
                case "GridViewFilterIsLessThanOrEqualTo":
                    return "Меньше или равно";
                case "GridViewFilterIsNotEqualTo":
                    return "Не равно";
                case "GridViewFilterStartsWith":
                    return "Начинается с";
                case "GridViewFilterAnd":
                    return "И";
                case "GridViewFilterOr":
                    return "Или";

                case "GridViewFilter":
                    return "Фильтр";
            }
            return base.GetStringOverride(key);
        }
    }
}
