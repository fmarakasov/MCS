using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace UIShared.Common
{
    /// <summary>
    /// Класс для расширения класса FrameworkElement
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// метод для поиска контрола в контейнере
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="parent">Контейнер</param>
        /// <param name="ElementName">Имя элемента</param>
        /// <returns>Искомый элемент</returns>
        public static T GetElementWithParent<T>(this FrameworkElement parent, string ElementName) where T: FrameworkElement
        {
            if (parent == null) return null;
            
            if (parent.GetType() == typeof(T) && ((FrameworkElement)parent).Name == ElementName)
            {
                return parent as T;
            }
            FrameworkElement result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                FrameworkElement child = (FrameworkElement)VisualTreeHelper.GetChild(parent, i);

                if (child.GetElementWithParent<T>(ElementName) != null)
                {
                    result = child.GetElementWithParent<T>(ElementName);
                    break;
                }
            }
            return result as T;
        }

        public static DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }

        public static IEnumerable<DataGridRow> GetRows(this DataGrid grid)
        {
            List<DataGridRow> list = new List<DataGridRow>();
            foreach (object o in grid.ItemsSource)
            {
                list.Add(grid.ItemContainerGenerator.ContainerFromItem(o) as DataGridRow);
            }
            return list;
        }

        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        /// <summary>
        /// Проверят все дочерние элементы на валидность
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this T source) where T : DependencyObject
        {
            if (source == null) throw new ArgumentNullException("source");
            // The dependency object is valid if it has no errors, 
            //and all of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(source) &&
                   LogicalTreeHelper.GetChildren(source)
                       .OfType<DependencyObject>()
                       .All(child => IsValid(child));
        }
    }
}
