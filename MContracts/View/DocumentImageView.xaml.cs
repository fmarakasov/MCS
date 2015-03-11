using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MContracts.ViewModel;
using CommonBase;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for DocumentImageView.xaml
    /// </summary>
    public partial class DocumentImageView : UserControl
    {
        public DocumentImageView()
        {
            InitializeComponent();
        }

        void RequestFileNameHandler(object sender, EventParameterArgs<string> e)
        {
            var dlg = new OpenFileDialog {Title = "Выберите файл для закачки"};
            if (dlg.ShowDialog().GetValueOrDefault())
                e.Parameter = dlg.FileName;
        }
        void RequestDownloadFileNameHandler(object sender, EventParameterArgs<string> e)
        {
            var dlg = new SaveFileDialog
                {
                    Title = "Укажите путь для загрузки файла",
                    FileName = e.Parameter,
                    InitialDirectory = GetInitialFolder()
                };
            e.Parameter = dlg.ShowDialog().GetValueOrDefault() ? dlg.FileName : string.Empty;
        }

        private static string GetInitialFolder()
        {
            return Properties.Settings.Default.DocumentDefFolder.Return(x => x,
                                                                        Environment.GetFolderPath(
                                                                            Environment.SpecialFolder.MyDocuments));
        }

        DocumentImageViewModel ViewModel
        {
            get { return DataContext as DocumentImageViewModel; }
        }

       
        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            var ctxOld = e.OldValue.NullCastTo<DocumentImageViewModel>();
            var ctxNew = e.NewValue.NullCastTo< DocumentImageViewModel>();
           
            ctxOld.Do(x => x.RequestDownloadFileName -= RequestDownloadFileNameHandler);
            ctxOld.Do(x => x.RequestUploadFileName -= RequestFileNameHandler);
            ctxOld.Do(x => x.RequestDownloadToFolder -= RequestDownloadToFolderHandler);
            ctxOld.Do(x => x.DownloadCompleted -= DownloadCompletedHandler);

            ctxNew.Do(x => x.RequestDownloadFileName += RequestDownloadFileNameHandler);
            ctxNew.Do(x => x.RequestUploadFileName += RequestFileNameHandler);
            ctxNew.Do(x => x.RequestDownloadToFolder += RequestDownloadToFolderHandler);
            ctxNew.Do(x => x.DownloadCompleted += DownloadCompletedHandler);
           

        }

        private void DownloadCompletedHandler(object sender, EventParameterArgs<System.Collections.Generic.IEnumerable<string>> e)
        {
            if (e.Parameter == null || !e.Parameter.Any()) return;
            var result = AppMessageBox.Show("Загрузка документов завершена. Открыть папку с загруженными документами?",
                               MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var fileName = e.Parameter.First();
            ShellFileUtils.LaunchFolderView(fileName);

        }

        private void RequestDownloadToFolderHandler(object sender, EventParameterArgs<string> e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    //dialog.RootFolder = Environment.SpecialFolder.MyDocuments,
                    ShowNewFolderButton = true,
                    Description = "Выберите папку для загрузки документов по договору",
                    SelectedPath = Properties.Settings.Default.DocumentDefFolder
                };
       
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                e.Parameter = dialog.SelectedPath;
            }


        }
    }
}
