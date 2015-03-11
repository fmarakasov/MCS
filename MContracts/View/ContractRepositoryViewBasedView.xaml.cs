using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonBase;
using MCDomain.Model;
using MContracts.ViewModel;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UIShared.Common;

namespace MContracts.View
{
    /// <summary>
    ///     Логика взаимодействия для ContractRepositoryViewBasedView.xaml
    /// </summary>
    public partial class ContractRepositoryViewBasedView : UserControl
    {


        public ContractRepositoryViewBasedView()
        {
            InitializeComponent();

        }

        private ContractRepositoryViewBasedViewModel ViewModel
        {
            get
            {
                var vm = DataContext.CastTo<ContractRepositoryViewBasedViewModel>();
                return vm;
            }
        }

        private void ContractRepositoryViewBasedViewRequestExportRegistry(object sender, EventArgs e)
        {
            const string extension = "xls";
            const ExportFormat format = ExportFormat.Html;

            //RadComboBoxItem comboItem = ComboBox1.SelectedItem as RadComboBoxItem;
            //string selectedItem = comboItem.Content.ToString();

            //switch (selectedItem)
            //{
            //    case "Excel": extension = "xls";
            //format = ExportFormat.Html;
            ////        break;
            //    case "ExcelML": extension = "xml";
            //        format = ExportFormat.ExcelML;
            //        break;
            //    case "Word": extension = "doc";
            //        format = ExportFormat.Html;
            //        break;
            //    case "Csv": extension = "csv";
            //        format = ExportFormat.Csv;
            //        break;
            //}                        

            var dialog = new SaveFileDialog
                {
                    DefaultExt = extension,
                    Filter =
                        String.Format("{1} файлы (*.{0})|*.{0}|Все файлы (*.*)|*.*", extension,
                                      "Microsoft Excel"),
                    FilterIndex = 1
                };

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    var exportOptions = new GridViewExportOptions
                        {
                            Format = format,
                            ShowColumnFooters = true,
                            ShowColumnHeaders = true,
                            ShowGroupFooters = true
                        };

                    contractsDataGrid.Export(stream, exportOptions);
                    AppMessageBox.Show(
                        string.Format("Реестр договоров успешно экспортирован в файл {0}", dialog.FileName),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ContractsDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var cvm = DataContext.CastTo<ContractRepositoryViewBasedViewModel>();
            if (cvm.MainViewModel.OpenContractCommand.CanExecute(sender))
                DataContext.CastTo<ContractRepositoryViewBasedViewModel>()
                           .MainViewModel.OpenContractCommand.Execute(sender);
        }

        private void RepositoryDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            e.DependencyPropertyAction<ContractRepositoryViewBasedViewModel>((oldObj, newObj) =>
                {
                    oldObj.Do(x => x.RequestExportRegistry -= ContractRepositoryViewBasedViewRequestExportRegistry);
                    newObj.Do(x => x.RequestExportRegistry += ContractRepositoryViewBasedViewRequestExportRegistry);
                    newObj.Do(x => x.FetchContextFunc = () => contractsDataGrid.Items);
                });


        }


        private void ContractRepositoryViewBasedViewOnInitialized(object sender, EventArgs e)
        {
            contractsDataGrid.Settings.SelectSpecifiedItems += Settings_SelectSpecifiedItems;
            contractsDataGrid.Settings.SelectionIdsRequire += Settings_SelectionIdsRequire;
        }

        private void Settings_SelectionIdsRequire(object sender, EventParameterArgs<ObjectIdentifierSelector> e)
        {
            e.Parameter.Id = e.Parameter.Item.Return(x => x.CastTo<Contractrepositoryview>().Id, default(long?));
        }

        private void Settings_SelectSpecifiedItems(object sender, EventParameterArgs<ObjectSelector> e)
        {
            e.Parameter.Item = ViewModel.Contractrepositoryviews.SingleOrDefault(x => x.Id == e.Parameter.Id);
        }
    }
}