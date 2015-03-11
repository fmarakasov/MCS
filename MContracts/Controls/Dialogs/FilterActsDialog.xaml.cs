using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Classes.Converters;
using UIShared.Commands;
using MContracts.ViewModel;

namespace MContracts.Dialogs
{
   
    /// <summary>
    /// Interaction logic for ServerConnectionDialog.xaml
    /// </summary>
    public partial class FilterActsDialog : Window
    {
        private CheckBox commonCheckBox = null;
        
        public FilterActsDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            commonCheckBox = this.GetCheckBoxWithParent(ContractsDataGrid, typeof (CheckBox), "CommonCheckBox");
        }

        public List<Contractdoc> Contracts { get; set; }

        public List<Contractdoc> SelectedContracts { get; set; }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as CheckBox).IsChecked = SelectedContracts.Select(x=>x.Id).Contains(((sender as CheckBox).DataContext as Contractdoc).Id);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            long ID = ((sender as CheckBox).DataContext as Contractdoc).Id;
            if (!SelectedContracts.Select(x=>x.Id).Contains(ID))
            {
                SelectedContracts.Add(((sender as CheckBox).DataContext as Contractdoc));
            }
            
            if (SelectedContracts.Count == Contracts.Count)
            {
                commonCheckBox.IsChecked = true;
            }
            else
            {
                commonCheckBox.IsChecked = null;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            long ID = ((sender as CheckBox).DataContext as Contractdoc).Id;
            if (SelectedContracts.Select(x=>x.Id).Contains(ID))
            {
                SelectedContracts.Remove(SelectedContracts.Single(x => x.Id == ID));
            }
            
            if (SelectedContracts.Count == 0)
            {
                commonCheckBox.IsChecked = false;
            }
            else
            {
                commonCheckBox.IsChecked = null;
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            foreach (Contractdoc contractdoc in Contracts)
            {
                if (!SelectedContracts.Select(x=>x.Id).Contains(contractdoc.Id))
                {
                    SelectedContracts.Add(contractdoc);
                }
            }

            ContractsDataGrid.ItemsSource = null;
            ContractsDataGrid.ItemsSource = Contracts;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            SelectedContracts.Clear();

            ContractsDataGrid.ItemsSource = null;
            ContractsDataGrid.ItemsSource = Contracts;
        }

        public CheckBox GetCheckBoxWithParent(UIElement parent, Type targetType, string ElementName)
        {
            if (parent.GetType() == targetType && ((CheckBox)parent).Name == ElementName)
            {
                return (CheckBox)parent;
            }
            CheckBox result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (GetCheckBoxWithParent(child, targetType, ElementName) != null)
                {
                    result = GetCheckBoxWithParent(child, targetType, ElementName);
                    break;
                }
            }
            return result;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(ContractsDataGrid.ItemsSource);
            collectionView.Filter = (item) =>
            {
                return FilterItem(item, (sender as TextBox).Text);
            };
        }

        private bool FilterItem(object item, object value)
        {
            var pi = item.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                //берем все простые свойства, которые можно читать и писать, кроме ID и ссылки на родителя
                if (propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.Name != "Id" && !propertyInfo.PropertyType.IsGenericType && item.GetType() != propertyInfo.PropertyType)
                {
                    var val = propertyInfo.GetValue(item, null);

                    if (val != null)
                    {
                        if (val.ToString().ToLower().Contains(value.ToString().ToLower()))
                            return true;
                    }
                }
            }

            return false;
        }

    }
}
