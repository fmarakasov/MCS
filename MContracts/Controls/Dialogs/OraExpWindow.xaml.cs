using System;
using System.Windows;
using MContracts.ViewModel;
using MCDomain.DataAccess;
using System.Windows.Threading;

namespace MContracts.Controls.Dialogs
{

    public static class ExtensionMethods
    {

        private static readonly Action EmptyDelegate = delegate() { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OraExpWindow : Window
    {

        private OraExpViewModel viewmodel = null;

        public OraExpWindow(IContractRepository repository)
        {
            InitializeComponent();
            viewmodel = new OraExpViewModel(repository, null);
            viewmodel.OnRefresh += FormRefresh;
            DataContext = viewmodel;
        }

        
        public void FormRefresh(object sender, EventArgs eventargs)
        {
            txt.Refresh();
            lbl.Refresh();
        }
                
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.MakeQueries();           
        }
    }
}
