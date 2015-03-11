using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using MCDomain.DataAccess;

namespace MCDomain.Importer
{

    public class ImportingSchemeItem : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private double _dbid;
        /// <summary>
        /// идентификатор элемента схемы в БД
        /// </summary>
        public double DbId
        {
            get { return _dbid; }
            set
            {
                if (_dbid == value) return;
                _dbid = value;
            }
        }


        private int _col = -1;
        /// <summary>
        /// столбец привязки (пока считаем что схема накладывается на таблицы,
        /// в которых для каждого элемента данных выделена одна строка)
        /// </summary>
        public int Col
        {
            get { return _col; }
            set
            {
               Contract.Ensures(_col == value);
               if (_col == value) return;
               
               if (value != -1)
               {
                   _iscolchanged = true;
               }
               else if ((_col != -1)&&(value == -1))
               {
                   _iscolchanged = false;
               }
               
               _col = value;
               DefaultCol = value;
                
               SendPropertyChanged("Col");
            }

        }
        
        private int _defaultcol = -1;
        /// <summary>
        /// столбец по умолчанию - устанавливается один раз при добавлении
        /// </summary>
        public int DefaultCol
        {
            get { return _defaultcol; }
            set
            {
                if (_defaultcol != -1) return;
                _defaultcol = value;

            }
        }

        public void SetDefaultCol ()
        {
            _col = _defaultcol;
        }

        private bool _iscolchanged;
        /// <summary>
        /// менялось ли значение для столбца в схеме
        /// </summary>
        /// <returns></returns>
        public bool IsColChanged()
        {
            return _iscolchanged;
        }


 

        private string _code;
        /// <summary>
        /// код колонки 
        /// коды прописаны в схеме по умолчанию
        /// каждый код уникален 
        /// и определяет какому свойству конвертируемого объекта 
        /// будут приписываться данные
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                Contract.Ensures(_code == value);
                if (_code == value) return;
                _code = value;
            }
        }

        /// <summary>
        /// наименование колонки - для отображения в интерфейсе
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Contract.Ensures(_name == value);
                if (_name == value) return;
                _name = value;
            }
        }


        public override string ToString()
        {
            return Name;
        }

        private bool _isrequired;
        /// <summary>
        /// является поле обязательным или нет
        /// </summary>
        public bool IsRequired
        {
            get { return _isrequired; }
            set
            {
                Contract.Ensures(_isrequired == value);
                if (_isrequired == value) return;
                _isrequired = value;
            }
        }
    }

    public class ImportingSchemeItems : List<ImportingSchemeItem>
    {

        public void SetDefaultCols()
        {
            foreach (ImportingSchemeItem r in this)
            {
                r.SetDefaultCol();
            }
        }
        /// <summary>
        /// добавить новый элемент схемы
        /// </summary>
        /// <param name="name">наименование элемента</param>
        /// <param name="code">код элемента</param>
        /// <param name="col">столбец</param>
        /// <returns></returns>
        public ImportingSchemeItem AddNewItem(double dbid, string name, string code, double col, bool isrequired)
        {

            ImportingSchemeItem r = this[code];
            if (r == null)
            {
                r = new ImportingSchemeItem();
                r.DbId = dbid;
                r.Name = name;
                r.Code = code;
                r.Col = (int)col;
                r.DefaultCol = (int)col;
                r.IsRequired = isrequired;
                Add(r);
            }
            return r;
        }


        /// <summary>
        /// поиск в коллекции по коду
        /// </summary>
        /// <param name="code">код</param>
        /// <returns>элемент схемы, код которого совпадает с параметром</returns>
        public ImportingSchemeItem GetItemByCode(string code)
        {
           return Find(x => x.Code == code);
        }

        /// <summary>
        /// искать в коллекции по имени
        /// </summary>
        /// <param name="name">имя</param>
        /// <returns>элемент схемы, имя которого совпадает с параметром</returns>
        public ImportingSchemeItem GetItemByName(string name)
        {
            return Find(x => x.Name == name);
        }

        public ImportingSchemeItem GetItemByColumn(int col)
        {
            return Find(x => x.Col == col);
        }

        /// <summary>
        /// дополнительный индексатор по коду
        /// </summary>
        /// <param name="code">код</param>
        /// <returns>элемент схемы</returns>
        public ImportingSchemeItem this[string code]
        {
            get { return GetItemByCode(code); }
        }


        public bool IsAllRequiredFieldsFilled()
        {
            List<ImportingSchemeItem> lst = FindAll(x => x.IsRequired);
            return lst.All(x => x.IsColChanged());
        }

        public int MaxCol()
        {
            int m = 0;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Col > m) m = this[i].Col;
            }
            return m;
        }

        public void RestoreDefaultCols()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Col = this[i].DefaultCol;
            }
        }
      }
    
 

    
    public class ImportingScheme : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double _dbid;
        /// <summary>
        /// идентификатор схемы в БД
        /// </summary>
        public double DbId
        {
            get { return _dbid; }
            set
            {
                if (_dbid == value) return;
                _dbid = value;
            }
        }

        private string _schemename;
        /// <summary>
        /// наименование схемы
        /// </summary>
        public string SchemeName
        {
            get { return _schemename; }
            set {
                    Contract.Ensures(_schemename == value);
                    if (_schemename == value) return;
                    _schemename = value;
                    SendPropertyChanged("SchemeName");
                }
        }
         
        private ImportingSchemeItems   _items;
        public ImportingSchemeItems  Items 
        {
            get
            {
                Contract.Ensures(_items != null);
                if (_items == null) _items = new ImportingSchemeItems();
                return _items;
            }
        }

        public override string ToString()
        {
            return SchemeName;
        }

        private int _startcol = 1;
        /// <summary>
        /// начальный столбец в схеме
        /// </summary>
        public int StartCol
        {
            get
            {
                return _startcol;
            }

            set
            {
                if (_startcol == value) return;
                _startcol = value;
            }
        }

        private int _finishcol = 15;
        /// <summary>
        /// конечный столбец в схеме
        /// </summary>
        public int FinishCol
        {
            get
            {
                return _finishcol;
            }

            set
            {
                if (_finishcol == value) return;
                _finishcol = value;
            }


        }

    }

    public class DefaultImportingScheme : ImportingScheme
    {
        private DefaultImportingScheme()
        {
            SchemeName = "Схема по умолчанию";
            Items.AddNewItem(0, "Номер этапа", "num", 0, true);
            Items.AddNewItem(0, "Наименование работ", "subject", 2, true);
            Items.AddNewItem(0, "Начало", "startsat", 3, false);
            Items.AddNewItem(0, "Окончание", "endsat", 4, false);
            Items.AddNewItem(0, "Стоимость работ", "price", 5, false);
            Items.AddNewItem(0, "Код объекта", "objectcode", -1, false);
            Items.AddNewItem(0, "Результаты", "result", -1, false);
            Items.AddNewItem(0, "Начало - окончание", "dateinterval", -1, false);
            Items.AddNewItem(0, "Соисполнитель", "contributor", -1, false);
        }

        private static DefaultImportingScheme _scheme;
        public static DefaultImportingScheme Scheme
        {
            get
            {
                if (_scheme == null)
                    _scheme = new DefaultImportingScheme();

                return _scheme;
            }
        }
    }
}
