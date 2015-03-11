using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Classes.Converters;
using MContracts.Controls.Dialogs;
using MContracts.Dialogs;
using MContracts.ViewModel;
using System.ComponentModel;
using Telerik.Windows.Controls.GridView;


namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for CatalogView.xaml
    /// </summary>
    public partial class CatalogView : UserControl
    {
        private CatalogViewModel ViewModel = null;

        public CatalogView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(CatalogView_DataContextChanged);
        }

        void CatalogView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel = this.DataContext as CatalogViewModel;

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
                dataGrid.CanUserSortColumns = !(ViewModel is HierarchicalCatalogViewModel);
                ViewModel.ClearEvents();
                //ViewModel.CatalogTypeChanged += new CatalogViewModel.CatalogTypeEventHandler(ViewModel_CatalogTypeChanged);
                //ViewModel.EditWindowShowed += new CatalogViewModel.EditWindowShowedEventHandler(ViewModel_EditWindowShowed);
                ViewModel.CatalogTypeChanged += ViewModel_CatalogTypeChanged;
                ViewModel.EditWindowShowed += ViewModel_EditWindowShowed;

                ViewModel.LoadData();
            }

        }

        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CollectionView"&&ViewModel!=null)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = ViewModel.CollectionView;
                
            }
        }

        void ViewModel_EditWindowShowed(object sender, EditWindowShowedEventsArgs e)
        {
            if (sender is CatalogViewModel)
            {
                foreach (UIElement ctrl in this.LayoutRoot.Children)
                {
                    ctrl.Opacity = 0.5;
                }
               


                EditCatalogItemDialog dialog = new EditCatalogItemDialog((sender as CatalogViewModel).Repository);
                dialog.Closed += new EventHandler(dialog_Closed);
                dialog.CatalogType = e.CatalogType;
                dialog.ViewModel.ActionType = e.ActionType;

                if (e.ActionType == ActionType.Edit)
                {
                    dialog.ViewModel.ObjectForEdit = (sender as CatalogViewModel).SelectedItem;
                    //(dialog.ViewModel.ObjectForEdit as IEditableObject).BeginEdit();
                }
                else
                {
                    //var type = dataGrid.ItemsSource.GetType().GetGenericArguments().FirstOrDefault();
                    dialog.ViewModel.ObjectForEdit = Activator.CreateInstance(ViewModel.CurrentType);
                    switch (e.CatalogType)
                    {

                        case CatalogType.Contractor:
                            {
                                (dialog.ViewModel.ObjectForEdit as Contractor).Contractortype = ViewModel.Repository.Contractortypes.GetReservedUndefined();
                                break;
                            };
                        case CatalogType.Person:
                            {
                                (dialog.ViewModel.ObjectForEdit as Person).Contractorposition = ViewModel.Repository.Contractorpositions.GetReservedUndefined();
                                (dialog.ViewModel.ObjectForEdit as Person).Degree = ViewModel.Repository.Degrees.GetReservedUndefined();
                                break;
                            }

                        case CatalogType.Department:
                           {
                               (dialog.ViewModel.ObjectForEdit as Department).Manager = ViewModel.Repository.Employees.GetReservedUndefined();
                               (dialog.ViewModel.ObjectForEdit as Department).Director = ViewModel.Repository.Employees.GetReservedUndefined();
                               break;
                           };
                        case CatalogType.Employee:
                            {
                                // здесь должна вводиться должность
                                break;
                            }
                    }


                }
                //if (ViewModel == null) ViewModel = sender as CatalogViewModel;
                dialog.ShowDialog();
            }
        }



        void dialog_Closed(object sender, EventArgs e)
        {
            if ((sender as EditCatalogItemDialog).DialogResult == true)
            {
                if ((sender as EditCatalogItemDialog).ViewModel.ActionType == ActionType.Edit)
                {
                    ViewModel.SelectedItem = (sender as EditCatalogItemDialog).ViewModel.ObjectForEdit;
                    ViewModel.IsModifiedChanged();
                }
                if ((sender as EditCatalogItemDialog).ViewModel.ActionType == ActionType.Add)
                {
                    ViewModel.AddObject((sender as EditCatalogItemDialog).ViewModel.ObjectForEdit);
                }
            }
            else
            {
                 ViewModel.Repository.RejectChanges();
                 ViewModel.IsObjectsChanged();
            }

            foreach (UIElement ctrl in LayoutRoot.Children)
            {
                ctrl.Opacity = 1;
            }

            if (row != null)
            {
                var color = row.DataContext as IColor;
                if (color != null)
                    row.Background = GetSolidBrushForRow(color.Color);
            }
        }

        void ViewModel_CatalogTypeChanged(object sender, CatalogTypeEventsArgs e)
        {
            if (ViewModel != null)
            {
                SetTemplate(e.CatalogType);
                dataGrid.ItemsSource = ViewModel.CollectionView;
            }
        }

        private void SetTemplate(CatalogType catalogType)
        {
            dataGrid.Columns.Clear();
            foreach (var column in this.Resources[catalogType.ToString()] as TelerikColumnCollection)
            {
                dataGrid.Columns.Add(column);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            collectionView.Filter = (item) =>
                                        {
                                            return ViewModel.FilterItem(item, (sender as TextBox).Text);
                                        };
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.EditCommand.CanExecute(null)) 
                ViewModel.EditCommand.Execute(null);
        }

        private void dataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                if (ViewModel.EditCommand.CanExecute(null))
                    ViewModel.EditCommand.Execute(null);
            }
        }

        private SolidColorBrush GetSolidBrushForRow(long? dbcolor)
        {
            int nval;
            if (int.TryParse(dbcolor.ToString(), out nval))
            {
                var bytes = BitConverter.GetBytes(nval);
                var color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
                return new SolidColorBrush(color);
            }
            else
            {
                return  new SolidColorBrush(new Color(){R = 255, G = 255, B = 255});
            }
        }

        private void DataGrid_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.Row.DataContext is IColor)
            {
               e.Row.Background = GetSolidBrushForRow((e.Row.DataContext as IColor).Color);
            }
        }

        private GridViewRowItem row = null;

        private void dataGrid_RowActivated(object sender, RowEventArgs e)
        {

            if (e.Row.DataContext is IColor)
            {
                e.Row.Background = GetSolidBrushForRow((e.Row.DataContext as IColor).Color);
                row = e.Row;
                row.IsSelected = true;
            }
        }


    }
}
