using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Текущее состояние договора
    /// </summary>
    [Flags]
    public enum ContractCondition
    {
        /// <summary>
        /// Состояние договра не определено
        /// </summary>
        [Description("Не определено")] Undefined = 0x0,

        /// <summary>
        /// Актуальные - такие договора, для которых не выставлены даты завершения,
        /// снятия с контроля и расторжения или одна из этих дат не входт в рассматриваемый контекстный период
        /// </summary>
        [Description("Актуальный")] Actual = 0x1,

        /// <summary>
        /// Действующий договор 
        /// </summary>
        [Description("Действующий")] NormalActive = 0x2,

        /// <summary>
        /// договора - разорванные на дату 
        /// </summary>
        [Description("Разорван в текущем периоде")] Broken = 0x4,

        /// <summary>
        /// договора - разорванные в предыдущем периоде
        /// </summary>
        [Description("Разорван в предыдущем периоде")] BrokenPreviously = 0x8,

        /// <summary>
        /// снят с контроля в текущем периоде
        /// </summary>
        [Description("Снят с контроля в текущем периоде")] OutOfControl = 0x10,

        /// <summary>
        /// снят с контроля в предыдущем периоде
        /// </summary>
        [Description("Cнят с контроля в предыдущем периоде")] OutOfControlPreviously = 0x20,

        /// <summary>
        /// завершен (закрыты все этапы) на дату
        /// </summary>
        [Description("Завершен в заданном периоде")] Closed = 0x40,

        /// <summary>
        /// завершен до начала заданного периода
        /// </summary>
        [Description("Завершен до начала заданного периода")] СlosedPreviously = 0x80,
    }

    /// <summary>
    /// Интерфейс, задающий вычисление состояния договора на определённую дату
    /// </summary>
    public interface IContractConditionResolver
    {
        ContractCondition GetContractCondition(IContractStateData contract, DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Класс вычисленителя состояния договора
    /// </summary>
    internal class DefaultContractConditionReolver : IContractConditionResolver
    {
        /// <summary>
        /// Получает единственный экземпляр класса DefaultContractStateReolver
        /// </summary>
        public static readonly DefaultContractConditionReolver Instance = new DefaultContractConditionReolver();

        private DefaultContractConditionReolver()
        {
        }

        /// <summary>
        /// Возвращает выражение для оперделения, является ли заданный объект договора - актуальным на переданный интервал дат
        /// </summary>
        /// <param name="start">Начальная дата</param>
        /// <param name="end">Конечная дата</param>
        /// <returns>Выражение предиката</returns>
        public static Expression<Func<IContractStateData, bool>> ActualContractExpression(DateTime start, DateTime end)
        {
            return x => ((!x.Brokeat.HasValue && !x.Reallyfinishedat.HasValue && !x.Outofcontrolat.HasValue) ||
                        ((x.Brokeat >= start && x.Brokeat <= end) ||
                         (x.Reallyfinishedat >= start && x.Reallyfinishedat <= end) ||
                         (x.Outofcontrolat >= start && x.Outofcontrolat <= end)))
                         &&(!x.Startat.HasValue||x.Startat.HasValue&&x.Startat <= end);
                         //&& (x.Agreementreferencecount == 0) - мне кажется это пока не нужно, бывает что соглашение и основной договор действуют одновременно;
        }

        private DateCheckResult CheckDate(DateTime? value, DateTime start, DateTime end)
        {
            return new DateCheckResult
                       {
                           InPeriod = value.HasValue && value.Value.Between(start, end),
                           Previously = value.HasValue && value < end
                       };
        }

        private void ApplyCondition(DateTime? value, DateTime startDate, DateTime endDate,
                                    ContractCondition inPeriodCondition, ContractCondition prevPeriodCondition,
                                    ref ContractCondition collectorState)
        {
            DateCheckResult checkResult = CheckDate(value, startDate, endDate);
            if (checkResult.InPeriod) collectorState = collectorState | inPeriodCondition;
            if (checkResult.Previously) collectorState = collectorState | prevPeriodCondition;
        }

        #region Implementation of IContractStateResolver

        /// <summary>
        /// Получает состояние договора на определённую дату
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <param name="startDate">Дата начала периода</param>
        ///   /// <param name="endDate">Дата окончания периода</param>
        /// <returns>Состояние договора</returns>
        public ContractCondition GetContractCondition(IContractStateData contract, DateTime startDate, DateTime endDate)
        {
            Contract.Assert(contract != null);

            var c = ContractCondition.Undefined;

            if (IsActualContract(contract, startDate, endDate))
                c = c | ContractCondition.Actual;

            ApplyCondition(contract.Brokeat, startDate, endDate, ContractCondition.Broken,
                           ContractCondition.BrokenPreviously, ref c);
            ApplyCondition(contract.Outofcontrolat, startDate, endDate, ContractCondition.OutOfControl,
                           ContractCondition.OutOfControlPreviously, ref c);
            ApplyCondition(contract.Reallyfinishedat, startDate, endDate, ContractCondition.Closed,
                           ContractCondition.СlosedPreviously, ref c);
            return c;
        }

        public static bool IsActualContract(IContractStateData contract, DateTime startDate, DateTime endDate)
        {
            return (!contract.Brokeat.HasValue && !contract.Reallyfinishedat.HasValue &&
                    !contract.Outofcontrolat.HasValue) ||
                   ((contract.Brokeat >= startDate && contract.Brokeat <= endDate) ||
                    (contract.Reallyfinishedat >= startDate && contract.Reallyfinishedat <= endDate) ||
                    (contract.Outofcontrolat >= startDate && contract.Outofcontrolat <= endDate) &&
                    (contract.Agreementreferencecount == 0));
        }

        #endregion

        #region Nested type: DateCheckResult

        private struct DateCheckResult
        {
            public bool InPeriod { get; set; }
            public bool Previously { get; set; }
        }

        #endregion
    }


    /// <summary>
    /// Заглушка для тестирования вычисления состояния договора
    /// </summary>
    public class StubContractConditionResolver : IContractConditionResolver
    {
        /// <summary>
        /// Создаёт экземпляр вычислителя состояния договора
        /// </summary>
        /// <param name="contractState">предопределённое состояние</param>
        public StubContractConditionResolver(ContractCondition contractState)
        {
            Condition = contractState;
        }

        /// <summary>
        /// Получает предопределённое состояние договора
        /// </summary>
        public ContractCondition Condition { get; private set; }

        #region IContractConditionResolver Members

        public ContractCondition GetContractCondition(IContractStateData contract, DateTime startDate, DateTime endDate)
        {
            return Condition;
        }

        #endregion
    }
}