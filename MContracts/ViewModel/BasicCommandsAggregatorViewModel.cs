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
    /// ������������ ��� ����������� ������� �������� ��� ���������� ���������:
    /// - ������������ ��������� ��������� ��������� ����
    /// - ������� ��������, ������ � �������� ���������
    /// - ������� ��������
    /// </summary>
    /// <typeparam name="T">��� ��������</typeparam>
    public class BasicCommandsAggregatorViewModel<T> : ViewModelBase where T : class
    {
        /// <summary>
        /// �������� ��� ������������� ������� ������� ���������
        /// </summary>
        public Func<IEnumerable<T>> LoadEntities { get; set; }
        /// <summary>
        /// �������� ��� ������������� ������� �������� ����� ��������
        /// </summary>
        public Func<T> CreateEntity { get; set; }
        /// <summary>
        /// �������� ��� ������������� ������� ��������� �������� ��������
        /// </summary>
        public Action<T> UpdateEntity { get; set; }
        /// <summary>
        /// �������� ��� ������������� ������� �������� �������� ��������
        /// </summary>
        public Action<T> DeleteEntity { get; set; }

        private T _selected;

        /// <summary>
        /// �������� ��� ������������� ������� ��������
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
        /// �������� ��������� ���������
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
        /// �������� ������� �������� ����� ��������
        /// </summary>
        [ApplicationCommand("�������", "")]
        public ICommand New
        {
            get
            {
                return new RelayCommand(_ => CreateObject());
            }
        }

        private void CreateObject()
        {
            // ������ �������� ����� ��������
            var newT = CreateEntity();

            // ���� �������� �������, �� ���������� � ���������
            if (newT != null)
                Entities.Add(newT);
        }

        [ApplicationCommand("��������", "")]
        public ICommand Update
        {
            get
            {
                return new RelayCommand(_ => UpdateEntity(Selected), _ => IsSelected());
            }
        }

    


        [ApplicationCommand("�������", "", AppCommandType.Confirm, "�������?", SeparatorType.Before)]
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
