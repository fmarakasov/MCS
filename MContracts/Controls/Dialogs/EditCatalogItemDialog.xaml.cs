using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Classes.Converters;
using MContracts.ViewModel;
using System.Diagnostics.Contracts;
using Microsoft.Windows.Controls;
using Telerik.Windows.Controls;

namespace MContracts.Dialogs
{
   
    /// <summary>
    /// Interaction logic for ServerConnectionDialog.xaml
    /// </summary>
    public partial class EditCatalogItemDialog : Window
    {
        public EditCatalogViewModel ViewModel = null;

        public CatalogType CatalogType
        {
            set
            {
                SetTemplate(value);
            }
        }

        private void SetTemplate(Classes.CatalogType value)
        {
            ObjectForEdit.Template = Resources[value.ToString()] as ControlTemplate;
        }
        
        public EditCatalogItemDialog(IContractRepository repository)
        {
            InitializeComponent();
            ViewModel = new EditCatalogViewModel(repository);
            DataContext = ViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var errorInfo = ViewModel.ObjectForEdit as IDataErrorInfo;
            if (errorInfo != null)
                if (errorInfo.Error != String.Empty)
                {
                    AppMessageBox.Show(
                        "Действие не может быть выполнено. Устраните ошибки.\n\n" +
                        (ViewModel.ObjectForEdit as IDataErrorInfo).Error, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            if (ViewModel.SaveCommand.CanExecute(this))
                ViewModel.SaveCommand.Execute(null);
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = new ActionTypeToStringConverter().Convert(ViewModel.ActionType, typeof(ActionType), null, null).ToString();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            (ViewModel.ObjectForEdit as Person).Sex = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            (ViewModel.ObjectForEdit as Person).Sex = false;
        }

        private void ManRB_Loaded(object sender, RoutedEventArgs e)
        {
            bool? sex = (ViewModel.ObjectForEdit as Person).Sex;

            if (sex.HasValue && sex.Value)
                (sender as RadioButton).IsChecked = true;
        }

        private void WomanRB_Loaded(object sender, RoutedEventArgs e)
        {
            bool? sex = (ViewModel.ObjectForEdit as Person).Sex;

            if (sex.HasValue && !sex.Value)
                (sender as RadioButton).IsChecked = true;
        }

        private void ManRB_Checked(object sender, RoutedEventArgs e)
        {
            (ViewModel.ObjectForEdit as Employee).Sex = true;
        }

        private void WomanRB_Checked(object sender, RoutedEventArgs e)
        {
            (ViewModel.ObjectForEdit as Employee).Sex = false;
        }

        private void ManRB_Loaded_1(object sender, RoutedEventArgs e)
        {
            bool? sex = (ViewModel.ObjectForEdit as Employee).Sex;

            if (sex == null||sex.Value)
                (sender as RadioButton).IsChecked = true;
        }

        private void WomanRB_Loaded_1(object sender, RoutedEventArgs e)
        {
            bool? sex = (ViewModel.ObjectForEdit as Employee).Sex;

            if (sex != null&&!sex.Value)
                (sender as RadioButton).IsChecked = true;
        }

        private void AddResponsible(ListBox lbx)
        {
            Contract.Requires(ViewModel != null);
            object item = lbx.SelectedItem;
            if (item != null)
                ViewModel.AddResponsibleCommand.Execute(item);
        }

        private void RemoveResponsible(ListBox lbx)
        {
            Contract.Requires(ViewModel != null);
            object item = lbx.SelectedItem;
            if (item != null)
                ViewModel.RemoveResponsibleCommand.Execute(item);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            AddResponsible(lbxAll);
            
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            RemoveResponsible(lbxSelected);
        }

        private ListBox lbxSelected;
        private ListBox lbxAll;
        private void lbxSelectedEmps_Loaded(object sender, RoutedEventArgs e)
        {
            lbxSelected = sender as ListBox;
        }

        private void lbxAllEmps_Loaded(object sender, RoutedEventArgs e)
        {
            lbxAll = sender as ListBox;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SendPropertyChanged("ParentFunctionalcustomers");
        }

        private TextBox txApprovalState; 
        private void RadColorPicker_OnSelectedColorChanged(object sender, EventArgs e)
        {
            if (txApprovalState != null)
                txApprovalState.Background = new SolidColorBrush((sender as ColorPicker).SelectedColor);
        }

        private void txtApprovalState_Initialized(object sender, EventArgs e)
        {
            txApprovalState = sender as TextBox;
        }

        public ComboBox cbQuarter { get; set; }
        private void RadColorPicker_OnSelectedColorChanged1(object sender, EventArgs e)
        {
            if (cbQuarter != null)
                cbQuarter.Background = new SolidColorBrush((sender as ColorPicker).SelectedColor);
        }

        private void cbxQuarter_Initialized(object sender, EventArgs e)
        {
            cbQuarter = sender as ComboBox;
        }

 
    }
}
