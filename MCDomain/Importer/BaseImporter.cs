using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using System.Diagnostics.Contracts;
using System.IO;
using System.Collections.ObjectModel;
using CommonBase;

namespace MCDomain.Importer
{

    #region Exceptions
    class NoReaderForExtension : Exception
    {
        public NoReaderForExtension(string message) : base(message) { }
    }


    class SchemeColumnIndexOutOfBounds : Exception
    {
        public SchemeColumnIndexOutOfBounds(string message) : base(message) { }
    }

    class StageNotExist : Exception
    {
        public StageNotExist(string message) : base(message) { }
    }
    #endregion

    #region HelperClasses
    public class ProgressEventArgs : EventArgs
    {
        public int Maximum;
    }

    public enum ImporterStageBindingSetting
    {
        BindByStageNum,
        BindByObjectCode,
        BindBySubject
    }
    #endregion

    #region DTO

    public class Contributor
    {
        public double Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    public class ImportedStageResult
    {

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private ImportedStage _Stage;
        public ImportedStage Stage
        {
            get
            {
                return _Stage;
            }

            set { _Stage = value; }
        }

        private double _Ntpsubviewid;
        public double Ntpsubviewid
        {
            get { return _Ntpsubviewid; }
            set { _Ntpsubviewid = value; }
        }

        private bool _useddefaultntpsubview = false;
        public bool UsedDefaultNTPSubView
        {
            get { return _useddefaultntpsubview; }
            set { _useddefaultntpsubview = value; }

        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ImportedStage : INotifyPropertyChanged
    {

        /// <summary>
        /// обработчик сообщения об изменениях свойств класса
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// метод, рассылающий сообщения об изменениях
        /// </summary>
        /// <param name="propertyName"></param>
        public void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Num;
        public string Num
        {
            get { return _Num; }
            set 
            {
                _Num = value; 
                SendPropertyChanged("Num");
            }
        }

        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set
            {
                _Subject = value;
                SendPropertyChanged("Subject");
            }
        }
        private System.DateTime _Startsat;
        public System.DateTime Startsat
        {
            get { return _Startsat; }
            set
            {
                _Startsat = value;
                SendPropertyChanged("Startsat");
            }
        }

        private System.DateTime _Endsat;
        public System.DateTime Endsat
        {
            get { return _Endsat; }
            set
            {
                _Endsat = value;
                SendPropertyChanged("Endsat");
            }
        }

        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                SendPropertyChanged("Price");
            }
        }

        private double _Ndsalgorithmid;
        public double Ndsalgorithmid
        {
            get { return _Ndsalgorithmid; }
            set
            {
                _Ndsalgorithmid = value;
                SendPropertyChanged("Ndsalgorithmid");
            }
        }

