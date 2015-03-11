using System;
using System.Collections.Generic;
using MCDomain.DataAccess;
using System.Linq;
using McUIBase.ViewModel;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Составная модель представления рабочей области. Включает свойство Childs для управления вложенными 
    /// моделями представления и переопределяет методы Save и CanSave для реализации сохранения данных 
    /// вложенных моделей
    /// </summary>
    public class CompositeWorkspaceViewModel : WorkspaceViewModel
    {
        /// <summary>
        /// Словарь дочерних моделей представления
        /// </summary>
        protected readonly IDictionary<string, ViewModelBase> Childs = new Dictionary<string, ViewModelBase>();


        protected CompositeWorkspaceViewModel(IContractRepository repository) : base(repository)
        {

        }

  

        #region Overrides of RepositoryViewModel

        /// <summary>
        /// Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {

            var childsRepository = Childs.Values.OfType<RepositoryViewModel>();
            foreach (var repositoryViewModel in childsRepository)
            {
                if (repositoryViewModel.SaveCommand.CanExecute(null))
                    repositoryViewModel.SaveCommand.Execute(null);
            }                 
           
        }

        /// <summary>
        /// Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            var childsRepository = Childs.Values.OfType<RepositoryViewModel>();
            return childsRepository.All(x => x.SaveCommand.CanExecute(null));
        }

        /// <summary>
        /// Получает признак того, что данные модели представления были изменены
        /// </summary>
        public override bool IsModified
        {
            get
            {
                var childsRepository = Childs.Values.OfType<RepositoryViewModel>();
                return base.IsModified || childsRepository.Any(x=>x.IsModified);
            }
        }

        #endregion

        public override bool IsClosable
        {
            get { return true; }
        }
    }
}