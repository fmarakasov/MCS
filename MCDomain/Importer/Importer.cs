using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.ComponentModel;
using MCDomain.Common;
using MCDomain.Model;
using System.Collections.ObjectModel;
using MCDomain.DataAccess;



namespace MCDomain.Importer
{
 
 




    /// <summary>
    /// класс, ответственный за импорт
    /// </summary>
    public class Importer: BaseImporter
    {

        #region Events
        /// <summary>
        /// событие инициализации progress bar
        /// </summary>
        public event EventHandler<ProgressEventArgs> InitSteps;
        /// <summary>
        /// событие шага progress bar-a
        /// </summary>
        public event EventHandler<ProgressEventArgs> NextStep;

        #endregion

        #region Properties

        private Contractdoc _originalcontract;
        public Contractdoc Originalcontract
        {
            get { return _originalcontract; }
            set
            {
                _originalcontract = value; 
            }
        }

        public Contractdoc Maincontract
        {
            get { return Generalcontract ?? Originalcontract ?? Contractdoc; }
        }

        private Schedulecontract _originalschedulecontract;
        public Schedulecontract Originalschedulecontract
        {
            get
            {
                //
                //if (scs.Count() == 0) scs = 

                if (_originalschedulecontract == null)
                    _originalschedulecontract =
                        Maincontract.Schedulecontracts.FirstOrDefault(
                            scc =>
                            scc.Schedule.Worktype != null && Schedule.Worktype != null &&
                            scc.Schedule.Worktype.Id == Schedule.Worktype.Id &&
                            Schedule.Schedulecontracts.Any(
                                sss => sss.Appnum.HasValue && scc.Appnum.HasValue && sss.Appnum == scc.Appnum));
                
                if (_originalschedulecontract == null)
                    _originalschedulecontract =
                        Maincontract.Schedulecontracts.FirstOrDefault(
                            scc =>
                            scc.Schedule.Worktype != null && Schedule.Worktype != null &&
                            scc.Schedule.Worktype.Id == Schedule.Worktype.Id);
                
                return _originalschedulecontract;
            }
            set { _originalschedulecontract = value; }
        }

        private Contractdoc _generalcontract;
        public Contractdoc Generalcontract
        {
            get { return _generalcontract; }
            set
            {
                _generalcontract = value;
            }
        }
        
        

        private bool _usedefaultstartdate = false;
        public bool UseDefaultStartDate
        {
            get { return _usedefaultstartdate; }
            set { _usedefaultstartdate = value; }
        }

        private bool _usedefaultFindate = false;
        public bool UseDefaultFinDate
        {
            get { return _usedefaultFindate; }
            set { _usedefaultFindate = value; }
        }

        private DateTime? _defaultstartdate;
        private DateTime? _defaultfindate;

        public DateTime? DefaultStartDate
        {
            get {
                   if ((!_defaultstartdate.HasValue)&&(Contractdoc != null)) _defaultstartdate = Contractdoc.Startat;
                   return _defaultstartdate; 
                }
            set
            {
                _defaultstartdate = value;
            }
        }

        public DateTime? DefaultFinDate
        {
            get {
                   if ((!_defaultfindate.HasValue) && (Contractdoc != null)) _defaultfindate = Contractdoc.Endsat;
                   return _defaultfindate; 
                }
            set
            {
                _defaultfindate = value;
            }
        }

        private Ndsalgorithm _defaultndsalgorithm;
        public Ndsalgorithm DefaultNdsalgorithm
        {
            get {
                  if (_defaultndsalgorithm == null) _defaultndsalgorithm = Contractdoc.Ndsalgorithm ?? Repository.Ndsalgorithms.FirstOrDefault();
                  return _defaultndsalgorithm; 
                }
            set { _defaultndsalgorithm = value; }
        }

        private Nds _defaultnds;
        public Nds DefaultNds
        {
            get {
                   if (_defaultnds == null) _defaultnds = Contractdoc.Nds ?? Repository.Nds.FirstOrDefault();
                   return _defaultnds; 
                }
            set { _defaultnds = value; }
        }

        private Ntpsubview _defaultntpsubview;


        public Ntpsubview DefaultNTPSubview
        {
            get {
                  if (_defaultntpsubview == null) _defaultntpsubview = Repository.Ntpsubviews.FirstOrDefault();
                  return _defaultntpsubview; 
                }
            set { _defaultntpsubview = value; }
        }



