using System;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using CommonBase.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using WindowsTaskDialog;

namespace AppExceptions
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class HandleExceptionDialog : IExceptionHandler
    {
        public static string FileVersion { get; set; }

        public static string LogFileName { get; set; }
        public static string ContextFilePath { get; set; }

        private string GetLogFileName()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + LogFileName;
        }

        readonly TaskDialog _taskDialog = new TaskDialog();

        public HandleExceptionDialog()
        {
          
        }

        public HandleExceptionDialog(NameValueCollection attributes)
        {
            _taskDialog.Callback += dlg_Callback;

        }



        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            Contract.Requires(exception != null);
            Contract.Ensures(Contract.Result<Exception>() != null);
            try
            {
                var manager = ExceptionFilterManager.Instance;
                Contract.Assert(manager!=null);
                //Применить проекцию исключения
                var targetException = manager.Mappers.MapException(exception);
                Contract.Assert(targetException != null);
                // Получить описание исключения
                var description = manager.DescriptionResolvers.GetDescription(targetException);
                Contract.Assert(!description.Equals(default(ExceptionDescription)));

                InitializeExceptionDialog(description);
                
                _taskDialog.Show(null);
            }
            catch (Exception e)
            {
                Logger.Write("Ошибка создания TaskDialog:" + e.Message);
                MessageBox.Show(exception.AggregateMessages(),
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return exception;
        }

        private void InitializeExceptionDialog(ExceptionDescription description)
        {
            _taskDialog.Content = description.Content;
            _taskDialog.MainIcon = description.Icon.ToTaskDialogIcon();
            _taskDialog.MainInstruction = description.Instruction;
            _taskDialog.WindowTitle = description.Title;
            _taskDialog.EnableHyperlinks = true;
            if (description.ShowFooter)
                _taskDialog.Footer =
                    string.Format(
                        "<A HREF=\"file:///{0}\"> Показать журнал </A>  <A HREF=\"file:///{1}\">Показать данные контекста </A>",
                        GetLogFileName(), ContextFilePath);
        }


        void dlg_Callback(object sender, TaskDialogCallbackEventArgs e)
        {
            if (e.Notification == TaskDialogNotification.HyperlinkClicked)
            {
                //
                // Open link.
                //
                System.Diagnostics.Process.Start(e.Hyperlink);
            }
        }
    }
}
