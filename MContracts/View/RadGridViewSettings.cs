using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using CommonBase;
using MContracts.Controls.Components;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace MContracts.View
{
    [Serializable]
    public class RadGridViewApplicationSettings : Dictionary<string, object>
    {
        private readonly DataContractSerializer _serializer;
        // private readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _serializer;

        public RadGridViewApplicationSettings()
        {
            //
        }

        public RadGridViewApplicationSettings(RadGridViewSettings settings)
        {
            var types = new List<Type>
                {
                    typeof (List<ColumnSetting>),
                    typeof (List<FilterSetting>),
                    typeof (List<GroupSetting>),
                    typeof (List<SortSetting>),
                    typeof (List<PropertySetting>),
                    typeof (long?)
                };

            _serializer = new DataContractSerializer(typeof (RadGridViewApplicationSettings), types);
            //_serializer = new BinaryFormatter();
            PersistID = settings.Grid.WithAssert(x => x.Name);
        }

        public string PersistID
        {
            get
            {
                if (!ContainsKey("PersistID"))
                {
                    this["PersistID"] = Guid.NewGuid().ToString();
                }
                return (string) this["PersistID"];
            }

            set { this["PersistID"] = value; }
        }

        public int FrozenColumnCount
        {
            get
            {
                if (!ContainsKey("FrozenColumnCount"))
                {
                    this["FrozenColumnCount"] = 0;
                }

                return (int) this["FrozenColumnCount"];
            }
            set { this["FrozenColumnCount"] = value; }
        }

        public List<ColumnSetting> ColumnSettings
        {
            get
            {
                if (!ContainsKey("ColumnSettings"))
                {
                    this["ColumnSettings"] = new List<ColumnSetting>();
                }

                return (List<ColumnSetting>) this["ColumnSettings"];
            }
        }

        public List<SortSetting> SortSettings
        {
            get
            {
                if (!ContainsKey("SortSettings"))
                {
                    this["SortSettings"] = new List<SortSetting>();
                }

                return (List<SortSetting>) this["SortSettings"];
            }
        }

        public List<GroupSetting> GroupSettings
        {
            get
            {
                if (!ContainsKey("GroupSettings"))
                {
                    this["GroupSettings"] = new List<GroupSetting>();
                }

                return (List<GroupSetting>) this["GroupSettings"];
            }
        }

        public List<FilterSetting> FilterSettings
        {
            get
            {
                if (!ContainsKey("FilterSettings"))
                {
                    this["FilterSettings"] = new List<FilterSetting>();
                }

                return (List<FilterSetting>) this["FilterSettings"];
            }
        }

        public long? SelectedObjectId
        {
            get
            {
                if (!ContainsKey("SelectedObjectId"))
                {
                    this["SelectedObjectId"] = default(long?);
                }

                return (long?) this["SelectedObjectId"];
            }
            set { this["SelectedObjectId"] = value; }
        }

        public void Reload()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    using (var stream = new IsolatedStorageFileStream(PersistID, FileMode.OpenOrCreate, file))
                    {
                        if (stream.Length <= 0) return;

                        var loaded = (RadGridViewApplicationSettings) _serializer.ReadObject(stream);

                        FrozenColumnCount = loaded.FrozenColumnCount;

                        ColumnSettings.Clear();
                        foreach (ColumnSetting cs in loaded.ColumnSettings)
                        {
                            ColumnSettings.Add(cs);
                        }

                        FilterSettings.Clear();
                        foreach (FilterSetting fs in loaded.FilterSettings)
                        {
                            FilterSettings.Add(fs);
                        }

                        GroupSettings.Clear();
                        foreach (GroupSetting gs in loaded.GroupSettings)
                        {
                            GroupSettings.Add(gs);
                        }

                        SortSettings.Clear();
                        foreach (SortSetting ss in loaded.SortSettings)
                        {
                            SortSettings.Add(ss);
                        }

                        SelectedObjectId = loaded.SelectedObjectId;
                    }
                }
            }
            catch (Exception e)
            {
                App.LogMessage(e.Message);
#if DEBUG
                // В режиме отладки показать исключение
                throw;
#endif
            }
        }

        public void Reset()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    file.DeleteFile(PersistID);
                }
            }
            catch (Exception e)
            {
                App.LogMessage(e.Message);
//#if DEBUG
//                    // В режиме отладки показать исключение
//                    throw;
//#endif
            }
        }

        public void Save()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    using (var stream = new IsolatedStorageFileStream(PersistID, FileMode.Create, file))
                    {
                        stream.Position = 0;
                        _serializer.WriteObject(stream, this);
                    }
                }
            }
            catch (Exception e)
            {
                App.LogMessage(e.Message);
#if DEBUG
                // В режиме отладки показать исключение
                throw;
#endif
            }
        }
    }

    public sealed class RadGridViewSettings
    {
        public static readonly DependencyProperty IsEnabledProperty
            = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (RadGridViewSettings),
                                                  new PropertyMetadata(OnIsEnabledPropertyChanged));

        private RadGridViewApplicationSettings _gridViewApplicationSettings;

        public RadGridViewSettings()
        {
            //
        }

        public RadGridViewSettings(PersistentRadGridView grid)
        {
            Grid = grid;
        }

        public PersistentRadGridView Grid { get; private set; }

        private RadGridViewApplicationSettings Settings
        {
            get
            {
                return _gridViewApplicationSettings ??
                       (_gridViewApplicationSettings = CreateRadGridViewApplicationSettingsInstance());
            }
        }

        public bool IsChanging { get; private set; }

        public static bool GetIsEnabled(DependencyObject dependencyObject)
        {
            return (bool) dependencyObject.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(IsEnabledProperty, enabled);
        }

        private static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject,
                                                       DependencyPropertyChangedEventArgs e)
        {
            var grid = dependencyObject as PersistentRadGridView;
            if (grid == null) return;
            if ((bool) e.NewValue)
            {
                var settings = new RadGridViewSettings(grid);
                settings.Attach();
            }
        }

        public void LoadState()
        {
            try
            {
                Settings.Reload();
            }
            catch
            {
                Settings.Reset();
            }

            if (Grid != null)
            {
                IsChanging = true;
                try
                {
                    Grid.FrozenColumnCount = Settings.FrozenColumnCount;

                    LoadColumnLayoutSettings();


                    using (Grid.DeferRefresh())
                    {
                        LoadSortSettings();

                        LoadGroupSettings();

                        LoadFilterSettings();
                    }
                    SetItemsSelection();
                }
                finally
                {
                    IsChanging = false;
                }
            }
        }

        private void SetItemsSelection()
        {
            var args = new EventParameterArgs<ObjectSelector>(new ObjectSelector(Settings.SelectedObjectId));
            OnSelectSpecifiedItems(args);
            object item = args.Parameter.Item;
            item.Do(x => Grid.SelectedItem = x);
            item.Do(x => Grid.CurrentItem = x);
            item.Do(x => Grid.ScrollIntoView(x));
        }

        private void LoadFilterSettings()
        {
            if (Settings.FilterSettings.Count > 0)
            {
                foreach (FilterSetting setting in Settings.FilterSettings)
                {
                    var currentSetting = setting;

                    var matchingColumn =
                        (from column in Grid.Columns.OfType<GridViewDataColumn>()
                         where
                             column.DataMemberBinding != null &&
                             column.DataMemberBinding.Path.Path == currentSetting.PropertyName
                         select column).FirstOrDefault();

                    if (matchingColumn != null)
                    {
                        LambdaColumnFilterDescriptor cfd = new LambdaColumnFilterDescriptor(matchingColumn);
                       // cfd.LambdaExpression = 
                        
                        

                        /*if (setting.Filter1 != null && setting.Filter1.Member != null)
                        {
                            cfd. = setting.Filter1.Member;
                            //cfd.FieldFilter.Filter1 = setting.Filter1.Member;
                            cfd.FieldFilter.Filter1.Operator = setting.Filter1.Operator;
                            cfd.FieldFilter.Filter1.Value = setting.Filter1.Value;
                        }

                        if (setting.Filter2 != null && setting.Filter2.Member != null)
                        {
                            cfd.FieldFilter.Filter2.Member = setting.Filter2.Member;
                            cfd.FieldFilter.Filter2.Operator = setting.Filter2.Operator;
                            cfd.FieldFilter.Filter2.Value = setting.Filter2.Value;
                        }

                        foreach (FilterDescriptorSetting fds in setting.SelectedDistinctValues)
                        {
                            var fd = new FilterDescriptor
                                {Member = fds.Member, Operator = fds.Operator, Value = fds.Value};
                            cfd.DistinctFilter.FilterDescriptors.Add(fd);
                        }

                        Grid.FilterDescriptors.Add(cfd);
                        */
                    }
                }
            }
        }

        private void LoadGroupSettings()
        {
            if (Settings.GroupSettings.Count <= 0) return;
            Grid.GroupDescriptors.Clear();

            foreach (ColumnGroupDescriptor d in Settings.GroupSettings.Select(setting => new ColumnGroupDescriptor
                {
                    Column = (from c in Grid.Columns.OfType<GridViewBoundColumnBase>()
                              where c.GetDataMemberName() == setting.PropertyName
                              select c).FirstOrDefault(),
                    SortDirection = setting.SortDirection
                }).Where(d => d.Column != null))
            {
                Grid.GroupDescriptors.Add(d);
            }
        }

        private void LoadSortSettings()
        {
            if (Settings.SortSettings.Count > 0)
            {
                Grid.SortDescriptors.Clear();

                foreach (SortSetting setting in Settings.SortSettings)
                {
                    var d = new ColumnSortDescriptor
                        {
                            Column = (from c in Grid.Columns.OfType<GridViewBoundColumnBase>()
                                      where c.GetDataMemberName() == setting.PropertyName
                                      select c).FirstOrDefault(),
                            SortDirection = setting.SortDirection
                        };
                    if (d.Column != null)
                        Grid.SortDescriptors.Add(d);
                }
            }
        }

        private void LoadColumnLayoutSettings()
        {
            if (Settings.ColumnSettings.Count > 0)
            {
                foreach (ColumnSetting setting in Settings.ColumnSettings)
                {
                    ColumnSetting currentSetting = setting;

                    GridViewDataColumn column = (from c in Grid.Columns.OfType<GridViewDataColumn>()
                                                 where c.UniqueName == currentSetting.UniqueName
                                                 select c).FirstOrDefault();

                    if (column == null) continue;
                    if (currentSetting.DisplayIndex != null)
                    {
                        column.DisplayIndex = currentSetting.DisplayIndex.Value;
                    }

                    if (setting.Width != null)
                    {
                        column.Width = new GridViewLength(setting.Width.Value);
                    }
                }
            }
        }

        public void ResetState()
        {
            Settings.Reset();
        }

        private FilterDescriptorSetting CreateFilterDescriptor(FilterDescriptor descriptor)
        {
            return new FilterDescriptorSetting
                {
                    Member = descriptor.Member,
                    Operator = descriptor.Operator,
                    Value = descriptor.Value
                };
        }

        public void SaveState(bool saveFilters = true, bool saveGroups = true, bool saveSort = true)
        {
            Settings.Reset();
            if (Grid != null)
            {
                SaveSelection();
                if (Grid.IsLoaded)

                    if (Grid.Columns != null)
                    {
                        Settings.ColumnSettings.Clear();

                        foreach (GridViewColumn column in Grid.Columns)
                        {
                            var dataColumn = column as GridViewDataColumn;
                            if (dataColumn == null) continue;
                            if (dataColumn.DataMemberBinding == null) continue;
                            var setting = new ColumnSetting
                                {
                                    PropertyName = dataColumn.DataMemberBinding.Path.Path,
                                    UniqueName = dataColumn.UniqueName,
                                    Header = dataColumn.Header,
                                    Width = dataColumn.ActualWidth,
                                    DisplayIndex = dataColumn.DisplayIndex
                                };

                            Settings.ColumnSettings.Add(setting);
                        }
                    }
                if (saveFilters)
                {
                
                //if (Grid.FilterDescriptors != null)
                    //{
                    //    Settings.FilterSettings.Clear();

                    //    foreach (IColumnFilterDescriptor cfd in Grid.FilterDescriptors.OfType<IColumnFilterDescriptor>()
                    //        )
                    //    {
                    //        var setting = new FilterSetting();

                    //        if (cfd.FieldFilter.Filter1.Value != FilterDescriptor.UnsetValue)
                    //        {
                    //            //setting.Filter1 = CreateFilterDescriptor(cfd.FieldFilter.Filter1);
                    //        }

                    //        if (cfd.FieldFilter.Filter2.Value != FilterDescriptor.UnsetValue)
                    //        {
                    //            //setting.Filter2 = CreateFilterDescriptor(cfd.FieldFilter.Filter2);
                    //        }

                    //        foreach (
                    //            FilterDescriptor fd in cfd.DistinctFilter.FilterDescriptors.OfType<FilterDescriptor>())
                    //        {
                    //            if (fd.Value == FilterDescriptor.UnsetValue)
                    //            {
                    //                continue;
                    //            }

                    //            var fds = new FilterDescriptorSetting
                    //                {Member = fd.Member, Operator = fd.Operator, Value = fd.Value};

                    //            setting.SelectedDistinctValues.Add(fds);
                    //        }

                    //        //setting.PropertyName = cfd.Column.DataMemberBinding.Path.Path;

                    //        Settings.FilterSettings.Add(setting);
                    //    }
                    }
                if (saveSort)
                    if (Grid.SortDescriptors != null)
                    {
                        Settings.SortSettings.Clear();

                        foreach (
                            SortSetting setting in
                                Grid.SortDescriptors.OfType<ColumnSortDescriptor>().Select(d => new SortSetting
                                    {
                                        PropertyName = ((GridViewDataColumn) d.Column).GetDataMemberName(),
                                        SortDirection = d.SortDirection
                                    }))
                        {
                            Settings.SortSettings.Add(setting);
                        }
                    }

                if (saveGroups)
                    if (Grid.GroupDescriptors != null)
                    {
                        Settings.GroupSettings.Clear();

                        foreach (
                            GroupSetting setting in
                                Grid.GroupDescriptors.OfType<ColumnGroupDescriptor>().Select(d => new GroupSetting
                                    {
                                        PropertyName = ((GridViewDataColumn) d.Column).GetDataMemberName(),
                                        SortDirection = d.SortDirection
                                    }))
                        {
                            Settings.GroupSettings.Add(setting);
                        }
                    }

                Settings.FrozenColumnCount = Grid.FrozenColumnCount;
            }

            Settings.Save();
        }

        private void SaveSelection()
        {
            // Если запрос выделенных объектов был удачном, то свойство Parameter должно содержать коллекцию их идентификаторов
            var args = new EventParameterArgs<ObjectIdentifierSelector>(new ObjectIdentifierSelector(Grid.LastSelected));
            OnSelectionIdsRequire(args);
            Settings.SelectedObjectId = args.Parameter.Id;
        }

        /// <summary>
        ///     Событие возникает при необходимости сохранить идентификаторы выбранных в таблице строк для последующего восстановления
        /// </summary>
        public event EventHandler<EventParameterArgs<ObjectIdentifierSelector>> SelectionIdsRequire;

        /// <summary>
        ///     Событие возникает при необходимости восстановить выделенные строки по сохранённым идентификаторам
        /// </summary>
        public event EventHandler<EventParameterArgs<ObjectSelector>> SelectSpecifiedItems;

        private void OnSelectSpecifiedItems(EventParameterArgs<ObjectSelector> e)
        {
            EventHandler<EventParameterArgs<ObjectSelector>> handler = SelectSpecifiedItems;
            if (handler != null) handler(this, e);
        }


        private void OnSelectionIdsRequire(EventParameterArgs<ObjectIdentifierSelector> e)
        {
            EventHandler<EventParameterArgs<ObjectIdentifierSelector>> handler = SelectionIdsRequire;
            if (handler != null) handler(this, e);
        }

        private void Attach()
        {
            if (Grid == null) return;
            Grid.LayoutUpdated += LayoutUpdated;
            Grid.Loaded += Loaded;
            Application.Current.Exit += CurrentExit;
        }

        private void CurrentExit(object sender, EventArgs e)
        {
            SaveState();
        }

        private void Loaded(object sender, EventArgs e)
        {
            LoadState();
        }

        private void LayoutUpdated(object sender, EventArgs e)
        {
            if (Grid.Parent == null)
            {
                SaveState();
            }
        }

        private RadGridViewApplicationSettings CreateRadGridViewApplicationSettingsInstance()
        {
            return new RadGridViewApplicationSettings(this);
        }
    }

    [Serializable]
    public class PropertySetting
    {
        public string PropertyName { get; set; }
    }

    [Serializable]
    public class SortSetting : PropertySetting
    {
        public ListSortDirection SortDirection { get; set; }
    }

    [Serializable]
    public class GroupSetting : PropertySetting
    {
        public ListSortDirection? SortDirection { get; set; }
    }

    [Serializable]
    public class FilterSetting : PropertySetting
    {
        private List<FilterDescriptorSetting> _selectedDistinctValues;

        public List<FilterDescriptorSetting> SelectedDistinctValues
        {
            get { return _selectedDistinctValues ?? (_selectedDistinctValues = new List<FilterDescriptorSetting>()); }
            set { throw new NotImplementedException(); }
        }

        public FilterDescriptorSetting Filter1 { get; set; }

        public FilterDescriptorSetting Filter2 { get; set; }
    }

    [Serializable]
    public class FilterDescriptorSetting
    {
        public string Member { get; set; }

        public FilterOperator Operator { get; set; }

        public object Value { get; set; }

        public bool IsCaseSensitive { get; set; }
    }

    [Serializable]
    public class ColumnSetting : PropertySetting
    {
        public string UniqueName { get; set; }

        public object Header { get; set; }

        public double? Width { get; set; }

        public int? DisplayIndex { get; set; }
    }
}