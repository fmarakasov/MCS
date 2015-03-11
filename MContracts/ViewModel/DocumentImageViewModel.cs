using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public class DocumentImageViewModel : ContractdocBaseViewModel
    {
        private IBindingList _documentImages;


        public IEnumerable<Document> Documents
        {
            get { return Repository.TryGetContext().Documents; }
        }

        /// <summary>
        /// Получает коллекцию файлов документов
        /// </summary>
        public IBindingList DocumentImages
        {
            get { return _documentImages ?? (_documentImages = ContractObject.Contractdocdocumentimages.GetNewBindingList()); }
        }

        public event EventHandler<EventParameterArgs<string>> RequestUploadFileName;
        public event EventHandler<EventParameterArgs<string>> RequestDownloadFileName;
        public event EventHandler<EventParameterArgs<string>> RequestDownloadToFolder;
        public event EventHandler<EventParameterArgs<IEnumerable<string>>> DownloadCompleted;

        protected virtual void OnDownloadCompleted(EventParameterArgs<IEnumerable<string>> e)
        {
            EventHandler<EventParameterArgs<IEnumerable<string>>> handler = DownloadCompleted;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnRequestDownloadToFolder(EventParameterArgs<string> e)
        {
            EventHandler<EventParameterArgs<string>> handler = RequestDownloadToFolder;
            if (handler != null) handler(this, e);
        }

        public void OnRequestDownloadFileName(EventParameterArgs<string> e)
        {
            EventHandler<EventParameterArgs<string>> handler = RequestDownloadFileName;
            if (handler != null) handler(this, e);
        }

        public void OnRequestFileName(EventParameterArgs<string> e)
        {
            EventHandler<EventParameterArgs<string>> handler = RequestUploadFileName;
            if (handler != null) handler(this, e);
        }

        [ApplicationCommand("Добавить документ", "/MContracts;component/Resources/upload.png")]
        public RelayCommand AddDocumentCommand
        {
            get { return new RelayCommand((x) => AddDocument()); }
        }

        [ApplicationCommand("Удалить документ", "/MContracts;component/Resources/remove.png",
            AppCommandType.Confirm, "Удалить документ из хранилища?")]
        public RelayCommand DeleteDocumentCommand
        {
            get { return new RelayCommand((x) => DeleteDocument(), (x) => SelectedDocumentImage != null); }
        }

        [ApplicationCommand("Загрузить документ",
            "/MContracts;component/Resources/database_download.png")]
        public RelayCommand DownloadDocumentCommand
        {
            get { return new RelayCommand((x) => DownloadDocument(), (x) => SelectedDocumentImage != null); }
        }

        [ApplicationCommand("Загрузить все документы",
            "/MContracts;component/Resources/database_download.png")]
        public RelayCommand DownloadAllDocumentsCommand
        {
            get { return new RelayCommand((x) => DownloadAllDocuments()); }
        }

        private void DownloadAllDocuments()
        {
            var args = new EventParameterArgs<string>(null);
            OnRequestDownloadToFolder(args);
            if (string.IsNullOrWhiteSpace(args.Parameter)) return;
            var images = ContractObject.Contractdocdocumentimages.ToList();
            var downloaded = new List<string>(images.Count);
         
            foreach (var documentImage in images)
            {
                var path = BuildPath(args.Parameter, documentImage.Documentimage.Document);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filePath = path + "\\" + documentImage.Documentimage.Physicalname;
                downloaded.Add(filePath);
                _fileBytesProvider.SaveFile(documentImage.Documentimage.Image, filePath);

            }
            OnDownloadCompleted(new EventParameterArgs<IEnumerable<string>>(downloaded));
        }

        private string BuildPath(string path, Document document)
        {
            if (document != null && document.Foldername != null) return path +"\\"+document.Foldername;
            return path;
        }

        private void DownloadDocument()
        {
            var fileName = SelectedDocumentImage.Documentimage.Physicalname;
            var args = new EventParameterArgs<string>(fileName);
            OnRequestDownloadFileName(args);
            if (string.IsNullOrWhiteSpace(args.Parameter)) return;
            _fileBytesProvider.SaveFile(SelectedDocumentImage.Documentimage.Image, args.Parameter);
            OnDownloadCompleted(new EventParameterArgs<IEnumerable<string>>(args.Parameter.AsSingleElementCollection()));

        }

        public Contractdocdocumentimage SelectedDocumentImage { get; set; }

        private void DeleteDocument()
        {
            Repository.TryGetContext().Documentimages.DeleteOnSubmit(SelectedDocumentImage.Documentimage);
            DocumentImages.Remove(SelectedDocumentImage);
        }


        private readonly IFileBytesProvider _fileBytesProvider = new FileBytesProvider();

        private void AddDocument()
        {
            var args = new EventParameterArgs<string>(string.Empty);
            OnRequestFileName(args);
            if (string.IsNullOrWhiteSpace(args.Parameter)) return;
            var bytes = _fileBytesProvider.ReadFile(args.Parameter);

            var newImage = new Documentimage()
                {
                    Created = DateTime.Now,
                    Physicalname = Path.GetFileName(args.Parameter),
                    Name = Path.GetFileNameWithoutExtension(args.Parameter),
                    Image = bytes,
                    Modified = DateTime.Now,
                    Document =
                        Repository.TryGetContext().Documents.SingleOrDefault(
                            x => x.Id == EntityBase.ReservedUndifinedOid)
                };
            Repository.TryGetContext().Documentimages.InsertOnSubmit(newImage);
            var newContractdocImage = new Contractdocdocumentimage() { Documentimage = newImage };
            Repository.TryGetContext().Contractdocdocumentimages.InsertOnSubmit(newContractdocImage);
            
            DocumentImages.Add(newContractdocImage);
        }

        public DocumentImageViewModel(IContractRepository repository, ViewModelBase owner)
            : base(repository, owner)
        {
        }


        protected override void Save()
        {

        }

        protected override bool CanSave()
        {
            return true;
        }
    }

    internal interface IFileBytesProvider
    {
        byte[] ReadFile(string fileName);
        void SaveFile(byte[] image, string parameter);
    }

    class FileBytesProvider : IFileBytesProvider
    {
        public byte[] ReadFile(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        public void SaveFile(byte[] image, string parameter)
        {
           File.WriteAllBytes(parameter, image);
        }
    }
}
