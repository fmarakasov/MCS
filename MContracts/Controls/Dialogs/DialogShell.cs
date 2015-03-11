using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MContracts.ViewModel;
using McUIBase.ViewModel;
using UIShared.Commands;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DialogShell.xaml
    /// </summary>
    [TemplatePart(Name = "PART_OKButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ApplyButton", Type = typeof(Button))]
    public partial class DialogShell : Window
    {
        static DialogShell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DialogShell),
                                                     new FrameworkPropertyMetadata(typeof (DialogShell)));
        }

        public RepositoryViewModel ViewModel
        {
            get { return DataContext as RepositoryViewModel; }
            set { DataContext = value; }
        }

        public static DependencyProperty OkCommandProperty = DependencyProperty.Register("OkCommand", typeof(ICommand), typeof(DialogShell), new FrameworkPropertyMetadata(null));
        public static DependencyProperty ApplyCommandProperty = DependencyProperty.Register("ApplyCommand", typeof(ICommand), typeof(DialogShell), new FrameworkPropertyMetadata(null));
        public static DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(DialogShell), new FrameworkPropertyMetadata(null));





        public ICommand OkCommand
        {
            get
            {

                return (ICommand)(base.GetValue(OkCommandProperty));
            }
            set
            {
                base.SetValue(OkCommandProperty, value);
            }
        }


        public ICommand ApplyCommand
        {
            get
            {

                return (ICommand)(base.GetValue(ApplyCommandProperty));
            }
            set
            {
                base.SetValue(ApplyCommandProperty, value);
            }
        }

        public  ICommand CancelCommand
        {
            get
            {
                return (ICommand) (base.GetValue(CancelCommandProperty));
            }
            set
            {
                base.SetValue(CancelCommandProperty, value);
            }
        }

        void SubmitChanges()
        {
            if (ViewModel == null) return;
            if (ViewModel.SaveCommand.CanExecute(this))
                ViewModel.SaveCommand.Execute(this);
        }

        public bool DoRejectChanges { get; set; }

        public void RejectChanges()
        {
            if (DoRejectChanges)
            {
                ViewModel.Repository.RejectChanges();
                DialogResult = false;
            }
        }

        public DialogShell()
        {


            DoRejectChanges = false;

            OkCommand = new RelayCommand((o) => {
                                                    SubmitChanges();
                                                    DialogResult = true;
                                                },
                                         (o) => ViewModel != null &&
                                                ViewModel.SaveCommand.CanExecute(o)); 

            ApplyCommand = new RelayCommand((o) => SubmitChanges(),
                                            (o) => ViewModel != null &&
                                                   ViewModel.SaveCommand.CanExecute(o));

            CancelCommand = new RelayCommand((o) =>
                                                {
                                                    RejectChanges();
                                                    DialogResult = false;
                                                },
                                            (o) => true); 

        }
    }
}
