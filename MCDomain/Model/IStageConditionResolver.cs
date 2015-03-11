using System;
using System.ComponentModel;
using CommonBase;
using System.Linq;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет возможные состояния по этапу
    /// </summary>
    public enum StageCondition
    {
        /// <summary>
        /// Неопределенное состояние - ?
        /// </summary>
        [Description("Не определено")]
        Undefined,
        /// <summary>
        /// Этап завершён. Есть подписанный акт.
        /// </summary>
        [Description("Завершён")]
        Closed,
        /// <summary>
        /// Этап ожидает начала выполнения. Акт не подписан.
        /// </summary>
        [Description("Ожидает начала работ")]
        Pending,
        /// <summary>
        /// Этап должнен быть сдан.
        /// Это состояние используется для определения состояния этапа за период
        /// </summary>
        [Description("Завершается в заданном периоде")]
        HaveToEnd,
        /// <summary>
        /// Этап выполняется в данный момент. Сроки не нарушены. Акт не подписан.
        /// </summary>
        [Description("Работы идут в данный момент")]
        Active,
        /// <summary>
        /// Этап просрочен. Акт не подписан
        /// </summary>
        [Description("Просрочен")]
        Overdue                
    }

    /// <summary>
    /// Определяет классы, которые могут вычислять состояние этапа
    /// </summary>
    public interface IStageConditionResolver
    {
        /// <summary> 
        /// Получает состояние этапа договора на заданную дату
        /// </summary>
        /// <param name="stage">Этап договора</param>
        /// <param name="onDate">Дата</param>
        /// <returns>Состояние договора</returns>
        StageCondition GetStageCondition(Stage stage, DateTime onDate);

        StageCondition GetPlanStageCondition(Stage stage, DateTime onDate);
    }

    /// <summary>
    /// Заглушка для вычисления состояния этапа
    /// </summary>
    class StubStageConditionResolver : IStageConditionResolver
    {
        /// <summary>
        /// Получает состояние этапа договора
        /// </summary>
        public StageCondition Condition { get; private set; }

        public StageCondition PlanCondition { get; private set; }

        /// <summary>
        /// Создаёт экземпляр StubStageConditionResolver с заданным состоянием
        /// </summary>
        /// <param name="condition">Состояние этапа</param>
        public StubStageConditionResolver(StageCondition condition)
        {
            Condition = condition;
        }
        
        public StageCondition GetStageCondition(Stage stage, DateTime onDate)
        {
            return Condition;
        }

        public StageCondition GetPlanStageCondition(Stage stage, DateTime onDate)
        {
            return PlanCondition;
        }
    }

    /// <summary>
    /// Получает состояние этапа договора
    /// </summary>
    class StageConditionResolver : IStageConditionResolver
    {
        public static readonly StageConditionResolver Instance = new StageConditionResolver();


        /// <summary>
        /// Получает состояние этапа договора
        /// </summary>
        /// <param name="stage">Этап договора</param>
        /// <param name="onDate">Дата на которую требуется вычислить состояние</param>
        /// <returns>Состояние этапа договора</returns>
        public StageCondition GetStageCondition(Stage stage, DateTime onDate)
        {

            if (stage.Stages.Count == 0)
            {
                if (stage == null)
                    throw new ArgumentNullException("stage", "Параметр stage должен быть задан.");

                //Если закрыт актом, датируемым более ранней датой - считать закрытым
                if (stage.HasActOrParentHasActOnDate(onDate)) return StageCondition.Closed;

                //
                if (stage.Endsat == default(DateTime) || stage.Startsat == default(DateTime))
                    return StageCondition.Undefined;
                //если в этапе нет даты начала или даты конца - неопределенное сосотояние
                if (!stage.Startsat.HasValue || !stage.Endsat.HasValue) return StageCondition.Undefined;

                if (onDate.Between(stage.Startsat.Value, stage.Endsat.Value)) return StageCondition.Active;

                //Если начало этапа больше даты, на которую вычисляем состояние - ожидает начала выполнения
                if (stage.Startsat > onDate) return StageCondition.Pending;

                //Если не закрыт актом и срок выполенения вышел - просрочен
                if (onDate > stage.Endsat.Value && !stage.HasActOrParentHasActOnDate(onDate))
                    return StageCondition.Overdue;

                return StageCondition.Overdue;
            }
            else
            {
                var res = (stage.Stages.Select(st => GetStageCondition(st, onDate)).Any(stc => stc == StageCondition.Active))?StageCondition.Active:StageCondition.Undefined;
                if (res == StageCondition.Undefined) res = (stage.Stages.All(x => GetStageCondition(x, onDate) == StageCondition.Closed) ? StageCondition.Closed : StageCondition.Undefined);
                return res;
            }
        }


        public StageCondition GetPlanStageCondition(Stage stage, DateTime onDate)
        {
            if (stage == null)
                throw new ArgumentNullException("stage", "Параметр stage должен быть задан.");

            if (stage.Endsat == default(DateTime) || stage.Startsat == default(DateTime))
                return StageCondition.Undefined;
            //если в этапе нет даты начала или даты конца - неопределенное сосотояние
            if (!stage.Startsat.HasValue || !stage.Endsat.HasValue) return StageCondition.Undefined;

            if (onDate.Between(stage.Startsat.Value, stage.Endsat.Value)) return StageCondition.Active;

            //Если начало этапа больше даты, на которую вычисляем состояние - ожидает начала выполнения
            if (stage.Startsat > onDate) return StageCondition.Pending;

            //Если не закрыт актом и срок выполенения вышел - просрочен
            if (onDate > stage.Endsat.Value && !stage.HasActOrParentHasActOnDate(onDate)) return StageCondition.Overdue;

            return StageCondition.Overdue;
        }
    }
}
