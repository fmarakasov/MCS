using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace MCDomain.Importer
{



    public class ContributorImporter : BaseImporter
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

        #region Constructor
        public ContributorImporter(IContractRepository repository, Schedule schedule, Contractdoc contractdoc): base(repository, schedule, contractdoc)
        {

        }
        #endregion

        #region Methods


        /// <summary>
        /// загрузка схем (шаблонный метод, вызывается из аксессора Schemes)
        /// пока схемы не типизированы
        /// </summary>
        /// <param name="schemes">коллекция для загрузки схем, соответствует скрытому полю, доступ к которому предоставляется через аксессор</param>
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

                        if ((si.Col < Cells[i].Count) && (si.Col > -1))
                        {

                            if (si.Code == "subject")
                            {
                                ParseSubject(i, s, si);
                            }
                            else if ((si.Code == "startsat") && (!UseDefaultStartDate))
                            {
                                ParseStartAt(i, s, si);
                            }
                            else if ((si.Code == "endsat") && (!UseDefaultFinDate))
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
                            else if (si.Code == "contributor")
                            {
                                ParseContributor(i, s, si);
                            }

                        }

                    }



                }
                if (NextStep != null) NextStep(this, null);
            }
        }

        #region ParsingMethods

        private void ParseContributor(int row, ImportedStage s, ImportingSchemeItem si)
        {
            if (si.Col != -1)
            {
                if ((Cells[row][si.Col].Value != null) && (Cells[row][si.Col].Value != ""))
                {
                    
                    string sContributorName = Cells[row][si.Col].Value;
                    sContributorName = sContributorName.ToUpper().Trim();



                    Contributor ctrb = ContributorList.FirstOrDefault(p => ((p.Name!=null)&&(p.Name.ToString().ToUpper().Trim() == sContributorName)) || ((p.ShortName!=null)&&(p.ShortName.ToString().ToUpper().Trim() == sContributorName)));
                    if (ctrb != null)
                    {
                        s.Contributor = ctrb;
                    }
                    else
                    {
                        ctrb = new Contributor() { Id = 0, Name = Cells[row][si.Col].Value, ShortName = Cells[row][si.Col].Value };
                        ContributorList.Add(ctrb);
                        s.Contributor = ctrb;
                        SendPropertyChanged("ContributorList");
                    }
                }

            }
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
                        int i = 0; string _sforcheck = "";

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

                    // теперь нужно привязать найденный этап к этапу наддоговора
                    if (s != null)
                    {
                        s.GeneralContractStage = Schedule.FindStage(s.Num);
                    }
                    return s;
                }
            }
            return null;
        }

        #endregion


        #endregion

        #region Properties

        public bool UseDefaultNtpsubview
        { get; set; }
        public bool UseDefaultNdsalgorithm
        { get; set; }
        public bool UseDefaultNds
        { get; set; }

        private ObservableCollection<Contributor> _contributorlist;
        public ObservableCollection<Contributor> ContributorList
        {
            get
            {
                if (_contributorlist == null)
                {
                    _contributorlist = new ObservableCollection<Contributor>();

                    Contributor ctrb;
                    foreach (Contractor c in Repository.Contractors)
                    {
                        ctrb = new Contributor();
                        ctrb.Id = c.Id;
                        ctrb.Name = c.Name;
                        ctrb.ShortName = c.Shortname;
                        _contributorlist.Add(ctrb);
                    }

                    SendPropertyChanged("ContributorList");
                }
                return _contributorlist;
            }
        }

        private Ndsalgorithm _defaultndsalgorithm;
        public Ndsalgorithm DefaultNdsalgorithm
        {
            get
            {
                if (_defaultndsalgorithm == null) _defaultndsalgorithm = Contractdoc.Ndsalgorithm ?? Repository.Ndsalgorithms.FirstOrDefault();
                return _defaultndsalgorithm;
            }
            set { _defaultndsalgorithm = value; }
        }

        private Nds _defaultnds;
        public Nds DefaultNds
        {
            get
            {
                if (_defaultnds == null) _defaultnds = Contractdoc.Nds ?? Repository.Nds.FirstOrDefault();
                return _defaultnds;
            }
            set { _defaultnds = value; }
        }

        private Ntpsubview _defaultntpsubview;
        public Ntpsubview DefaultNTPSubview
        {
            get
            {
                if (_defaultntpsubview == null) _defaultntpsubview = Repository.Ntpsubviews.FirstOrDefault();
                return _defaultntpsubview;
            }
            set { _defaultntpsubview = value; }
        }

        private bool _usedefaultstartdate = false;

        /// <summary>
        /// использовать дату начала этапа по умолчанию
        /// </summary>
        public bool UseDefaultStartDate
        {
            get { return _usedefaultstartdate; }
            set { _usedefaultstartdate = value; }
        }

        private bool _usedefaultFindate = false;

        /// <summary>
        /// использовать дату окончания этапа по умолчанию
        /// </summary>
        public bool UseDefaultFinDate
        {
            get { return _usedefaultFindate; }
            set { _usedefaultFindate = value; }
        }

        private DateTime? _defaultstartdate;
        private DateTime? _defaultfindate;

        /// <summary>
        /// дата начала этапа по умолчанию
        /// </summary>
        public DateTime? DefaultStartDate
        {
            get
            {
                if ((!_defaultstartdate.HasValue) && (Contractdoc != null)) _defaultstartdate = Contractdoc.Startat;
                return _defaultstartdate;
            }
            set
            {
                _defaultstartdate = value;
            }
        }

        /// <summary>
        /// дата окончания этапа по умолчанию
        /// </summary>
        public DateTime? DefaultFinDate
        {
            get
            {
                if ((!_defaultstartdate.HasValue) && (Contractdoc != null)) _defaultfindate = Contractdoc.Endsat;
                return _defaultfindate;
            }
            set
            {
                _defaultfindate = value;
            }
        }

        public IEnumerable<Stage> _mainstages;
        /// <summary>
        /// этапы главного договора
        /// </summary>
        public IEnumerable<Stage> MainStages
        {
            get
            {
                return _mainstages;
            }

            set
            {
                if (_mainstages == value) return;
                _mainstages = value;
            }
        }



        #endregion




    }
 
}
