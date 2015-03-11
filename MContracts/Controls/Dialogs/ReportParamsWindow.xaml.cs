using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using McReports.Common;
using McReports.ViewModel;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ReportParamsWindow.xaml
    /// </summary>
    public partial class ReportParamsWindow : IUiQueryParametersProvider
    {
        public ReportParamsWindow()
        {
            InitializeComponent();
        }
            
 

        private void BuildUI(BaseReportViewModel reportViewModel)
        {
            System.Windows.Data.Binding myBinding = new System.Windows.Data.Binding
            {
                StringFormat = "0:N"
            };



            foreach (var reportParameter in reportViewModel.Parameters)
            {
                var paramSp = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 10, 10, 5) };

                var paramLbl = new Label
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = reportParameter.Attribute.DisplayName,
                    Margin = new Thickness(0, 0, 10, 0)
                };


                paramSp.Children.Add(paramLbl);


                if (reportParameter.Attribute.DesiredType.BaseType == typeof(Enum))
                {
                    MakeEnumInput(reportViewModel, reportParameter, paramSp);
                }

                else if (reportParameter.Attribute.LookUpType != null)
                {
                    var provider =
                        Activator.CreateInstance(reportParameter.Attribute.LookUpType) as IObjectDataProvider;
                    Contract.Assert(provider != null, "Тип reportParameter.Attribute.LookUpType должен поддерживать IObjectDataProvider");
                    MakeObjectDataProviderInput(reportViewModel, reportParameter, paramSp, provider.GetDataProvider());

                }
                else
                
                    MakeTextBoxInput(reportViewModel, reportParameter, paramSp);
                

                SP.Children.Add(paramSp);
            }
        }

        private static void MakeTextBoxInput(BaseReportViewModel reportViewModel, ReportParameter reportParameter,
                                             StackPanel paramSp)
        {
            var paramTb = new TextBox {VerticalAlignment = VerticalAlignment.Top, MinWidth = 150};

            paramTb.Name = reportParameter.PropertyName;
            Binding binding = new Binding();
            binding.Source = reportViewModel;
            binding.Path = new PropertyPath(reportParameter.PropertyName);
            binding.Mode = BindingMode.TwoWay;
            paramTb.SetBinding(TextBox.TextProperty, binding);


            paramSp.Children.Add(paramTb);
        }

        private static void MakeObjectDataProviderInput(BaseReportViewModel reportViewModel,
                                                        ReportParameter reportParameter,
                                                        StackPanel paramSp, ObjectDataProvider objectDataProvider)
        {
            Contract.Assert(objectDataProvider!=null);

            var paramCntr = new ComboBox
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    MinWidth = 150,
                    Name = reportParameter.PropertyName,
                    ItemsSource = (objectDataProvider.Data as IEnumerable)
                };
            
            var binding = new Binding
            {
                Source = reportViewModel,
                Path = new PropertyPath(reportParameter.PropertyName),
                Mode = BindingMode.TwoWay
            };

            paramCntr.SetBinding(Selector.SelectedItemProperty, binding);


            paramSp.Children.Add(paramCntr);
        }

        private static void MakeEnumInput(BaseReportViewModel reportViewModel, ReportParameter reportParameter,
                                          StackPanel paramSp)
        {
          
            var objectDataProvider = new ObjectDataProvider
                {
                    MethodName = "GetValues",
                    ObjectType = reportParameter.Attribute.DesiredType
                };

            objectDataProvider.MethodParameters.Add(reportParameter.Attribute.DesiredType);
            objectDataProvider.Refresh();
            MakeObjectDataProviderInput(reportViewModel, reportParameter, paramSp, objectDataProvider);
        }


        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;

            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public BaseReportViewModel ViewModel { get; set; }
        

        public bool GetParameters()
        {
            return ShowDialog().GetValueOrDefault();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            BuildUI(ViewModel);
        }
    }
}