        private ImportedStage _Parent;
        public ImportedStage Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                SendPropertyChanged("Code");
            }
        }

        private double _Ndsid;
        public double Ndsid
        {
            get { return _Ndsid; }
            set
            {
                _Ndsid = value;
                SendPropertyChanged("Ndsid");
            }
        }

        private List<ImportedStage> _Stages;
        public List<ImportedStage> Stages
        {
            get
            {
                if (_Stages == null) _Stages = new List<ImportedStage>();
                return _Stages;
            }
        }


        private List<ImportedStageResult> _Stageresults;
        public List<ImportedStageResult> Stageresults
        {
            get
            {
                if (_Stageresults == null) _Stageresults = new List<ImportedStageResult>();
                return _Stageresults;
            }
        }

       
        public int Level
        {
            get { return Parent == null?0:Parent.Level + 1; }
        }

        private Stage _origincontractstage;
        /// <summary>
        /// это - для привязки этапа ДС к генеральному
        /// </summary>
        public Stage OriginContractStage
        {
            get { return _origincontractstage; }
            set
            {
                _origincontractstage = value;
                _mainstages = null;
                SendPropertyChanged("OriginContractStage");
                SendPropertyChanged("MainStages");
            }
        }


        private Stage _generalcontractstage;
        public Stage GeneralContractStage
        {
            get { return _generalcontractstage; }
            set
            {
                _generalcontractstage = value;
                _mainstages = null;
                SendPropertyChanged("GeneralContractStage");
                SendPropertyChanged("MainStages");
            }
        }

        private IList<Stage> _mainstages; 
        public IEnumerable<Stage> MainStages
        {
            get
            {
                if (_mainstages == null)
                {
                    _mainstages = new List<Stage>();
                    if (OriginContractStage != null) _mainstages.Add(OriginContractStage);
                    if (GeneralContractStage != null) _mainstages.Add(GeneralContractStage);
                }
                return _mainstages;
            }
        }

        private Contributor _contributor;
        public Contributor Contributor
        {
            get
            {
                return _contributor;
            }

            set
            {
                _contributor = value;
                SendPropertyChanged("Contributor");
            }
        }
    }

    #endregion 

    public class BaseImporter: INotifyPropertyChanged, IReaderCommonParameters, IDataErrorInfo
    {

        
        #region Events

        /// <summary>
        /// обработчик сообщения об изменениях свойств класса
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// метод, рассылающий сообщения об изменениях
        /// </summary>
        /// <param name="propertyName"></param>
        public void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        #endregion


        #region Properties

        protected ObservableCollection<ImportedStage> _stages;
        /// <summary>
        /// иерархическая коллекция этапов, в которую производится импорт
        /// </summary>
        public IEnumerable<ImportedStage> Stages
        {
            get
            {
                if (_stages == null) _stages = new ObservableCollection<ImportedStage>();
                return _stages.AsEnumerable<ImportedStage>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int StageCount
        {
            get { return _stages.Count; }
        }

        private IList<ImportedStage> _plainstagelist;
        /// <summary>
        /// "плоский" список этапов (в него разворачивается иерархический Stages) 
        /// </summary>
        public IEnumerable<ImportedStage> PlainStageList
        {
            get
            {
                if (_plainstagelist == null) _plainstagelist = new List<ImportedStage>();
                else _plainstagelist.Clear();

                foreach (ImportedStage s in Stages)
                {
                    GetSubStages(s, _plainstagelist);
                }

                return _plainstagelist;
            }
        }

        /// <summary>
        /// список листов или таблиц, найденных при чтении файла
        /// </summary>
        public IEnumerable<INamed> FileElements
        {
            get
            {
                if (ActiveReader != null)
                    return ActiveReader.FileElements;
                else
                    return null;
            }
        }

        private List<ImportingScheme> _schemes;
        /// <summary>
        /// все схемы, доступные для редакции в текущий момент
        /// пока только схема по умолчанию
        /// </summary>
        public List<ImportingScheme> Schemes
        {
            get
            {
                if (_schemes == null)
                {
                    _schemes = new List<ImportingScheme>();
                    LoadImportingSchemes(_schemes);
                    SendPropertyChanged("Schemes");
                }
                return _schemes;
            }

            set
            {
                if (_schemes == value) return;
                _schemes = value;
            }
        }

        /// <summary>
        /// набор ячеек, прочитанный из конвертируемого файла
        /// </summary>
        public Cells Cells
        {
            get
            {
                Contract.Requires(ActiveReader != null);
                return ActiveReader.Cells;
            }
        }



        /// <summary>
        /// количество строк в прочитанном наборе ячеек
        /// </summary>
        public int RowCount
        {
            get
            {
                Contract.Requires(ActiveReader != null);
                if (Cells != null)
                    return Cells.RowCount;
                return 0;
            }
        }


        /// <summary>
        /// количество столбцов в прочитанном наборе ячеек
        /// </summary>
        public int ColCount
        {
            get
            {
                Contract.Requires(ActiveReader != null);
                if (Cells != null)
                    return Cells.ColCount;
                return 0;
            }
        }

        /// <summary>
        /// активность компонента-импортера, по изменению свойства открываем - закрываем импортер
        /// </summary>
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                //Contract.Requires(_filename != @"", "Не задано имя файла"); fmarakasov: если состояние false и мы вызываем imp.Active = false, то требовать _fileName!=null нет смысла

                if (_active == value) return;
                _active = value;
                if (_active) Open(); else Close();
                SendPropertyChanged("Active");
                SendPropertyChanged("Cells");
                SendPropertyChanged("FileElements");
                SendPropertyChanged("Stages");
                SendPropertyChanged("PlainStageList");

            }
        }

        public string _filename;
        /// <summary>
        /// имя конвертируемого файла
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set
            {
                Contract.Requires(value != "");
                //Contract.Ensures(_filename == value); -- fmarakasov: контракт нельзя гарантировать в любом из случаев т.к. есть выброс исключения



                if (!File.Exists(value)) throw new FileNotFoundException("Файл не найден");

                if (_filename != value)
                {
                    _filename = value;
                    SendPropertyChanged("Filename");
                    string _ext = Path.GetExtension(_filename);
                    int i;
                    _reader = null;
                    for (i = 0; i < Readers.Count; i++)
                    {
                        if (Readers[i].AcceptExtension(_ext))
                        {
                            Active = false;
                            _reader = Readers[i];
                            _reader.FileName = _filename;
                            SendPropertyChanged("ActiveReader");

                            return;
                        }
                    }

                }

                throw new NoReaderForExtension("Не найден конвертор для данного типа файлов");
            }
        }

        // опция каким образом привязывать к генеральному договору
        public ImporterStageBindingSetting StageBindingSetting
        {
            get;
            set;
        }

        private BaseReader _reader;
        /// <summary>
        /// активный конвертор - определяется при задании имени файла
        /// </summary>
        public BaseReader ActiveReader
        {
            get { return _reader; }
        }

        public bool IsExcel
        {
            get { return (ActiveReader != null && ActiveReader is ExcelReader); }
        }

        public bool IsWord
        {
            get { return (ActiveReader != null && ActiveReader is WordReader); }
        }

        private List<BaseReader> _readers;
        /// <summary>
        /// список поддерживаемых конверторов
        /// </summary>
        public List<BaseReader> Readers
        {
            get
            {
                if (_readers == null)
                {
                    _readers = new List<BaseReader>();
                    _readers.Add(new ExcelReader());
                    _readers.Add(new WordReader());
                }
                return _readers;
            }
        }

        private ImportingScheme _scheme;
        /// <summary>
        /// активная (выбранная схема конвертации)
        /// </summary>
        public ImportingScheme Scheme
        {
            get { return _scheme; }
            set
            {
                if (_scheme == value) return;
                _scheme = value;
                SendPropertyChanged("Scheme");
            }
        }


        IContractRepository _repository;

        public IContractRepository Repository
        {
            get { return _repository; }
        }


        Schedule schedule;
        public Schedule Schedule
        {
            get
            {
                return schedule;
            }

            set { schedule = value; }
        }

        
        Contractdoc contractdoc;
        public Contractdoc Contractdoc
        {
            get
            {
                return contractdoc;
            }
        }


        #endregion


        #region Methods

        /// <summary>
        /// метод, разворачивающий иерархический список этапов - в плоский
        /// </summary>
        /// <param name="s">родительский этап</param>
        /// <param name="stagelist">наполняемый "плоский" список этапов</param>
        private void GetSubStages(ImportedStage s, IList<ImportedStage> stagelist)
        {
            stagelist.Add(s);
            foreach (ImportedStage ss in s.Stages)
            {
                GetSubStages(ss, stagelist);
            }
        }

        /// <summary>
        /// метод, позволяющий переприменить схему конвертации
        /// </summary>
        public void ReapplyScheme()
        {
            Contract.Requires(ActiveReader != null);
            Contract.Ensures(Active);
            ApplyScheme();
            SendPropertyChanged("Stages");
            SendPropertyChanged("PlainStageList");
        }

        /// <summary>
        /// метод, позволяющий перечитать открытый файл
        /// </summary>
        public void Refresh()
        {
            Contract.Requires(ActiveReader != null);
            Contract.Ensures(Active);
            ActiveReader.AcceptParameters(this);
            DataContextDebug.DebugPrintRepository(Repository);
            ActiveReader.Read();
            DataContextDebug.DebugPrintRepository(Repository);
            ApplyScheme();
            DataContextDebug.DebugPrintRepository(Repository);

            SendPropertyChanged("Cells");

            SendPropertyChanged("StartColumn");
            SendPropertyChanged("FinishColumn");
            SendPropertyChanged("StartRow");
            SendPropertyChanged("FinishRow");

            SendPropertyChanged("ColCount");
            SendPropertyChanged("RowCount");
            SendPropertyChanged("Stages");
            SendPropertyChanged("PlainStageList");
        }

        virtual protected void InitPrgBar() { }

        virtual protected void ApplyScheme() { }

        virtual protected void LoadImportingSchemes(List<ImportingScheme> schemes) { }

        /// <summary>
        /// обновить индекс активного элемента
        /// </summary>
        private void RefreshActiveElementIndex()
        {
            ActiveFileElementIndex = 0;
            ActiveFileElementIndex = 1;
        }


        /// <summary>
        /// метод инициализации импортера: открывает файл, читает его
        /// </summary>
        virtual protected void Open()
        {
            Contract.Requires(ActiveReader != null);
            Contract.Ensures(Active);
            ActiveReader.Open();
            ActiveReader.AcceptParameters(this);

            InitPrgBar();


            SendPropertyChanged("StartColumn");
            SendPropertyChanged("FinishColumn");
            SendPropertyChanged("StartRow");
            SendPropertyChanged("FinishRow");
            SendPropertyChanged("ColCount");
            SendPropertyChanged("RowCount");

            RefreshActiveElementIndex();

            ActiveReader.Read();
            ApplyScheme();




        }

        /// <summary>
        /// метод завершения работы с файлом
        /// </summary>
        virtual protected void Close()
        {

            Contract.Ensures(!Active);
            if (ActiveReader != null) ActiveReader.Close();
        }


        /// <summary>
        /// очистка списка конверторов, нужна для повторного использования Importer'а
        /// </summary>
        public void ClearReaders()
        {
            foreach (BaseReader r in Readers)
            {
                r.Clear();
            }
            Readers.Clear();
        }


        public BaseImporter(IContractRepository repository, Schedule schedule, Contractdoc contractdoc)
        {
            _scheme = DefaultImportingScheme.Scheme;
            _repository = repository;
            this.schedule = schedule;
            this.contractdoc = contractdoc;
        }
        #endregion

        #region Members of IReaderCommonParameters
        /// <summary>
        /// выбранный лист (таблица в случае MS Word)
        /// </summary>
        public int ActiveFileElementIndex
        {
            get
            {
                if (ActiveReader != null)
                    return ActiveReader.ActiveFileElementIndex;
                else
                    return 1;

            }

            set
            {
                if (ActiveReader != null)
                    ActiveReader.ActiveFileElementIndex = value;
                SendPropertyChanged("ActiveFileElementIndex");
            }
        }

        /// <summary>
        /// начальная строка предназначенного для чтения региона
        /// </summary>
        public int? StartRow
        {
            get
            {
                if (ActiveReader != null)
                    return (ActiveReader as IReaderCommonParameters).StartRow;
                else
                    return 1;
            }

            set
            {
                if (ActiveReader != null)
                {
                    (ActiveReader as IReaderCommonParameters).StartRow = value;
                    //FinishRow = StartRow + 1;
                    SendPropertyChanged("Error");
                    SendPropertyChanged("StartRow");
                    SendPropertyChanged("FinishRow");

 
                }
            }
        }

        /// <summary>
        /// конечная строка предназначенного для чтения региона
        /// </summary>
        public int? FinishRow
        {
            get
            {
                if (ActiveReader != null)
                    return (ActiveReader as IReaderCommonParameters).FinishRow;
                else
                    return null;
            }

            set
            {
                if (ActiveReader != null)
                {
                    (ActiveReader as IReaderCommonParameters).FinishRow = value;
                    SendPropertyChanged("FinishRow");
                    SendPropertyChanged("StartRow");
                    SendPropertyChanged("Error");
                }
            }

        }

        /// <summary>
        /// начальный столбец предназначенного для чтения региона
        /// </summary>
        public int? StartColumn
        {
            get
            {
                if (Scheme != null)
                    return Scheme.StartCol;
                else
                    return 1;
            }

            set
            {
                if (Scheme != null)
                {
                    Scheme.StartCol = (value != null) ? value.Value : 1;
                }

                if (ActiveReader != null)
                {
                    (ActiveReader as IReaderCommonParameters).StartColumn = value;
                }
                
                //FinishColumn = StartColumn + 1;

                SendPropertyChanged("StartColumn");
                SendPropertyChanged("FinishColumn");
                SendPropertyChanged("Error");
            }
        }

        /// <summary>
        /// конечный столбец предназначенного для чтения региона
        /// </summary>
        public int? FinishColumn
        {
            get
            {
                if (Scheme != null)
                    return Scheme.FinishCol;
                else
                    return null;
            }

            set
            {

                if (Scheme != null)
                {
                    Scheme.FinishCol = (value != null) ? value.Value : 15;
                }

                if (ActiveReader != null)
                {
                    (ActiveReader as IReaderCommonParameters).FinishColumn = value;
                }

                SendPropertyChanged("FinishColumn");
                SendPropertyChanged("StartColumn");
                SendPropertyChanged("Error");
            }
        }

        #endregion

        
        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return this.Validate();
            }
        }


        class NonIntegerRegionBoundValuesHandler : IDataErrorHandler<BaseImporter>
        {
            public string GetError(BaseImporter source, string propertyName, ref bool handled)
            {
                if (propertyName == "StartRow" || propertyName == "FinishRow")
                {
                    return HandleRowBoundsError(source, out handled);
                }
                else if (propertyName == "StartColumn" || propertyName == "FinishColumn")
                {
                    return HandleColumnBoundsError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleRowBoundsError(BaseImporter source, out bool handled)
            {
                handled = false;
                if (source.StartRow > source.FinishRow)
                {
                    handled = true;
                    return Resource.Importer_this_Region_bound_row_values_error;
                }
                return string.Empty;
            }

            private static string HandleColumnBoundsError(BaseImporter source, out bool handled)
            {
                handled = false;
                if (source.StartColumn >= source.FinishColumn)
                {
                    handled = true;
                    return Resource.Importer_this_Region_bound_column_values_error;
                }
                return string.Empty;
            }
        }

        private readonly DataErrorHandlers<BaseImporter> _errorHandlers = new DataErrorHandlers<BaseImporter>()
                                                                             {
                                                                                 new NonIntegerRegionBoundValuesHandler()
                                                                             };
        

        
        public string this[string columnName]
        {
            get
            {
                return _errorHandlers.HandleError(this, columnName);
            }
        }

        #endregion
    }
}
