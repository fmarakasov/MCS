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
using MCDomain.Model;
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Логика взаимодействия для ActRepositoryViewBasedView.xaml
    /// </summary>
    public partial class ActRepositoryViewBasedView : UserControl
    {
        public ActRepositoryViewBasedView()
        {
            InitializeComponent();
        }

        private void ActsDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

       
        private void ActRepositoryViewBasedView_OnInitialized(object sender, EventArgs e)
        {
            ActsDataGrid.Settings.SelectSpecifiedItems += Settings_SelectSpecifiedItems;
            ActsDataGrid.Settings.SelectionIdsRequire += Settings_SelectionIdsRequire;
        }

        void Settings_SelectionIdsRequire(object sender, CommonBase.EventParameterArgs<ObjectIdentifierSelector> e)
        {
            e.Parameter.Id = e.Parameter.Item.Return(x => x.CastTo<Actrepositoryview>().Id, default(long?));
        }

        void Settings_SelectSpecifiedItems(object sender, CommonBase.EventParameterArgs<ObjectSelector> e)
        {
            e.Parameter.Item = DataContext.CastTo<ActRepositoryViewBasedViewModel>().Acts.SingleOrDefault(x => x.Id == e.Parameter.Id);
        }
    }
}