        #endregion

        #region Methods

        protected override void LoadImportingSchemes(List<ImportingScheme> schemes)
        {
            base.LoadImportingSchemes(schemes);

            schemes.Add(DefaultImportingScheme.Scheme);
            // при первом обращении нужно наполнить коллекцию из БД
            ImportingScheme si;
            foreach (Importingscheme dbsi in Repository.Importingschemes)
            {
                si = new ImportingScheme() { DbId = dbsi.Id, SchemeName = dbsi.Schemename };
                schemes.Add(si);

                foreach (Importingschemeitem dbsicol in dbsi.Importingschemeitems)
                {
                    if ((dbsicol.Columnsign != "finishcol") && (dbsicol.Columnsign != "startcol"))
                    {
                        si.Items.AddNewItem(dbsicol.Id, dbsicol.Columnname, dbsicol.Columnsign, dbsicol.Columnindex, dbsicol.Isrequired);
                    }
                    else
                    {
                        if (dbsicol.Columnsign == "finishcol") si.FinishCol = (int)dbsicol.Columnindex;
                        else if (dbsicol.Columnsign == "startcol") si.StartCol = (int)dbsicol.Columnindex;
                    }
                }
            }
        }

        protected override void InitPrgBar()
        {
            
            base.InitPrgBar();
            if (InitSteps != null) ActiveReader.InitSteps += InitSteps;
            if (NextStep != null) ActiveReader.NextStep += NextStep;

        }


        /// <summary>
        /// метод, позволяющий применить схему конвертации
        /// </summary>
        override protected void ApplyScheme()
        {
            
            Contract.Assert(Scheme != null);
            Contract.Assert(Cells != null);
            Contract.Assert(Stages != null);

            base.ApplyScheme();

            int i; int j;

            if (_stages == null) _stages = new ObservableCollection<ImportedStage>();
            else
            {
                _stages.Clear();
            }

            ImportedStage s;
            ImportingSchemeItem si;

            DataContextDebug.DebugPrintRepository(Repository);

            for (i = 0; i < Cells.RowCount; i++)
            {
                s = null;
                // при первом проходе находим номер
                // он позволяет правильно разместить этап
                si = Scheme.Items.GetItemByCode("num");
                if (si != null)
                {
                    s = ParseStageNum(i, si);
                }

                if (s != null)
                {
                    // все остальные поля вроде бы равноценны
                    for (j = 0; j < Scheme.Items.Count; j++)
                    {
                        si = Scheme.Items[j];

                        if ((si.Col < Cells[i].Count)&&(si.Col > -1))
                        {

                            if (si.Code == "subject")
                            {
                                ParseSubject(i, s, si);
                            }
                            else if ((si.Code == "startsat")&&(!UseDefaultStartDate))
                            {
                                ParseStartAt(i, s, si);
                            }
                            else if ((si.Code == "endsat")&&(!UseDefaultFinDate))
                            {
                                ParseEndsAt(i, s, si);
                            }
                            else if (si.Code == "dateinterval")
                            {
                                ParseDateInterval(i, s, si);
                            }
                            else if (si.Code == "price")
                            {
                                ParsePrice(i, s, si);
                            }
                            else if (si.Code == "objectcode")
                            {
                                ParseObjectCode(i, s, si);
                            }
                            else if (si.Code == "result")
                            {
                                ParseResult(i, s, si);
                            }

                        }

                    }

                    ParseStageGeneralHierarchy(s);

                }
                if (NextStep != null) NextStep(this, null);
            }
        }
        

        #region ParsingMethods


        private void ParseOriginal (ImportedStage s)
        {
            if (Originalcontract != null)
            {
                var sc = Originalschedulecontract;

                if (sc != null)
                {
                    // if (scs.Count() == 0) scs = Originalcontract.Schedulecontracts;

                    Stage st = null;
                    if (StageBindingSetting == ImporterStageBindingSetting.BindByStageNum)
                    {

                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Num.Trim() == s.Num.Trim());

                    }
                    else if (StageBindingSetting == ImporterStageBindingSetting.BindByObjectCode)
                    {
                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Code.Trim() == s.Code.Trim());
                    }
                    else if (StageBindingSetting == ImporterStageBindingSetting.BindBySubject)
                    {

                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Subject.Trim() == s.Subject.Trim());
                    }


