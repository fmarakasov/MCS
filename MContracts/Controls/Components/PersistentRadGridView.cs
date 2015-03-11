using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MContracts.View;
using Telerik.Windows.Controls;
using CommonBase;

namespace MContracts.Controls.Components
{
    /// <summary>
    /// Компонент RadGridView с поддержкой автоматического сохранения состояния
    /// </summary>
    public class PersistentRadGridView : RadGridView
    {
        private RadGridViewSettings _settings;
        
        /// <summary>
        /// Получает экземпляр  RadGridViewSettings, связанный с данным компонентом для управления сохранением состояния
        /// </summary>
        public RadGridViewSettings Settings
        {
            get { return _settings; }
        }

        internal object LastSelected
        {
            get { return _lastSelected; }
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Автоматическое управление состоянием основано на обработке изменений в коллекциях дескрипторов сортировки
            // группировки и фильтрации
            GroupDescriptors.CollectionChanged +=
            DataGridDescriptorsCollectionChanged;
            FilterDescriptors.CollectionChanged +=
                DataGridDescriptorsCollectionChanged;
            SortDescriptors.CollectionChanged +=
                DataGridDescriptorsCollectionChanged;
            Loaded += PersistentRadGridViewLoaded;
            Unloaded += PersistentRadGridViewUnloaded;
            SelectionChanged += PersistentRadGridView_SelectionChanged;
            
            _settings = new RadGridViewSettings(this);

            base.OnInitialized(e);
        }

        void PersistentRadGridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
             if (IsLoaded)
                _lastSelected = SelectedItem;

        }

        private object _lastSelected;

        



        void PersistentRadGridViewUnloaded(object sender, RoutedEventArgs e)
        {
            SaveState(false,false,false);
        }

        void PersistentRadGridViewLoaded(object sender, RoutedEventArgs e)
        {
            Settings.LoadState();
        }

       

        private void DataGridDescriptorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Обработка уведомлений об изменениях в свойствах дескрипторов
            if (e.Action == NotifyCollectionChangedAction.Add)
                e.NewItems.Cast<INotifyPropertyChanged>().Apply(x => x.PropertyChanged += DataGridDescriptorPropertyChanged);
            if (e.Action == NotifyCollectionChangedAction.Remove)
                e.OldItems.Cast<INotifyPropertyChanged>().Apply(x => x.PropertyChanged -= DataGridDescriptorPropertyChanged);

            // Если происходит сброс или изменение коллекции инициировано загрузкой сотстояния, то не требуется 
            // выполнять сохранение (разрыв рекурсии)
            if (e.Action == NotifyCollectionChangedAction.Reset || Settings.IsChanging) return;

            SaveState(true, true, true);
        }

        private void SaveState(bool saveFilterDesc, bool saveGroupDesc, bool saveSort)
        {
            if (Settings != null)
                Settings.SaveState(saveFilterDesc, saveGroupDesc, saveSort);
        }

        private void DataGridDescriptorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveState(true, true, true);
        }
    }
}
