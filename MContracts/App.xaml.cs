#region

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using AppExceptions;
using CommonBase;
using CommonBase.Exceptions;
using MCDomain.AOP;
using MCDomain.DataAccess;
using MContracts.Classes;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ninject;
using Ninject.Modules;
using Telerik.Windows.Controls;
using MContracts.Controls.Dialogs;

#endregion

namespace MContracts
{
    internal class IoCModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<
        }
    }

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string DefaultPolicy = "DefaultExceptionPolicy";
        private static IKernel _kernel;

        private static ExceptionManager _manager;
        private SplashScreen _splashScreen;

        /// <summary>
        ///     Получает менеджер исключений
        /// </summary>
        public static ExceptionManager ExceptionManager
        {
            get
            {
                Contract.Ensures(Contract.Result<ExceptionManager>() != null);
                return _manager ?? (_manager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>());
            }
        }


        public static string FileVersionInfo
        {
            get { return Assembly.GetEntryAssembly().AssemblyFileVersion(); }
        }

        public static string AssemblyVersionInfo
        {
            get { return Assembly.GetEntryAssembly().AssemblyVersion(); }
        }

        public static string AssemblyCopyrightInfo
        {
            get { return Assembly.GetEntryAssembly().AssemblyCopyright(); }
        }

        public static string ProductInfo
        {
            get { return Assembly.GetEntryAssembly().AssemblyProduct(); }
        }

        public static string CompanyInfo
        {
            get { return Assembly.GetEntryAssembly().AssemblyCompany(); }
        }
        /// <summary>
        ///     Записывает сообщение в журнал
        /// </summary>
        /// <param name="logMessage"></param>
        public static void LogMessage(string logMessage)
        {
            Logger.Write(logMessage);
        }

        //private IApplicationExceptionHandler _exceptionHandler;

        /// <summary>
        ///     Метод обработки исключения в соответствие с установленной политикой
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <returns>Истина, если исключение обработано</returns>
        public static bool HandleException(Exception exception)
        {
            return ExceptionManager.HandleException(exception, DefaultPolicy);
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            SetupEventHandlers();
            
            SetupBinds();

            SetupExceptionDialog();

            InitQueryProvider();

            if (e.Args.Length > 0)
                ProceedArgs(e.Args);
            
            SetupLocalization();

            AppMessageBox.ApplicationName = MainWindowViewModel.Instance.DisplayName;
            base.OnStartup(e);
        }

        private void SetupEventHandlers()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            MainWindowViewModel.Instance.Do(x => x.Dispose());
        }

        private static void SetupExceptionDialog()
        {
            Logger.SetContextItem("Версия клиента", string.Format("{0} - {1}", AssemblyVersionInfo, FileVersionInfo));
            HandleExceptionDialog.LogFileName = "\\UD\\trace.log";
            HandleExceptionDialog.ContextFilePath = LinqLogWriter.Instance.FilePath;
            ExceptionFilterManager.Instance.AssemblyVersion = AssemblyVersionInfo;
            ExceptionFilterManager.Instance.FileVersion = FileVersionInfo;
            ExceptionFilterManager.Instance.AssemblyTitle = ProductInfo;
            ExceptionFilterManager.Instance.Mappers.Mappers.Add(new TargetInvocationExceptionMapper());
            ExceptionFilterManager.Instance.DescriptionResolvers.Filters.Add(new ApplicationExceptionDescription());
            ExceptionFilterManager.Instance.DescriptionResolvers.Filters.Insert(0, new SaveWorkspaceExceptionDescription());
            LogMessage("Создан файл отладочных данных контекста: " + HandleExceptionDialog.ContextFilePath);
        }

        private static void SetupBinds()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ICoreLogger>().To<DefaultLogger>();
        }

        private void SetupLocalization()
        {
            LocalizationManager.Manager = new DefaultTelerikLocalizationManager();
        }

        private void ProceedArgs(string[] args)
        {
            ProjectStartupInfo.FastLoad = args.Any(x => x.ToUpper() == "/FAST");
            ProjectStartupInfo.CreateNewContract = args.Any(x => x.ToUpper() == "/NEW");
            ProjectStartupInfo.UpdateStatistics = args.Any(x => x.ToUpper() == "/UPDATE-STATISTICS");


            if (!args.Any(x => x.ToUpper().Contains("/C"))) return;

            var a = args.FirstOrDefault(x => x.ToUpper().Contains("/C"));
            if (a == null) return;
            var i = Array.IndexOf(args, a);
            if (args.Length < i) return;
            if (i < 0 || i >= args.Count()) return;
            ProjectStartupInfo.Contracts = args[i + 1];
        }


        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MainWindowViewModel.Instance.With(x => x.ActiveWorkspace).Do(x => x.CollectDiagnosticsData(e.Exception));

            e.Handled = HandleException(e.Exception);
        }

        private void InitQueryProvider()
        {
#if !DEBUG
            MCDomain.DataAccess.ContextFactoryService.Instance.QueryLoginProvider = new DefaultLoginProvider();
#else
            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider("UD", "sys", "XE");
#endif

            // Подписка на событие ошибки создания контекста. Стандартная реакция - завершить процесс.
            ContextFactoryService.Instance.CreateFailed += ContextCreateFailed;
            ContextFactoryService.Instance.Connected += InstanceConnected;
        }

        private void InstanceConnected(object sender, EventArgs e)
        {
            if (_splashScreen != null) return;
            _splashScreen = new SplashScreen("Resources/Promgaz1.png");
            _splashScreen.Show(true);
        }

        private void ContextCreateFailed(object sender, EventParameterArgs<Exception> e)
        {
            LogMessage("Провайдер подключения вернул ошибку " + e.Parameter.With(x => x.Message));
            LogMessage("Завершение приложения с кодом 0xFA");
            Shutdown(0xFA);
        }
    }
}