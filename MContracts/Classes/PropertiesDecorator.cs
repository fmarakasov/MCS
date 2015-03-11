using System;
using System.ComponentModel;
using MContracts.Properties;

namespace MContracts.Classes
{
   
    [DisplayName(@"Настройки")]
    public class PropertiesDecorator
    {
        private Settings _settings = Properties.Settings.Default;

        [Description("Отображение панели генеральных договорв на панели слева в редакторе договора")]
        [DisplayName(@"Панель слева")]        
        [Category("Редактор договорв")]
        public bool LeftPanelVisible
        {
            get { return _settings.LeftPanelVisibility; }
            set { _settings.LeftPanelVisibility = value; }
        }

        [Description("Отображение панели субподрядных договорв и соглашений на панели справа в редакторе договора")]
        [DisplayName(@"Панель справа")]
        [Category("Редактор договорв")]
        public bool RightPanelVisible
        {
            get { return _settings.RightPanelVisibility; }
            set { _settings.RightPanelVisibility = value; }
        }

        //[Description("Определяет число элементов на каждой из страниц отображения данных")]
        //[DisplayName(@"Размер страницы")]
        //[Category("Реестры")]
        //public int PageSize
        //{
        //    get { return _settings.PageSize; }
        //    set { _settings.PageSize = value; }
        //}

        //[Description("Производить загрузку договоров, снятых  с контроля. По умолчанию такие договора не загружаются, что может повысить производительность.")]
        //[DisplayName(@"Загрузка договоров, снятых с учёта")]
        //[Category("Реестры")]
        //public bool LoadInactiveContracts
        //{
        //    get { return _settings.LoadInactiveContracts; }
        //    set { _settings.LoadInactiveContracts = value; }
        //}
        [Description("Процент аванса для договоров НИОКР по умолчанию.")]
        [DisplayName(@"Аванс договоров НИОКР (%)")]
        [Category("Редактор договорв")]
        public float DefaultContractPrepaymentPercent
        {
            get { return _settings.DefaultPrepaymentPercent; }
            set { _settings.DefaultPrepaymentPercent = value; }
        }


        [Description("Папка для сохранения результатов отчета")]
        [DisplayName(@"Папка для сохранения результатов отчета")]
        [Category("Отчёты")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string UserReportFolder
        {
            get
            {
                return _settings.UserReportFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            set { _settings.UserReportFolder = value; }
        }

        [Description("Папка для сохранения загружаемых документов")]
        [DisplayName(@"Папка для загрузки документов")]
        [Category("Редактор договорв")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DocumentFolder
        {
            get
            {
                return _settings.DocumentDefFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            set { _settings.DocumentDefFolder = value; }
        }


    }
}
