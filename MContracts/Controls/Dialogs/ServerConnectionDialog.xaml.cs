using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CommonBase;
using UIShared.Commands;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ServerConnectionDialog.xaml
    /// </summary>
    public partial class ServerConnectionDialog : INotifyPropertyChanged
    {
        
        public string ExceptionTooltip
        {
            get
            {
                if (_lastException != null) return _lastException.Message;
                return string.Empty;
            }
        }
        public ICommand TryConnectCommand
        {
            get { return new RelayCommand(p => Logon(), x => cbServerName.SelectedItem != null); }
        }

        #region Private Fields
 
        private int _attempts;
 
        #endregion
 
        #region Public Properties
 
        public int Attempts {
            get { return _attempts; }
            set {
                if (value != _attempts) {
                    _attempts = value;
                    OnPropertyChanged("Attempts");
                    OnPropertyChanged("ShowInvalidCredentials");
                }
            }
        }
 
        public Visibility ShowInvalidCredentials {
            get {
                if (_attempts > 0) {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }
 
        public string UserName {
            get { return txtUsername.Text; }
        }
 
        public string Password {
            get 
            { return txtPassword.Password; }
            set 
            { 
                txtPassword.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public string ServerName { get { return cbServerName.Text; } }
 
        #endregion
 
        public ServerConnectionDialog() : this(string.Empty,string.Empty) {}

        
        public ServerConnectionDialog(string userName, string password)
        {
            InitializeComponent();
            DataContext = this;
            txtUsername.Focus();
            txtUsername.Text = userName;
            txtPassword.Password = password;
        }
 
        #region INotifyPropertyChanged Members
 
        public event PropertyChangedEventHandler PropertyChanged {
            add { PropertyChangedEvent += value; }
            remove { PropertyChangedEvent -= value; }
        }
 
        #endregion

        public string OracleStatus
        {
            get
            {
                try
                {
                    var res = OracleDataSources;
                    return null;
                }
                catch (Exception e)
                {
                    return "Ошибка: " + e.Message;                    
                }
            }
        }

        private Exception _lastException = null;

        private void Logon()
        {                        
            if (_loginProvider.Connect(UserName, Password, ServerName, out _lastException)) 
            {
                DialogResult = true;
                Close();
            }
            else
            {
                Password = string.Empty;
                if (++Attempts > 3)
                {
                    DialogResult = false;
                    Close();
                }
                    
            }

            OnPropertyChanged("ExceptionTooltip");
        }
        

        private void CredentialsFocussed(object sender, RoutedEventArgs e) {
            var tb = sender as TextBoxBase;
            if (tb == null) {
                var pwb = sender as PasswordBox;
                Contract.Assert(pwb!=null);
                pwb.SelectAll();
            }
            else {
                tb.SelectAll();
            }
        }
 
        private event PropertyChangedEventHandler PropertyChangedEvent;
 
        protected void OnPropertyChanged(string prop) {
            if (PropertyChangedEvent != null)
                PropertyChangedEvent(this, new PropertyChangedEventArgs(prop));
        }
        
        public IEnumerable<MCDomain.DataAccess.OracleDataSource> OracleDataSources
        {
            get { return MCDomain.DataAccess.OracleConnectionProvider.Instance.OracleDataSources; }
        }

        private ILoginProvider _loginProvider;

        public bool QueryCredentails(ILoginProvider loginProvider)
        {
            //var dlg = new ServerConnectionDialog(UserName, string.Empty) {_loginProvider = loginProvider};
            _loginProvider = loginProvider;
            return ShowDialog().GetValueOrDefault();            
        }

        public Exception ConnectionException
        {
            get { return _lastException; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            //Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
