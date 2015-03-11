using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using MContracts.Controls;
using UIShared.Commands;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Используется для определения базовых операций над коллекцией сущностей:
    /// - Формирование коллекции сущностей заданного типа
    /// - Команды создания, правки и удаления сущностей
    /// - Текущая сущность
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class BasicCommandsAggregatorViewModel<T> : ViewModelBase where T : class
    {
        /// <summary>
        /// Получает или устанавливает делегат выборки сущностей
        /// </summary>
        public Func<IEnumerable<T>> LoadEntities { get; set; }
        /// <summary>
        /// Получает или устанавливает делегат создания новой сущности
        /// </summary>
        public Func<T> CreateEntity { get; set; }
        /// <summary>
        /// Получает или устанавливает делегат изменения заданной сущности
        /// </summary>
        public Action<T> UpdateEntity { get; set; }
        /// <summary>
        /// Получает или устанавливает делегат удаления заданной сущности
        /// </summary>
        public Action<T> DeleteEntity { get; set; }

        private T _selected;

        /// <summary>
        /// Получает или устанавливает текущую сущность
        /// </summary>
        public T Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value) return;
                _selected = value;
                OnPropertyChanged(()=>Selected);
            }            
        }


        private ObservableCollection<T> _entities;

        /// <summary>
        /// Получает коллекцию сущностей
        /// </summary>
        public ObservableCollection<T> Entities
        {
            get
            {
                Contract.Requires(LoadEntities != null);
                Contract.Ensures(Contract.Result<ObservableCollection<T>>()!=null);
                var entities = LoadEntities();
                Contract.Assert(entities != null);
                return _entities ?? (_entities = new ObservableCollection<T>(entities));
            }
        }

        /// <summary>
        /// Получает команду создания новой сущности
        /// </summary>
        [ApplicationCommand("Создать", "")]
        public ICommand New
        {
            get
            {
                return new RelayCommand(_ => CreateObject());
            }
        }

        private void CreateObject()
        {
            // Запрос создания новой сущности
            var newT = CreateEntity();

            // Если сущность создана, то добавление в коллекцию
            if (newT != null)
                Entities.Add(newT);
        }

        [ApplicationCommand("Изменить", "")]
        public ICommand Update
        {
            get
            {
                return new RelayCommand(_ => UpdateEntity(Selected), _ => IsSelected());
            }
        }

    


        [ApplicationCommand("Удалить", "", AppCommandType.Confirm, "Удалить?", SeparatorType.Before)]
        public ICommand Delete
        {
            get
            {
                return new RelayCommand(_ => DeleteObject(Selected), _ => IsSelected());
            }
        }

        private void DeleteObject(T selected)
        {
            Entities.Remove(selected);
            DeleteEntity(selected);
        }

  
        private bool IsSelected()
        {
            return Selected != null;
        }

    }


}
