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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonBase;
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Логика взаимодействия для ContractTransActRepositoryView.xaml
    /// </summary>
    public partial class ContractTransActRepositoryView : UserControl
    {
        public ContractTransActRepositoryView()
        {
            InitializeComponent();
        }

        private DocumentTransferActsRepositoryViewModel ViewModel
        {
            get { return DataContext.CastTo<DocumentTransferActsRepositoryViewModel>(); }
        }

        private void ActsGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            actsGridView.Settings.SelectSpecifiedItems += SettingsSelectSpecifiedItems;
            actsGridView.Settings.SelectionIdsRequire += SettingsSelectionIdsRequire;
        }

        private void ContractTransActRepositoryView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            actsGridView.Settings.SelectSpecifiedItems -= SettingsSelectSpecifiedItems;
            actsGridView.Settings.SelectionIdsRequire -= SettingsSelectionIdsRequire;
        }

        void SettingsSelectSpecifiedItems(object sender, EventParameterArgs<ObjectSelector> e)
        {
            e.Parameter.Item = ViewModel.Documenttransacts.SingleOrDefault(x => x.Id == e.Parameter.Id);
        }

        static void SettingsSelectionIdsRequire(object sender, EventParameterArgs<ObjectIdentifierSelector> e)
        {
            e.Parameter.Id = e.Parameter.Item.Return(x => x.CastTo<Documenttransactdto>().Id, default(long?));
        }
    }
}