                    // если нашли этап - проверяем что для него совпадает ряд параметров
                    if (st != null)
                    {

                        if (((st.ParentStage != null) && (st.Price == s.Price) && (st.Startsat == s.Startsat) &&
                             (st.Endsat == s.Endsat)) || (st.ParentStage == null))
                        {
                            s.OriginContractStage = (st.Stagecondition == StageCondition.Closed) ? st : null;
                            return;
                        }
                    }


                }
                s.OriginContractStage = null;
            }
        }


        private void ParseGeneral(ImportedStage s)
        {
            if (Generalcontract != null)
            {
                var sc = Originalschedulecontract;

                if (sc != null)
                {
                    Stage st = null;

                    if (StageBindingSetting == ImporterStageBindingSetting.BindByStageNum)
                    {

                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Num.Trim() == s.Num.Trim());

                    }
                    else if (StageBindingSetting == ImporterStageBindingSetting.BindByObjectCode)
                    {
                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Code.Trim() == s.Code.Trim());
                    }
                    else if (StageBindingSetting == ImporterStageBindingSetting.BindBySubject)
                    {

                        st = sc.Schedule.Stages.FirstOrDefault(x => x.Subject.Trim() == s.Subject.Trim());

                    }


                    // если нашли этап - проверяем что для него совпадает ряд параметров
                    if (st != null)
                    {

                        s.GeneralContractStage = st;
                        return;
                    }

                }
                s.GeneralContractStage = null;
            }
        }
        /// <summary>
        /// указать принадлежность к старшему
        /// </summary>
        /// <param name="s">готовый прочитанный из источника этап</param>
        private void ParseStageGeneralHierarchy(ImportedStage s)
        {
            ParseOriginal(s);
            ParseGeneral(s);
        }

        /// <summary>
        /// разобрать результат
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseResult(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {

                    char[] delimiters = { '\r', '\n', '\a' };
                    string[] _sres = Cells[row][si.Col].Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string stmp;
                    bool buseddefault;
                    foreach (string ss in _sres)
                    {
                        stmp = ss.Trim();
                        Ntpsubview ntpsubview = _defaultntpsubview;
                        buseddefault = true;
                        foreach (Ntpsubview n in Repository.Ntpsubviews)
                        {
                            if (n.Shortname != null)
                            {
                                if ((n.Shortname.Trim() != "") && (ss.Contains(n.Shortname + " ")))
                                {
                                    ntpsubview = n;
                                    buseddefault = false;
                                    break;
                                }
                            }
                        }

                        DataContextDebug.DebugPrintRepository(Repository);

                        double ntpsubviewid = 0;
                        if (ntpsubview != null) ntpsubviewid = ntpsubview.Id;
                        s.Stageresults.Add(new ImportedStageResult()
                                               {
                                                   Name = ss,
                                                   Ntpsubviewid = ntpsubviewid,
                                                   Stage = s,
                                                   UsedDefaultNTPSubView = buseddefault
                                               });

                        DataContextDebug.DebugPrintRepository(Repository);
                    }
                }
            }
        }
        
        /// <summary>
        /// разобрать код стройки
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseObjectCode(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    s.Code = Cells[row][si.Col].Value;

                }
            }
        }

        /// <summary>
        /// разобрать стоимость работ
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParsePrice(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    double _dbl;
                    string str;
                    // меняем  концы строк
                    str = Cells[row][si.Col].Value.Replace("\r", " ");
                    str = str.Replace("\a", " ");
                    str = str.Trim();

                    if (Double.TryParse(str, out _dbl))
                    {
                        s.Price = Convert.ToDecimal(str);
                    }
                }
            }
        }

        /// <summary>
        /// разобрать дату окончания
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseEndsAt(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    System.DateTime _dt;
                    string str;
                    str = Cells[row][si.Col].Value.Replace("\r", " ");
                    str = str.Replace("\a", " ");
                    str = str.Trim();

                    if (DateTime.TryParse(str, out _dt))
                    {
                        s.Endsat = DateTime.Parse(str);
                    }
                }
            }
        }

        /// <summary>
        /// разобрать дату начала
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseStartAt(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    System.DateTime _dt;
                    string str;
                    str = Cells[row][si.Col].Value.Replace("\r", " ");
                    str = str.Replace("\a", " ");
                    str = str.Trim();
                    if (DateTime.TryParse(str, out _dt))
                    {
                        s.Startsat = DateTime.Parse(str);
                    }
                }
            }
        }

        /// <summary>
        /// разобрать интервал дат
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseDateInterval(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    System.DateTime _dt;
                    string str;
                    str = Cells[row][si.Col].Value.Replace("\r", " ");
                    str = str.Replace("\a", " ");
                    str = str.Trim();

                    char[] delimiters = { '-', '–' };
                    string[] _sres = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (DateTime.TryParse(_sres[0], out _dt))
                    {
                        s.Startsat = DateTime.Parse(_sres[0]);
                    }

                    if (DateTime.TryParse(_sres[1], out _dt))
                    {
                        s.Endsat = DateTime.Parse(_sres[1]);
                    }
                }
            }
        }

        /// <summary>
        /// разобрать наименование этапа
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="s">текущий этап</param>
        /// <param name="si">текущая схема импорта</param>
        private void ParseSubject(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                    s.Subject = Cells[row][si.Col].Value;
            }
        }

        /// <summary>
        /// разобрать номер этапа и создать этап
        /// </summary>
        /// <param name="row">строка</param>
        /// <param name="si">текущая схема импорта</param>
        /// <returns>созданный этап</returns>
        private ImportedStage ParseStageNum(int row, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                ImportedStage s = null;
                if (si.Col >= Cells[row].Count) return null;
                
                string _stageNum = Cells[row][si.Col].Value.Trim();

                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    

                    s = new ImportedStage();

                    if (UseDefaultStartDate && DefaultStartDate.HasValue)
                        s.Startsat = DefaultStartDate.Value;

                    if (UseDefaultFinDate && DefaultFinDate.HasValue)
                        s.Endsat = DefaultFinDate.Value;

                    if (_stageNum.EndsWith("."))
                    {
                        _stageNum = _stageNum.Substring(0, _stageNum.Length - 1);
                         s.Num = _stageNum;

                        _stages.Add(s);
                    }
                    else  // подэтап
                    {
                        char[] charSeparators = new char[] { '.', '\r', '\a' };
                        string[] _substagenums = _stageNum.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                        s.Num = _stageNum;
                        IList<ImportedStage> _tmpstages = _stages;
                        ImportedStage _superstage;
                        ImportedStage _lastsuperstage = null;
                        int i =0; string _sforcheck = "";

                        foreach (string _substagenum in _substagenums)
                        {
                            if (_substagenum.Trim() != "")
                            {
                                i++;
                                _sforcheck = _sforcheck + _substagenum;
                                try
                                {
                                    _superstage = _tmpstages.First(x => x.Num == _sforcheck);
                                }
                                catch (InvalidOperationException)
                                {
                                    _superstage = null;
                                }



                                if (_superstage != null)
                                {
                                    _tmpstages = _superstage.Stages;
                                    _lastsuperstage = _superstage;
                                }
                                else
                                {

                                    _tmpstages.Add(s);
                                    if (_lastsuperstage != null)
                                    {
                                        s.Parent = _lastsuperstage;
                                        

                                        if (!UseDefaultStartDate)
                                                s.Startsat = _lastsuperstage.Startsat;

                                        if (!UseDefaultFinDate)
                                                s.Endsat = _lastsuperstage.Endsat;

                                       
                                    }
                                 }
                                _sforcheck = _sforcheck + ".";
                            }
                        }
                    }

                return s;
            }
            }
            return null;
        }
        #endregion

        #endregion

        #region Constructor 

        public Importer(IContractRepository repository, Schedule schedule, Contractdoc contractdoc): base(repository, schedule, contractdoc)
        {
            _defaultntpsubview = repository.Ntpsubviews.FirstOrDefault();
            SendPropertyChanged("DefaultFinDate");
            SendPropertyChanged("DefaultStartDate");
            SendPropertyChanged("DefaultNds");
            SendPropertyChanged("DefaultNdsalgorithm");
            SendPropertyChanged("DefaultNTPSubview");

        }

        #endregion


    }
}
