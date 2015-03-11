using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    [ContractClass(typeof (FundsContract))]
    public interface IFundsResolver
    {

        /// <summary>
        /// Вычисляет сумму освоенных средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        decimal GetFundsDisbursed(Contractdoc contract, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Вычисляет сумму оставшихся средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        decimal GetFundsLeft(Contractdoc contract, DateTime fromDate, DateTime toDate);

    }

    /// <summary>
    /// Вычисляет сумму освоенных и оставшихся средств по договору
    /// </summary>
    public class DefaultFundsResolver : IFundsResolver
    {
        public static readonly DefaultFundsResolver Instance = new DefaultFundsResolver();

        #region Implementation of IFundsResolver

        private static decimal CalcFunds(Contractdoc contract, DateTime fromDate, DateTime toDate,
                                         Func<Stage, bool> predicate)
        {
            return contract.Schedulecontracts.Sum(
                schedulecontract =>
                schedulecontract.Schedule.SelectStages(fromDate, toDate,
                                                       x => x != DateRangesIntersectionResult.NotInRange).Where(
                                                           predicate).Sum(
                                                               x => x.StageMoneyModel.Factor.National.WithNdsValue));
        }

        /// <summary>
        /// Вычисляет сумму освоенных средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        public decimal GetFundsDisbursed(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            return contract.Schedulecontracts.Select(x => x.Schedule).SelectMany(x => x.Stages).Select(x => x.Act).Where
                (
                    x => x != null && x.Signdate >= fromDate && x.Signdate <= toDate).Distinct().Sum(
                        x => x.TransferSumMoney.Factor.National.WithNdsValue);

            // return CalcFunds(contract, fromDate, toDate, x=>x.Stagecondition==StageCondition.Closed);
        }

        /// <summary>
        /// Вычисляет сумму оставшихся средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        public decimal GetFundsLeft(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            return
                contract.Schedulecontracts.Select(x => x.Schedule).SelectMany(x => x.Stages).Where(
                    x => x.ParentStage == null).Sum(x => x.StageMoneyModel.Factor.National.PriceWithNdsValue);

            #endregion
        }
     
    }
    public class StubFundsResolver : IFundsResolver
    {
        public decimal Disbursed { get; private set; }
        public decimal Left { get; private set; }

        public StubFundsResolver(decimal disbured, decimal left)
        {
            Disbursed = disbured;
            Left = left;
        }

        #region Implementation of IFundsResolver

        /// <summary>
        /// Вычисляет сумму освоенных средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        public decimal GetFundsDisbursed(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            return Disbursed;
        }

        /// <summary>
        /// Вычисляет сумму оставшихся средств по договору за указанный период
        /// </summary>
        /// <param name="contract">Объект договора</param>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <returns>Сумма оставшихся к освоению средств</returns>
        public decimal GetFundsLeft(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            return Left;
        }

        #endregion
    }
}
