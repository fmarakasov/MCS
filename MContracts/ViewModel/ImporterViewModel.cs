using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MContracts.Classes;
using System.Windows.Input;
using MCDomain.Importer;
using Microsoft.Win32;
using CommonBase;
using System.Windows.Data;
using System.Globalization;
using MCDomain.DataAccess;
using System.Collections.ObjectModel;
using MCDomain.Model;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using MContracts.Controls.Dialogs;
using CommonBase;

namespace MContracts.ViewModel
{


    #region Converters
    /// <summary>
    /// тип файла (Excel) в видимость (невидимость) панелей
    /// </summary>
    public class ExcelReaderTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if  (value is ExcelReader)
            {
              return System.Windows.Visibility.Visible;
            }
            else 
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    /// <summary>
    /// тип файла (Word) в видимость (невидимость) панелей
    /// </summary>
    public class WordReaderTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is WordReader)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    /// <summary>
    /// тип файла (еще не выбран) в видимость (невидимость) панелей
    /// </summary>
    public class NoReaderToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Hidden;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// тип файла (выбран любой) в видимость (невидимость) панелей
    /// </summary>
    public class AnyReaderToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Hidden;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    /// <summary>
    /// номер листа или таблицы в индекс выбранного элемента в комбобоксе листа или таблицы
    /// </summary>
    public class SheetIndexToListIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value + 1;
        }
    }
    /// <summary>
    /// значение параметра сохранения (сохранить всё) в value radio button'а
    /// </summary>
    public class SaveAllSavingSettingToRadioBtnCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ImporterSavingSetting)value == ImporterSavingSetting.SaveAll) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    /// <summary>
    /// значение параметра сохранения (сохранить только новые) в value radio button'а
    /// </summary>
    public class SaveNewSavingSettingToRadioBtnCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ImporterSavingSetting)value == ImporterSavingSetting.SaveNew) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    #endregion
    
    #region HelperClasses
    /// <summary>
    /// параметры, установленные для таблицы, отображающей прочитанные из источника данные
    /// </summary>
    public class GridSettingsChangedEventArgs: EventArgs
    {
        public int ColCount;
        public int RowCount;
        public Cells Cells;
        public GridSettingsChangedEventArgs(int colcount, int rowcount, Cells cells)
        {
            ColCount = colcount;
            RowCount = rowcount;
            Cells = cells;
        }
    }

    public class ImporterViewModelStateChangedEventArgs : EventArgs
    {
        public ImporterState OldState;
        public ImporterViewModelStateChangedEventArgs(ImporterState _oldstate)
        {
            OldState = _oldstate;
        }

    }
    /// <summary>
    /// возможные параметры сохранения
    /// </summary>
    public enum ImporterSavingSetting 
    {
        /// <summary>
        /// сохранить всё - будет импортировано все, закрытые этапы будут обойдены в соответствии с ТЗ
        /// </summary>
        SaveAll,
        /// <summary>
        /// будут импортированы и сохранены только новые этапы
        /// </summary>
        SaveNew
    }




    #endregion

    public class ImporterViewModel : BaseImporterViewModel
    {
        #region Constructor
        public ImporterViewModel(IContractRepository repository) : base(repository)
        {
           

        }
        #endregion

        #region Properties;



        /// <summary>
        /// генеральный договор
        /// </summary>
        public Contractdoc GeneralContractdoc
        {
            get { return (Importer as Importer).Generalcontract; }
            set
            {
                if ((Importer as Importer).Generalcontract == value) return;
                (Importer as Importer).Generalcontract = value;
            }

        }


                /// <summary>
        /// генеральный договор
        /// </summary>
        public Contractdoc Originalcontractdoc
        {
            get { return (Importer as Importer).Originalcontract; }
            set
            {
                if ((Importer as Importer).Originalcontract == value) return;
                (Importer as Importer).Originalcontract = value;
            }
        }


        public IList<Schedulecontract> Schedulecontracts
        {
            get
            {
                if (GeneralContractdoc != null) return GeneralContractdoc.Schedulecontracts;
                else
                {
                    if (Originalcontractdoc != null) return Originalcontractdoc.Schedulecontracts;
                }

                return null;
            }

        }


        public Schedulecontract Mainschedulecontract
        {
            
            get
            {
                return (Importer as Importer).Originalschedulecontract ?? ((Importer as Importer).Originalschedulecontract = Schedulecontracts.FirstOrDefault(
                                                                            x =>
                                                                            x.Schedule != null && CurrentSchedule.Schedule != null &&
                                                                            x.Schedule.Worktypeid == CurrentSchedule.Schedule.Worktypeid &&
                                                                            x.Appnum == CurrentSchedule.Appnum));
            }
            set 
            { 
                (Importer as Importer).Originalschedulecontract = value;
                OnPropertyChanged(()=> Mainschedulecontract);
            }
        }


        #endregion

        #region Methods

        protected override BaseImporter CreateImporter()
        {
            return new Importer(Repository, CurrentSchedule.Schedule, CurrentSchedule.Contractdoc);
        }
        
        /// <summary>
        /// наполнить возращаемый список этапов _outputstagesbindinglist
        /// </summary>
        /// <param name="s"></param>
        protected override void FillStagesList(IList<Stage> outputstageslist, ImportedStage s, Stage parentstage)
        {

            if (!outputstageslist.Any(x => (x.Num == s.Num)))
            {
                Stage rs = new Stage()
                               {

                                   Num = s.Num,
                                   Price = s.Price,
                                   Startsat = s.Startsat,
                                   Endsat = s.Endsat,
                                   Code = s.Code,
                                   Subject = s.Subject
                               };

                if (parentstage != null)
                {
                    rs.ParentStage = parentstage;
                }
                Repository.DebugPrintRepository();
                rs.Schedule = CurrentSchedule.Schedule;
                Repository.DebugPrintRepository();


                if (CurrentConrtactdoc.OriginalContract != null)
                {
                    rs.ClosedGeneralStage = s.OriginContractStage;
                }

                if (CurrentConrtactdoc.General != null) 
                {
                    rs.GeneralStage = s.GeneralContractStage;
                }

                Repository.DebugPrintRepository();

                rs.Ndsalgorithm = (Importer as Importer).DefaultNdsalgorithm;
                rs.Nds = (Importer as Importer).DefaultNds;

                

                Ntpsubview ntpsbvw;
                foreach (ImportedStageResult ir in s.Stageresults)
                {
                    if ((ir.Ntpsubviewid != 0)&&(!ir.UsedDefaultNTPSubView)) ntpsbvw = Repository.Ntpsubviews.FirstOrDefault(p => (p.Id == ir.Ntpsubviewid));
                    else ntpsbvw = (Importer as Importer).DefaultNTPSubview;

                    rs.Stageresults.Add (new Stageresult() { Stage = rs, 
                                                             Name = ir.Name,
                                                             Ntpsubview = ntpsbvw});
                }
         
                outputstageslist.Add(rs);
                rs.Num = s.Num;

                foreach (ImportedStage isc in s.Stages)
                {
                    FillStagesList(outputstageslist, isc, rs);
                }
                rs.CheckChildParentProperties();
            }
            else
            {
                Stage ss = outputstageslist.First(x => (x.Num == s.Num));
                // для имеющихся этапов может измениться название и код стройки
                if (ss.Stagecondition == StageCondition.Closed)
                {
                    if ((ss.Subject != s.Subject) || (ss.Code != s.Code))
                    {
                        ss.Code = s.Code;
                        ss.Subject = s.Subject;
                        
                    }

                    if (CurrentConrtactdoc.OriginalContract != null)
                    {
                        ss.ClosedGeneralStage = s.OriginContractStage;
                    }

                    if (CurrentConrtactdoc.General != null)
                    {
                        ss.GeneralStage = s.GeneralContractStage; 
                    }
                }
                else
                {
                    ss.Schedule = CurrentSchedule.Schedule;
                    ss.Num = s.Num;
                    ss.Price = s.Price;
                    ss.Startsat = s.Startsat;
                    ss.Endsat = s.Endsat;
                    ss.Code = s.Code;
                    ss.Subject = s.Subject;
                    
                    if (CurrentConrtactdoc.OriginalContract != null)
                    {
                        ss.ClosedGeneralStage = s.OriginContractStage;
                    }

                    if (CurrentConrtactdoc.General != null)
                    {
                        ss.GeneralStage = s.GeneralContractStage;
                    }

                    ss.Ndsalgorithm = (Importer as Importer).DefaultNdsalgorithm;
                    ss.Nds = (Importer as Importer).DefaultNds;

                    // чистим старые результаты
                    var srlst = ss.Stageresults.GetNewBindingList();
                    Stageresult sr;
                    for (int i = srlst.Count - 1; i >= 0; i--)
                    {
                        sr = srlst[i] as Stageresult;
                        Repository.DeleteStateResult(sr);
                    }
                    srlst.Clear();
                    ss.Stageresults.Clear();
                    // наполняем новыми
                    Ntpsubview ntpsbvw;
                    foreach (ImportedStageResult ir in s.Stageresults)
                    {

                        if (ir.Ntpsubviewid != 0) ntpsbvw = Repository.Ntpsubviews.FirstOrDefault(p => (p.Id == ir.Ntpsubviewid));
                        else ntpsbvw = (Importer as Importer).DefaultNTPSubview;

                        ss.Stageresults.Add(new Stageresult()
                        {
                            Stage = ss,
                            Name = ir.Name,
                            Ntpsubview = ntpsbvw
                        });
                    }
         
                }

                foreach (ImportedStage isc in s.Stages)
                {
                    FillStagesList(outputstageslist, isc, ss);
                }
                ss.CheckChildParentProperties();

            }

        }
    
        /// <summary>
        /// сохранить результаты импорта
        /// </summary>
        protected override void InternalSaveResults()
        {
            if (InputStageBindingList != null)
            {

                if (SavingSetting == ImporterSavingSetting.SaveAll)
                {
                    DataContextDebug.DebugPrintRepository(Repository);
                    OutputStageBindingList.Clear();

                    
                    // добавляем закрытые этапы, которые были в ScheduleViewModel
                    // остальные - удаляем
                    Stage s1;


                    for (int i = InputStageBindingList.Count - 1; i >= 0; i--)
                    {
                        s1 = InputStageBindingList[i] as Stage;
                        if (s1.Stagecondition == StageCondition.Closed || s1.Stages.Any(ss => ss.Stagecondition == StageCondition.Closed))
                        {
                            OutputStageBindingList.Insert(0, s1);
                        }
                        else
                        {
                            if (DeleteStageCommand != null) DeleteStageCommand.Execute(s1);
                        }
                    }

                   
                    foreach (ImportedStage s in Importer.Stages)
                    {
                        FillStagesList(OutputStageBindingList, s, FindParentStage(OutputStageBindingList, s));
                    }
                    DataContextDebug.DebugPrintRepository(Repository);
                    var ctx = Repository.TryGetContext();
                    ctx.Log = Console.Out;
                }
                else if (SavingSetting == ImporterSavingSetting.SaveNew)
                {

                    OutputStageBindingList.Clear();
     

                    // добавляем этапы, которые были в ScheduleViewModel
                    foreach (Stage s1 in InputStageBindingList)
                    {
                        OutputStageBindingList.Add(s1);
                    }

                    foreach (ImportedStage s in Importer.Stages)
                    {
                        FillStagesList(OutputStageBindingList, s, FindParentStage(OutputStageBindingList, s));
                    }
                }

                NeedSave = true;
            }

            CloseCommand.Execute(null);
        }

        private Stage FindParentStage(ObservableCollection<Stage> outputstageslist, ImportedStage s)
        {
            HierarchicalNumberingComparier comp = new HierarchicalNumberingComparier();
            string[] stagenum = comp.Matches(s.Num);
            Stage lastfoundparent = null;

            if (stagenum.Length > 1)
            {
                for (int i = 0; i < stagenum.Length - 1; i++)
                {
                    Stage foundparent = outputstageslist.FirstOrDefault(x => x.Num == stagenum[i].Trim());

                    if (foundparent == null)
                    {
                        foundparent = new Stage() {Num = stagenum[i]};
                        foundparent.Ndsalgorithm = (Importer as Importer).DefaultNdsalgorithm;
                        foundparent.Nds = (Importer as Importer).DefaultNds;
                        foundparent.Schedule = CurrentSchedule.Schedule;
                        foundparent.ParentStage = lastfoundparent;
                        outputstageslist.Add(foundparent);
                    }
                    lastfoundparent = foundparent;
                }
            }
            return lastfoundparent;
        }


  
        #endregion


    }

   
}
