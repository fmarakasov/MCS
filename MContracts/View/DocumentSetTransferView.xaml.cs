using System.Windows;
using System.Windows.Controls;
using CommonBase;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using UIShared.Common;

namespace MContracts.View
{
    /// <summary>
    /// Логика взаимодействия для ContractTrandocView.xaml
    /// </summary>
    public partial class DocumentSetTransferView : UserControl
    {
        public DocumentSetTransferView()
        {
            InitializeComponent();
        }

        private void DocumentSetTransferView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            e.DependencyPropertyAction<DocumentSetTransferViewModel>((oldVal, newVal) =>
                {
                    newVal.Do(x=>x.SelectTransferActRequest += newVal_SelectTransferActRequest);
                    oldVal.Do(x=>x.SelectTransferActRequest -= newVal_SelectTransferActRequest);
                });
        }

        DocumentSetTransferViewModel ViewModel
        {
            get { return DataContext.CastTo<DocumentSetTransferViewModel>(); }
        }
        void newVal_SelectTransferActRequest(object sender, CommonBase.EventParameterArgs<TransferActSelector> e)
        {
            var vm = new SelectTransferActDialogViewModel(ViewModel.Repository)
                {
                    ActType = e.Parameter.PrefferedActType,
                    Selected = e.Parameter.Act
                };

            var dlg = new SelectTransferActDialog
                {
                    DataContext = vm
                };
            var dlgResult = dlg.ShowDialog();

            if (!dlgResult.GetValueOrDefault()) e.Parameter.Act = null;
            e.Parameter.Act = vm.Selected;
        }

  
    }
}
