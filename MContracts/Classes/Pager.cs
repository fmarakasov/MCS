using System;
using System.ComponentModel;
using System.Windows.Input;
using UIShared.Commands;


namespace MContracts.Classes
{
    /// <summary>
    /// Класс управления страничным разбиением данных
    /// </summary>
    public class Pager : INotifyPropertyChanged
    {
        private int _currentPage;
        private ICommand _nextPageCommand;
        private ICommand _prevPageCommand;

        /// <summary>
        /// Создаёт экземпляр пейджера
        /// </summary>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <param name="count">Всего элементов</param>
        public Pager(int pageSize, int count)
        {
            Init(pageSize, count);
        }

        /// <summary>
        /// Получает размер страницы
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Получает количество элементов в списке
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Получает или устанавливает текущую страницу. Индексация страниц начинается с 1
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value) return;
                if (!ValidPage(value)) throw new ArgumentException("Неверный индекс страницы");
                _currentPage = value;
                SendPageChanged();
                SendPropertyChanged("CurrentPage");
                SendPropertyChanged("CanNext");
                SendPropertyChanged("CanPrev");
                SendPropertyChanged("ToolTip");
                SendPropertyChanged("Index");
            }
        }

        /// <summary>
        /// Получает команду перехода к следующей странице
        /// </summary>
        public ICommand NextPageCommand
        {
            get
            {
                if (_nextPageCommand == null)
                    _nextPageCommand = new RelayCommand(a => Next(), p => CanNext);
                return _nextPageCommand;
            }
        }

        /// <summary>
        /// Получает команду перехода к предыдущей странице
        /// </summary>
        public ICommand PrevPageCommand
        {
            get
            {
                if (_prevPageCommand == null)
                    _prevPageCommand = new RelayCommand(a => Prev(), p => CanPrev);
                return _prevPageCommand;
            }
        }

        /// <summary>
        /// Получает общее количество страниц
        /// </summary>
        public int PageCount
        {
            get { return (Count/PageSize + ((Count%PageSize == 0) ? 0 : 1)); }
        }

        /// <summary>
        /// Получает признак возможности перехода к следующей странице
        /// </summary>
        private bool CanNext
        {
            get { return PageCount > CurrentPage; }
        }

        /// <summary>
        /// Получает признак возможности перехода к предыдущей странице
        /// </summary>
        private bool CanPrev
        {
            get { return CurrentPage > 1; }
        }

        /// <summary>
        /// Формирует строку с информацией о текущей странице
        /// </summary>
        public string ToolTip
        {
            get { return string.Format("Страница {0} из {1}", CurrentPage, PageCount); }
        }

        

        /// <summary>
        /// Получает индекс элемента начала текущей страницы
        /// </summary>
        public int Index
        {
            get { return (CurrentPage-1)*PageSize; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Событие изменения страницы
        /// </summary>
        public event EventHandler PageChanged;


        private void SendPageChanged()
        {
            if (PageChanged != null)
                PageChanged(this, EventArgs.Empty);
        }

        private void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool ValidPage(int value)
        {
            return (value > 0) && (value <= PageCount);
        }

        /// <summary>
        /// Переход к следующей странице
        /// </summary>
        public void Next()
        {
            if (CanNext)

                ++CurrentPage;
            else
                throw new InvalidOperationException("Индекс текущей страницы не может быть больше числа страниц.");
        }

        /// <summary>
        /// Переход к предыдущей странице
        /// </summary>
        public void Prev()
        {
            if (CanPrev)
                --CurrentPage;
            else
                throw new InvalidOperationException("Индекс текущей страницы не может быть меньше нуля.");
        }

        private void Init(int pageSize, int count)
        {
            if (pageSize > count)
                pageSize = count;

            PageSize = pageSize;
            Count = count;
            CurrentPage = 1;
        }

        public void Last()
        {
            CurrentPage = PageCount;
        }
        public void First()
        {
            CurrentPage = 1;
        }
    }
}