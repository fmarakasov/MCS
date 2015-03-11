using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Dialogs;
using MContracts.ViewModel;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for MultiEditActWindow.xaml
    /// </summary>
    public partial class MultiEditActWindow : Window
    {
        public MultiEditActViewModel viewModel = null;

        public MultiEditActWindow()
        {
            InitializeComponent();
        }

        public MultiEditActWindow(IContractRepository _repository)
        {
            InitializeComponent();
            viewModel = new MultiEditActViewModel(_repository);
            this.DataContext = viewModel;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.EditActs();
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #region CatalogsAdded

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            InsertInCatalog(CatalogType.Contractor, typeof(Contractor));
        }

        private void InsertInCatalog(CatalogType catalogType, Type type)
        {
            foreach (UIElement ctrl in this.LayoutRoot.Children)
            {
                ctrl.Opacity = 0.5;
            }

            EditCatalogItemDialog dialog = new EditCatalogItemDialog(viewModel.Repository);
            dialog.Closed += new EventHandler(dialog_Closed);
            dialog.CatalogType = catalogType;
            dialog.ViewModel.ActionType = ActionType.Add;
            dialog.ViewModel.ObjectForEdit = Activator.CreateInstance(type) as IDataErrorInfo;
            dialog.ShowDialog();
        }

        void dialog_Closed(object sender, EventArgs e)
        {
            var dlg = sender as EditCatalogItemDialog;
            Contract.Assert(dlg != null);
            if (dlg.DialogResult == true)
            {
                if (dlg.ObjectForEdit is Contractor)
                    viewModel.Repository.InsertContractor((sender as EditCatalogItemDialog).ViewModel.ObjectForEdit as Contractor);
                if (dlg.ObjectForEdit is Acttype)
                    viewModel.Repository.InsertActtype((sender as EditCatalogItemDialog).ViewModel.ObjectForEdit as Acttype);

                viewModel.Repository.SubmitChanges();
                viewModel.ActtypesChanged();
            }

            foreach (UIElement ctrl in LayoutRoot.Children)
            {
                ctrl.Opacity = 1;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            InsertInCatalog(CatalogType.Acttype, typeof(Acttype));
        }

    #endregion

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Label).FontWeight = ((sender as Label).DataContext as ActRepositoryEntity) == viewModel.Acts[0]
                ? FontWeights.Black : FontWeights.Normal;
        }

    }
}
