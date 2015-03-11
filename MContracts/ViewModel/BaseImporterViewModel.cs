using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MContracts.Classes;
using UIShared.Commands;
using System.Windows.Input;
using MCDomain.Importer;
using Microsoft.Win32;
using CommonBase;
using System.Windows.Data;
using System.Globalization;
using MCDomain.DataAccess;
using System.Collections.ObjectModel;
using MCDomain.Model;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using MContracts.Controls.Dialogs;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{

    #region HelperClasses

    /// <summary>
    /// состояние диалога импорта
    /// </summary>
    public enum ImporterState
    {
        /// <summary>
        /// начальное, пустое состояние
        /// </summary>
        NullState,
        /// <summary>
        /// файл открыли и загрузили, ждем привязки
        /// </summary>
        FileOpenedState,
        /// <summary>
        /// все обязательные поля привязаны
        /// </summary>
        FieldsBoundState,
        /// <summary>
        /// нажата кнопка "Сохранить"
        /// </summary>
        DataSavedState

    }

    #endregion

    public class BaseImporterViewModel : WorkspaceViewModel
    {
        #region Constructor
        public BaseImporterViewModel(IContractRepository repository) : base(repository) 
        {
            SavingSetting = ImporterSavingSetting.SaveAll;
            NeedSave = false;
            _state = ImporterState.NullState;
            _isdatasourcesettingsvisible = true;

        }
        #endregion

        #region Properties 

        public ImportedStage SelectedStage
        {
            get;
            set;
        }

        public IEnumerable<ImportedStage> Stages
        {
            get 
            { 
                return Importer.Stages; 
            }
        }

        /// <summary>
        /// следует ли сохнанять? замена Model Result
        /// </summary>
        public bool NeedSave { get; set; }

        private ObservableCollection<Ndsalgorithm> _ndsalgorithms;
        /// <summary>
        /// Получает доступ к коллекции алгоритмов НДС
        /// </summary>
        public ObservableCollection<Ndsalgorithm> Ndsalgorithms
        {
            get
            {
                if (_ndsalgorithms == null)
                    _ndsalgorithms = new ObservableCollection<Ndsalgorithm>(Repository.Ndsalgorithms);
                return _ndsalgorithms;
            }
        }

        /// <summary>
        /// опция сохранения
        /// </summary>
        public ImporterSavingSetting SavingSetting { get; set; }

        /// <summary>
        /// опция привязки этапов
        /// </summary>
        public ImporterStageBindingSetting StageBindingSetting
        {
            get { return Importer.StageBindingSetting; }

            set
            {
                Importer.StageBindingSetting = value;
            }
        }


        /// <summary>
        /// текущий договор
        /// </summary>
        public Contractdoc CurrentConrtactdoc
        {
            get { return CurrentSchedule.Contractdoc; }
        }

        private Schedulecontract _currentschedule;
        /// <summary>
        /// текущий календарный план
        /// </summary>
        public Schedulecontract CurrentSchedule
        {
            get
            {
                return _currentschedule;
            }

            set
            {
                NeedSave = false;
                //if (_currentschedule == value) return;
                _currentschedule = value;
                _inputstagebindinglist =  new BindingList<Stage>(_currentschedule.Schedule.Stages.OrderBy(
                    s => s.Num, new NaturSortComparer<Stage>()).ToList());

                if (_imp == null)
                {
                    _imp = CreateImporter();
                    State = ImporterState.NullState;
                }
                else
                {
                    _imp.Schedule = value.Schedule;
                    if (_imp.ActiveReader != null)
                        State = ImporterState.FieldsBoundState;
                    else
                        State = ImporterState.NullState;
                }



            }
        }

        private IBindingList _inputstagebindinglist;
        /// <summary>
        /// входной напор этапов календарного плана
        /// </summary>
        public IBindingList InputStageBindingList
        {
            get { return _inputstagebindinglist; }
        }

        private ObservableCollection<Stage> _outputstagebindinglist;
        /// <summary>
        /// выходной набор этапов КП
        /// </summary>
        public ObservableCollection<Stage> OutputStageBindingList
        {
            get
            {
                if (_outputstagebindinglist == null) _outputstagebindinglist = new ObservableCollection<Stage>();
                return _outputstagebindinglist;
            }
        }

        OpenFileDialog _od;
        /// <summary>
        /// диалог для открытия файла
        /// </summary>
        public OpenFileDialog OpenDialog
        {
            get
            {
                if (_od == null)
                {
                    _od = new OpenFileDialog();
                    _od.Filter = "Файлы Microsoft Excel (*.xls, *.xlsx)|*.xls*|Файлы Microsoft Word (*.doc, *.docx)|*.doc*";
                    _od.AddExtension = true;
                    _od.CheckFileExists = true;
                }
                return _od;
            }
        }

        protected BaseImporter _imp;
        /// <summary>
        /// объект, осуществляющий импорт
        /// </summary>
        public BaseImporter Importer
        {
            get
            {
                return _imp;
            }
        }


        private ImporterState _state;
        public ImporterState State
        {
            get
            {
                return _state;
            }

            set
            {
                //if (_state == value) return;

                ImporterState oldstate = _state;
                _state = value;
                if (StateChanged != null) StateChanged(this, new ImporterViewModelStateChangedEventArgs(oldstate));
            }
        }

        #endregion

        #region Commands

        public ICommand DeleteStageCommand { get; set; }

        private ICommand _saveschemecommand;
        public ICommand SaveSchemeCommand
        {
            get
            {
                if (_saveschemecommand == null) _saveschemecommand = new RelayCommand(p => SaveScheme(), x => CanSaveScheme());
                return _saveschemecommand;
            }
        }

        private ICommand _openfilecommand;
        /// <summary>
        /// открыть файл
        /// </summary>
        public ICommand OpenFileCommand
        {
            get
            {
                if (_openfilecommand == null) _openfilecommand = new RelayCommand(p => OpenFile(), x => CanOpenFile);
                return _openfilecommand;
            }
        }


        private ICommand _reopenfilecommand;
        /// <summary>
        /// перечитать файл
        /// </summary>
        public ICommand ReopenFileCommand
        {
            get
            {
                if (_reopenfilecommand == null) _reopenfilecommand = new RelayCommand(p => ReopenFile(), x => CanReopenFile);
                return _reopenfilecommand;
            }
        }

        private ICommand _reapplyschemecommand;
        /// <summary>
        /// переприменить схему импорта
        /// </summary>
        public ICommand ReapplySchemeCommand
        {
            get
            {
                if (_reapplyschemecommand == null) _reapplyschemecommand = new RelayCommand(p => ReapplyScheme(), x => CanReapplyScheme);
                return _reapplyschemecommand;
            }
        }

        private ICommand _showappcommand;
        /// <summary>
        /// показать открытый в "родном" приложении документ
        /// </summary>
        public ICommand ShowAppCommand
        {
            get
            {
                if (_showappcommand == null) _showappcommand = new RelayCommand(p => ShowApp(), x => CanShowApp);
                return _showappcommand;
            }
        }

        private ICommand _saveimportresultscommand;
        /// <summary>
        /// сохранить результаты импорта
        /// </summary>
        public ICommand SaveImportResultsCommand
        {
            get
            {
                if (_saveimportresultscommand == null) _saveimportresultscommand = new RelayCommand(p => SaveResults(), x => CanSaveResults);
                return _saveimportresultscommand;
            }
        }

        private ICommand _showhidesettingscommand;
        public ICommand ShowHideSettingsCommand
        {
            get
            {
                if (_showhidesettingscommand == null) _showhidesettingscommand = new RelayCommand(p => ShowHideSettings(), x => CanShowHideSettings);
                return _showhidesettingscommand;
            }
        }

        private ICommand _showhidedatasourcesettingscommand;
        public ICommand ShowHideDataSourceSettingsCommand
        {
            get
            {
                if (_showhidedatasourcesettingscommand == null) _showhidedatasourcesettingscommand = new RelayCommand(p => ShowHideDataSourceSettings(), x => CanShowHideDataSourceSettings);
                return _showhidedatasourcesettingscommand;                
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// открыть и отобразить в таблице просмотра импортируемый файл
        /// </summary>
        private void OpenFile()
        {
            bool? showdialogresult;
            showdialogresult = OpenDialog.ShowDialog();

            if (showdialogresult.HasValue ? showdialogresult.Value : false)
            {
                DataContextDebug.DebugPrintRepository(Repository);
                Importer.Filename = OpenDialog.FileName;
                Importer.Scheme.Items.RestoreDefaultCols();
                Importer.Active = true;

                if (AfterExcelReloading != null)
                {
                    AfterExcelReloading(this, new GridSettingsChangedEventArgs(Importer.ColCount, Importer.RowCount, Importer.Cells));
                }
                DataContextDebug.DebugPrintRepository(Repository);
            }
            State = ImporterState.FileOpenedState;
        }

        /// <summary>
        /// можно открыть файл
        /// </summary>
        public bool CanOpenFile
        {
            get { return true; }
        }

        /// <summary>
        /// перечитать тот же самый файл
        /// </summary>
        private void ReopenFile()
        {
            DataContextDebug.DebugPrintRepository(Repository);
            State = ImporterState.FileOpenedState;
            if (BeforeExcelReloading != null)
            {
                BeforeExcelReloading(this, new EventArgs());
            }

            Importer.Scheme.Items.RestoreDefaultCols();
            Importer.Refresh();

            if (AfterExcelReloading != null)
            {
                AfterExcelReloading(this, new GridSettingsChangedEventArgs(Importer.ColCount, Importer.RowCount, Importer.Cells));
            }
            DataContextDebug.DebugPrintRepository(Repository);
        }

        /// <summary>
        /// можно переоткрыть файл
        /// </summary>
        public bool CanReopenFile
        {
            get { return State >= ImporterState.FileOpenedState; }
        }

        /// <summary>
        /// переприменить схему
        /// </summary>
        private void ReapplyScheme()
        {
            DataContextDebug.DebugPrintRepository(Repository);
            if (BeforeExcelReloading != null)
            {
                BeforeExcelReloading(this, new EventArgs());
            }

            Importer.ReapplyScheme();

            if (AfterSchemeApplying != null)
            {
                AfterSchemeApplying(this, new EventArgs());
            }
            State = ImporterState.DataSavedState;

            DataContextDebug.DebugPrintRepository(Repository);

            ;

        }

        public bool CanReapplyScheme
        {
            get { return State >= ImporterState.FieldsBoundState; }
        }

        /// <summary>
        /// открыть файл в "родном" приложении
        /// </summary>
        private void ShowApp()
        {
            Importer.ActiveReader.ShowApp();
        }

        public bool CanShowApp
        {
            get { return State >= ImporterState.FileOpenedState; }
        }


        #region Members of RepositoryViewModel
        /// <summary>
        /// Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {

            throw new NotImplementedException();
        }
        #endregion

        #region Members Of WorkspaceViewModel
        public override bool IsClosable
        {
            get { return true; }
        }
        #endregion

        /// <summary>
        /// можно сохранять результаты
        /// </summary>
        public bool CanSaveResults
        {
            get { return State >= ImporterState.DataSavedState; }
        }


        protected virtual void InternalSaveResults() { }

        private void SaveResults()
        {
            InternalSaveResults();
        }


        public event EventHandler<EventArgs> DoShowSettings;
        public event EventHandler<EventArgs> DoShowDataSourceSettings;
        

        private bool _issettingsvisible = false;
        public bool IsSettingsVisible
        {
            get
            {
                return _issettingsvisible;
            }
            set
            {
                _issettingsvisible = value;

                OnPropertyChanged(() => IsSettingsVisible);
                if (DoShowSettings != null) DoShowSettings(this, null);
            }
        }

        private bool _isdatasourcesettingsvisible = false;
        public bool IsDataSourceSettingsVisible
        {
            get
            {
                return _isdatasourcesettingsvisible;
            }
            set
            {
                _isdatasourcesettingsvisible = value;

                OnPropertyChanged(() => IsDataSourceSettingsVisible);
                if (DoShowDataSourceSettings != null) DoShowDataSourceSettings(this, null);
            }
        }
        
        public bool CanShowHideSettings
        {
            get { return State >= ImporterState.FieldsBoundState; }
        }

        private void ShowHideSettings()
        {
            //IsSettingsVisible = !IsSettingsVisible;
        }

        private void ShowHideDataSourceSettings()
        {
            //IsSettingsVisible = !IsSettingsVisible;
        }

        public bool CanShowHideDataSourceSettings
        {
            get { return State >= ImporterState.FileOpenedState; }
        }



        protected virtual void FillStagesList(IList<Stage> outputstageslist, ImportedStage s, Stage parentstage) { }

        private bool CanSaveScheme()
        {
            return (Importer.Scheme != null && State >= ImporterState.FieldsBoundState);
        }
        /// <summary>
        /// сохранить схему импорта
        /// </summary>
        private void SaveScheme()
        {
            // показываем  окно сохранения схемы, передав в него схему
            SaveSchemeDialog dlg = new SaveSchemeDialog();
            dlg.Scheme = Importer.Scheme;
            if (dlg.ShowDialog().Value)
            {
                // проверяем - если схема по умолчанию была передана - создаем новую
                if (Importer.Scheme is DefaultImportingScheme)
                {
                    ImportingScheme lastscheme = Importer.Scheme;

                    Importer.Scheme = new ImportingScheme() { SchemeName = dlg.NewSchemeName };
                    Importer.Schemes.Add(Importer.Scheme);

                    // сохраняем текущие мэппинги столбцов
                    foreach (ImportingSchemeItem si in lastscheme.Items)
                    {
                        Importer.Scheme.Items.AddNewItem(0, si.Name, si.Code, si.Col, si.IsRequired);
                    }

                }
                else // в противном случае переименовываем старую
                    Importer.Scheme.SchemeName = dlg.NewSchemeName;

                // отражаем на схему в БД
                Importingscheme dbscheme = Repository.Importingschemes.FirstOrDefault(p => p.Id == Importer.Scheme.DbId);
                if (dbscheme == null)
                {
                    dbscheme = new Importingscheme();
                    Repository.Importingschemes.Add(dbscheme);
                    Repository.TryGetContext().Importingschemes.InsertOnSubmit(dbscheme);
                };
                dbscheme.Schemename = Importer.Scheme.SchemeName;


                Importingschemeitem dbsi;
                foreach (ImportingSchemeItem si in Importer.Scheme.Items)
                {
                    dbsi = dbscheme.Importingschemeitems.FirstOrDefault(p => (p.Id == si.DbId && si.DbId != 0));
                    if (dbsi == null) dbsi = new Importingschemeitem() { Importingscheme = dbscheme };

                    dbsi.Isrequired = si.IsRequired;
                    dbsi.Columnindex = si.Col;
                    dbsi.Columnname = si.Name;
                    dbsi.Columnsign = si.Code;
                }

                // начальный и конечный столбцы сохраняются также как элементы схемы
                dbsi = dbscheme.Importingschemeitems.FirstOrDefault(p => (p.Columnsign == "startcol"));
                if (dbsi == null) dbsi = new Importingschemeitem() { Importingscheme = dbscheme };
                dbsi.Isrequired = false;
                dbsi.Columnindex = Importer.Scheme.StartCol;
                dbsi.Columnsign = "startcol";
                dbsi.Columnname = "Начальный столбец";

                dbsi = dbscheme.Importingschemeitems.FirstOrDefault(p => (p.Columnsign == "finishcol"));
                if (dbsi == null) dbsi = new Importingschemeitem() { Importingscheme = dbscheme };
                dbsi.Isrequired = false;
                dbsi.Columnindex = Importer.Scheme.FinishCol;
                dbsi.Columnsign = "finishcol";
                dbsi.Columnname = "Конечный столбец";


                // сохраняемся 

                DataContextDebug.DebugPrintRepository(Repository);
                Repository.SubmitChanges();


                // обновляем список
                Importer.SendPropertyChanged("Schemes");
                // обновляем текущий элемент
                Importer.SendPropertyChanged("Scheme");


            }
        }

        protected virtual BaseImporter CreateImporter()
        {
            return null;
        }

        #endregion

        #region Events

        /// <summary>
        /// событие после перезагрузки файла в окне импорта
        /// </summary>
        public event EventHandler<GridSettingsChangedEventArgs> AfterExcelReloading;
        /// <summary>
        /// событие до перезагрузки файла в окне импорта
        /// </summary>
        public event EventHandler<EventArgs> BeforeExcelReloading;

        /// <summary>
        /// событие после смены схемы
        /// </summary>
        public event EventHandler<EventArgs> AfterSchemeApplying;


        public event EventHandler<ImporterViewModelStateChangedEventArgs> StateChanged;
        
        #endregion
    }
}
