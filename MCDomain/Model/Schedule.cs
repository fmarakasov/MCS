using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using MCDomain.DataAccess;
using CommonBase;

namespace MCDomain.Model
{
    partial class Schedule : IObjectId, IDataErrorInfo, ICloneable, INull, IClonableRecursive
    {
        

        private readonly DataErrorHandlers<Schedule> _errorHandlers = new DataErrorHandlers<Schedule>
                                                                       {
                                                                           new WorktypeErrorHandler(),
                                                                           new CurrencymeasureErrorHandler()
                                                                       };

        /// <summary>
        /// Получает коллекцию этапов в заданном диапазоне дат, удовлетворяющих заданному условию результата пересечения дат
        /// </summary>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <param name="selectFunc">Предикат выбора</param>
        /// <returns>Коллекция этапов</returns>
        public IEnumerable<Stage> SelectStages(DateTime fromDate, DateTime toDate, Func<DateRangesIntersectionResult, bool> selectFunc)
        {
            return Stages.SelectRange(fromDate, toDate, selectFunc);
        }


        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Возвращает признак того, что этап является листом (т.е. для этапов 1, 1.1, 1.1.1, 1.2, 2 листьями будут 1.1.1, 1.2, 2), т.е. этап не является ни для кого предком
        /// </summary>
        /// <param name="stage">Этап</param>
        /// <returns>Признак того, что этап является листом</returns>
        public bool StageIsLeaf(Stage stage)
        {
            return Stages.FirstOrDefault(st=>st.ParentStage == stage) == null;
        }

        /// <summary>
        /// Получает признак того, что календарный план имеет просроченные этапы
        /// </summary>
        public bool IsOverdue
        {
            get
            {
                return Stages.AsEnumerable().Any(x => x.Stagecondition == StageCondition.Overdue);
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region Nested type: WorktypeErrorHandler

        private class WorktypeErrorHandler : IDataErrorHandler<Schedule>
        {
            #region IDataErrorHandler<Schedule> Members

            public string GetError(Schedule source, string propertyName, ref bool handled)
            {
                if (propertyName == "Worktype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Schedule source, out bool handled)
            {
                handled = false;
                if (source.Worktype  == null)
                {
                    handled = true;
                    return "Вид работ не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: CurrencymeasureErrorHandler

        private class CurrencymeasureErrorHandler : IDataErrorHandler<Schedule>
        {
            #region IDataErrorHandler<Schedule> Members

            public string GetError(Schedule source, string propertyName, ref bool handled)
            {
                if (propertyName == "Currencymeasure")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Schedule source, out bool handled)
            {
                handled = false;
                if (source.Currencymeasure == null)
                {
                    handled = true;
                    return "Единица измерения не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        partial void OnCreated()
        {
            Worktypeid = EntityBase.ReservedUndifinedOid;
            Currencymeasureid = EntityBase.ReservedUndifinedOid;
            
        }

        public object Clone()
        {
            Schedule newSchedule = new Schedule();
            newSchedule.Currencymeasure = Currencymeasure;
            newSchedule.Worktype = Worktype;

            Stage ns;
            foreach (var s in Stages)
            {
                ns = (Stage)s.Clone();
                ns.Schedule = newSchedule;
                newSchedule.Stages.Add(ns);
            }
            
            return newSchedule;

        }


        public object CloneRecursively(object owner, object source)
        {
            Schedule newSchedule = new Schedule();
            newSchedule.Currencymeasure = Currencymeasure;
            newSchedule.Worktype = Worktype;
            //repository.SubmitChanges();

            Stage ns;
            foreach (var s in Stages)
            {
                if (s.Level == 0)
                {
                    ns = (Stage)s.CloneRecursively(newSchedule, null);
                    newSchedule.Stages.Add(ns);
                }
            }
            

            return newSchedule;
        }


        bool INull.IsNull
        {
            get { return false; }
        }

        /// <summary>
        /// Получает первый договор в списке
        /// </summary>
        public Contractdoc DefaultContract
        {
            get { return Schedulecontracts.Select(x => x.Contractdoc).FirstOrDefault(); }
        }

        public Stage FindStage(string stagenum)
        {
            Stage s = Stages.FirstOrDefault(p => p.Num == stagenum);
            if (s == null)
            {
                foreach (Stage ss in Stages)
                {
                    s = ss.FindStage(stagenum);
                }
            }

            return s;
        }


        /// <summary>
        /// Возвращает список этапов - конечных для заданного
        /// </summary>
        /// <param name="stage">Этап</param>
        /// <returns>Список листовых этапов для заданного</returns>
        public IEnumerable<Stage> GetLeafs(Stage stage)
        {
            var res = new List<Stage>();
            if (stage.GetNextChildren() == null) 
                res.Add(stage);
            else
                foreach (var item in stage.GetNextChildren())
                {
                    res.AddRange(item.GetLeafs());
                }
            return res;
        }

        public IEnumerable<Stage> GetNextChildren(Stage stage)
        {
            return Stages.Where(x => x.ParentStage == stage );
        }
    }

    class NullSchedule : Schedule, INull
    {
        bool INull.IsNull { get { return true; }}       
        new public Contractdoc DefaultContract
        {
            get { return NullContractdoc.Instance; }
        }


        
    }


}
