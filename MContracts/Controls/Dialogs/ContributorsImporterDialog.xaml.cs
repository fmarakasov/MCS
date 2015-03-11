using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MContracts.ViewModel;
using MContracts.Controls;
using MCDomain.Importer;
using MCDomain.Model;
using MCDomain.DataAccess;

namespace MContracts.Dialogs
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ContributorsImporterDialogWindow : Window, IDisposable
    {

        ContributorsImporterViewModel _dataContext;
        
        public ContributorsImporterDialogWindow(IContractRepository _repository)
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
            
            _dataContext = new ContributorsImporterViewModel(_repository);
            
            
            
            DataContext = _dataContext;
            
            EventHandler handler = null;
            handler = delegate
            {
                this.Close();
            };



            _dataContext.Closable.RequestClose += handler;
            _dataContext.AfterExcelReloading += OnResetGridDimensions;
            _dataContext.BeforeExcelReloading += OnBeforeStagesReloading;
            _dataContext.AfterSchemeApplying += OnAfterSchemeApplying;
            _dataContext.SavingSetting = ImporterSavingSetting.SaveAll;
            _dataContext.StateChanged += OnStateChanged;
            rbObjectCode.IsEnabled = false;
            
            _dataContext.DoShowSettings += OnDoShowHideSettings;
            OnDoShowHideSettings(this, null);
            _dataContext.DoShowDataSourceSettings += OnDoShowHideDataSourceSettings;
            
            CellsGridPreview.SetDimensions(10, 10, 1, 1);

            AllowClosing = false;

        }

        private void OnDoShowHideSettings(object sender, EventArgs eventargs)
        {
            if (_dataContext.IsSettingsVisible)
            {
                gbxItemSettings.Visibility = System.Windows.Visibility.Visible;
                gbxItemSettings.Width = 300;
            }
            else
            {
                gbxItemSettings.Visibility = System.Windows.Visibility.Hidden;
                gbxItemSettings.Width = 0;
            }

        }


        public ContributorsImporterDialogWindow(ImporterViewModel viewmodel): base()
        {
            DataContext = viewmodel;
        }




        private void OnBeforeStagesReloading(object sender, EventArgs eventargs)
        {
            StageDataGrid.ItemsSource = null;

        }


        private void OnAfterSchemeApplying(object sender, EventArgs eventargs)
        {
            StageDataGrid.ItemsSource = _dataContext.Importer.PlainStageList;

        }

        private void OnStateChanged(object sender, ImporterViewModelStateChangedEventArgs args)
        { 
           if (_dataContext.State == ImporterState.DataSavedState) 
           {
               tshResults.IsSelected = true;
             

           }
              }


        public void OnSelectScheme(object sender, SelectionChangedEventArgs args)
        {

            _dataContext.Importer.Scheme.Items.SetDefaultCols();
            int i;
            ImportingSchemeItem item;

            for (i = 0; i < CellsGridPreview.HeaderControls.Count; i++)
            {
                item = _dataContext.Importer.Scheme.Items.GetItemByColumn(i);
                if (item != null)
                {
                    (CellsGridPreview.HeaderControls[i] as ImportingSchemeColumnDefiner).SelectedItem = item;
                }
            }
            
            // те, которые оказываются за пределами обозначенного диапазона - выкидываем
            for (i = CellsGridPreview.HeaderControls.Count; i <= _dataContext.Importer.Scheme.Items.MaxCol(); i++)
            {
                item = _dataContext.Importer.Scheme.Items.GetItemByColumn(i);
                if (item != null) item.Col = -1;
            }

            _dataContext.Importer.SendPropertyChanged("StartColumn");
            _dataContext.Importer.SendPropertyChanged("FinishColumn");
        }

        public void OnSelectItem(object sender, SelectionChangedEventArgs args)
        {
            ImportingSchemeColumnDefiner def = ((sender as ComboBox).Parent as Grid).Parent as ImportingSchemeColumnDefiner;
            if (def.SelectedItem != null)
            {
                ImportingSchemeItem item = _dataContext.Importer.Scheme.Items.GetItemByCode(def.SelectedItem.Code);
                if (item != null)
                {

                    ImportingSchemeItem selitem;
                    // если кто-то был на этом месте - говорим что он теперь нигде
                    selitem = _dataContext.Importer.Scheme.Items.GetItemByColumn(def.ColumnIndex - (CellsGridPreview.NumberRows ? 1 : 0));
                    if (selitem != null) selitem.Col = -1;

                    item.Col = def.ColumnIndex - (CellsGridPreview.NumberRows ? 1 : 0);
                    item.DefaultCol = item.Col;


                    for (int i = 0; i < CellsGridPreview.HeaderControls.Count; i++)
                    {
                        if (i != item.Col)
                        {
                            selitem = (CellsGridPreview.HeaderControls[i] as ImportingSchemeColumnDefiner).SelectedItem;
                            if ((selitem != null) && (selitem.Code == item.Code))
                            {
                                (CellsGridPreview.HeaderControls[i] as ImportingSchemeColumnDefiner).SelectedItem = null;
                            }
                        }
                    }


                    if  (_dataContext.Importer.Scheme.Items.IsAllRequiredFieldsFilled())
                    {
                        if (_dataContext.State < ImporterState.FieldsBoundState)
                        {
                            _dataContext.State = ImporterState.FieldsBoundState;
                        }
                    }
                    else
                    {
                            _dataContext.State = ImporterState.FileOpenedState;
                    }
                }
            }
            else
            {
                ImportingSchemeItem item = _dataContext.Importer.Scheme.Items.GetItemByColumn(def.ColumnIndex - (CellsGridPreview.NumberRows ? 1 : 0));
                if (item != null)
                {
                    item.Col = -1;
                }
            }
            // привязка по коду объекта и имени возможна только если этот код выбран в качестве одного из столбцов привязки
            rbObjectCode.IsEnabled = (_dataContext.Importer.Scheme.Items.GetItemByCode("objectcode").Col > -1);
            rbStageNum.IsEnabled = (_dataContext.Importer.Scheme.Items.GetItemByCode("num").Col > -1);
              rbStageName.IsEnabled = (_dataContext.Importer.Scheme.Items.GetItemByCode("subject").Col > -1);

        }

        private void OnInitSteps(object sender, EventArgs eventargs)
        {
            prg.Minimum = 0;
            prg.Maximum = (eventargs as ProgressEventArgs).Maximum;
            prg.Value = 0;
            prg.LargeChange = 1;
            updatePbDelegate = new UpdateProgressBarDelegate(prg.SetValue);
        }
        
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        UpdateProgressBarDelegate updatePbDelegate;

        private void OnMakeStep(object sender, EventArgs eventargs)
        {
           Dispatcher.Invoke(updatePbDelegate,  
                             System.Windows.Threading.DispatcherPriority.Background,
                             new object[] { ProgressBar.ValueProperty, prg.Value + 1 });
            
           
        }

        private void OnResetGridDimensions(object sender, GridSettingsChangedEventArgs eventargs)
        {
            cmbxImportingScheme.SelectionChanged += OnSelectScheme;
            CellsGridPreview.SetDimensions(eventargs.RowCount, eventargs.ColCount, _dataContext.Importer.StartColumn.HasValue?_dataContext.Importer.StartColumn.Value : 1, _dataContext.Importer.StartRow.HasValue? _dataContext.Importer.StartRow.Value : 1);

            int i; int j;

            for (i = 1; i <= eventargs.ColCount; i++)
            {
                ImportingSchemeColumnDefiner def = new ImportingSchemeColumnDefiner(i);
                def.cmbxColumn.SelectionChanged += OnSelectItem; 
                CellsGridPreview.SetHeaderControl(i, def, eventargs.Cells[0][i].ColumnWidth);
            }

            for (i = 0; i < eventargs.RowCount; i++)
            {
                for (j = 0; j < eventargs.ColCount; j++)
                {
                    CellsGridPreview.SetControl(i, j, new SingleCellControl(eventargs.Cells[i][j]), eventargs.Cells[i][j].ColumnWidth * 10, eventargs.Cells[i][j].RowHeight + 15);
                }
            }

            //StageDataGrid.ItemsSource = _dataContext.Importer.PlainStageList;

            SourceDataGrid.Children.Remove(cmbxImportingScheme);
            CellsGridPreview.CrossControl = cmbxImportingScheme;


            cmbxImportingScheme.Visibility = System.Windows.Visibility.Visible;


        }

        public void Dispose()
        {
            Close();
        }

        public bool AllowClosing { get; set; }
        private void ImportDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!AllowClosing)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
                e.Cancel = true;
            }
            else
            {
                _dataContext.Importer.ClearReaders();
            }
        }

        private void ImportDlg_Closed(object sender, EventArgs e)
        {

        }

        private void ImportDlg_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void rbSaveAll_Checked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null) _dataContext.SavingSetting = ImporterSavingSetting.SaveAll;
        }

        private void rbSaveNew_Checked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null) _dataContext.SavingSetting = ImporterSavingSetting.SaveNew;
        }



        public Schedulecontract CurrentSchedule
        {
            get
            {
                return _dataContext.CurrentSchedule;
            }

            set
            {
                tbView.SelectedIndex = 0;
                _dataContext.CurrentSchedule = value;
                if (value != null) 
                {
                    if (value.Schedule.Currencymeasure != null)
                    {
                        if (value.Schedule.Currencymeasure.Factor == 1)
                            PriceColumn.Header = "Сумма, руб.";
                        else
                        {
                            PriceColumn.Header = "Сумма, " + value.Schedule.Currencymeasure.Name + " руб.";
                        }
                    }
                    else
                    {
                        PriceColumn.Header = "Сумма, руб.";
                    }
  
                    (_dataContext.Importer as ContributorImporter).InitSteps += OnInitSteps;
                    (_dataContext.Importer as ContributorImporter).NextStep += OnMakeStep;
                }
                else
                {
                    PriceColumn.Header = "Сумма, руб.";
                }
            }
        }

        private void btnShowOptions_Checked(object sender, RoutedEventArgs e)
        {
            
        }




        private void btnWDReapplyScheme_Click(object sender, RoutedEventArgs e)
        {
        }

        private void chbxUseDefaultStartDate_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxUseDefaultStartDate.IsChecked.HasValue && !chbxUseDefaultStartDate.IsChecked.Value) dtmDefaultStartDate.SelectedDate = null;
        }

        private void chbxUseDefaultFinDate_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxUseDefaultFinDate.IsChecked.HasValue && !chbxUseDefaultFinDate.IsChecked.Value) dtmDefaultFinDate.SelectedDate = null;
        }

        private void rbStageNum_Checked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null) _dataContext.StageBindingSetting = ImporterStageBindingSetting.BindByStageNum;
        }

        private void rbObjectCode_Checked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null) _dataContext.StageBindingSetting = ImporterStageBindingSetting.BindByObjectCode;
        }

        private void rbStageName_Checked(object sender, RoutedEventArgs e)
        {
            if (_dataContext != null) _dataContext.StageBindingSetting = ImporterStageBindingSetting.BindBySubject;
        }

        private void label2_Checked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNdsalgorithm");
        }

        private void label3_Checked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNds");
        }

        private void label4_Checked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNtpsubview");
        }

        private void label2_Unchecked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNdsalgorithm");
        }

        private void label3_Unchecked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNds");
        }

        private void label4_Unchecked(object sender, RoutedEventArgs e)
        {
            _dataContext.Importer.SendPropertyChanged("UseDefaultNtpsubview");
        }


        //private void 
        private void OnDoShowHideDataSourceSettings(object sender, EventArgs eventargs)
        {
            if (_dataContext.IsDataSourceSettingsVisible)
            {
                if (_dataContext.Importer.IsExcel)
                {
                    gbxExcelSettings.Visibility = Visibility.Visible;
                    gbxExcelSettings.Height = 160;
                }
                else if (_dataContext.Importer.IsWord)
                {
                    gbxWordSettings.Visibility = Visibility.Visible;
                    gbxWordSettings.Height = 160;
                }
            }
            else
            {
                gbxExcelSettings.Visibility = System.Windows.Visibility.Hidden;
                gbxWordSettings.Visibility = Visibility.Hidden;

                gbxExcelSettings.Height = 0;
                gbxWordSettings.Height = 0;
            }

        }

    
    }
}
