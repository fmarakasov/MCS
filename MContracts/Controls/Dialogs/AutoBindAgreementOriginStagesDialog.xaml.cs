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
using MContracts.ViewModel;
using MContracts.Classes.Converters;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AutoBindAgreementOriginStagesDialog : Window
    {
        private AutoBindAgreementOriginStagesDialogViewmodel _viewmodel;
        public AutoBindAgreementOriginStagesDialog()
        {
            InitializeComponent();
            _viewmodel = new AutoBindAgreementOriginStagesDialogViewmodel();
            DataContext = _viewmodel;

        }

        public bool UseStagenum 
        {
            get
                {
                    return _viewmodel.UseStagenum;
                }
        }

        public bool UseStagename
        {
            get
                {
                    return _viewmodel.UseStagename;
                }
        }

        public bool UseObjectcode
        {
            get
            {
                return _viewmodel.UseObjectcode;
            }
        }



        public bool ClearBinding
        {
            get { return _viewmodel.ClearBinding;  }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
