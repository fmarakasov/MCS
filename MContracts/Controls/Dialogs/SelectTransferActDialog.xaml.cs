using System;
using System.Collections.Generic;
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
using CommonBase;
using MCDomain.Model;
using MContracts.ViewModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SelectTransferActDialog.xaml
    /// </summary>
    public partial class SelectTransferActDialog : Window
    {
        public SelectTransferActDialog()
        {
            InitializeComponent();
        }
        private SelectTransferActDialogViewModel ViewModel
        {
            get { return DataContext.CastTo<SelectTransferActDialogViewModel>(); }
        }
        private void GridViewDataControl_OnAddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            var act = e.NewObject as Transferact;
            act.Do(x=>x.Transferacttype = ViewModel.ActType);

        }

        private void GridViewDataControl_OnDeleting(object sender, GridViewDeletingEventArgs e)
        {
            e.Items.Cast<Transferact>().Do(x => x.Apply(item => item.Transferacttype = null));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
