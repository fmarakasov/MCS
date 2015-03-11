using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonBase;
using MCDomain.Common;
using MCDomain.Model;


namespace MContracts.Controls
{
    /// <summary>
    /// Логика взаимодействия для ResponsiblesListControl
    /// </summary>
    public partial class ResponsiblesListControl : UserControl
    {
        public ResponsiblesListControl()
        {
            InitializeComponent();
        }

        private IUnderResponsibility _responsibilityobject;
        public IUnderResponsibility ResponsibilityObject
        {
            get { return _responsibilityobject; }
            set
            {
                if (_responsibilityobject == value) return;
                _responsibilityobject = value;
                DataContext = _responsibilityobject;
            }
        }

        public static readonly DependencyProperty EmployeeProperty = DependencyProperty.Register("Employees", typeof(IList<Employee>), typeof(UserControl), 
                                                                                                  new FrameworkPropertyMetadata(null));

        private IList<Employee> _employees; 
        public IList<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; }
        }

        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register("Roles", typeof(IList<Role>), typeof(UserControl),
                                                                                                  new FrameworkPropertyMetadata(null));
        private IList<Role> _roles; 
        public IList<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

    }
}
