// This file is intended to be edited manually

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using MCDomain.Common;
using MCDomain.DataAccess;
using UOW;
using ChangeSet = Devart.Data.Linq.ChangeSet;
using CommonBase;

namespace MCDomain.Model
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Получить экземпляр сущьность с зарезервированным идентификатором
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="table">Коллекция</param>
        /// <returns></returns>
        public static T GetReservedUndefined<T>(this IEnumerable<T> table) where T : EntityBase, IObjectId
        {
            Contract.Requires(table != null);
            return table.Single(x => x.Id == EntityBase.ReservedUndifinedOid);
        }
    }

    partial class McDataContext
    {

        public override string ToString()
        {
            return this.GetDebugString();
        }
        /// <summary>
        /// Получает признак, что контекст был модифицировн с момента последнего сохранения изменений
        /// </summary>
        public bool IsModified
        {
            get
            {                
                ChangeSet cs = GetChangeSet();
                return ((cs.Deletes.Count != 0) || (cs.Inserts.Count!=0) || (cs.Updates.Count!=0)); 
            }
        }
        /// <summary>
        /// Создаёт новый объект Approvalprocess
        /// </summary>
        /// <param name="contractObject"></param>
        /// <returns></returns>
        public Approvalprocess NewApprovalProcess(Contractdoc contractdoc)
        {

            Approvalprocess prevApprvl = contractdoc.Approvalprocesses.LastOrDefault();

            var result = new Approvalprocess()
                             {
                                 Approvalstate = Approvalstates.GetReservedUndefined(),
                                 Enteringdate = DateTime.Today, 
                                 Enterstateat = (prevApprvl != null)?prevApprvl.Transferstateat:DateTime.Today,
                                 Transferstateat = DateTime.Today,
                                 Missivedate = DateTime.Today,
                                 ToLocation = Locations.GetReservedUndefined(),
                                 FromLocation = (prevApprvl != null) ? prevApprvl.ToLocation : Locations.GetReservedUndefined(),
                                 Approvalgoal = Approvalgoals.GetReservedUndefined(),
                                 Missivetype = Missivetypes.GetReservedUndefined()
                                
                             };

            
            
            return result;
        }

        /// <summary>
        /// Получает коллекцию актуальных договоров на заданный период дат
        /// </summary>
        /// <param name="start">Начальная дата</param>
        /// <param name="end">Конечная дата</param>
        /// <returns>Коллекция актуальных договоров</returns>
        public IEnumerable<IContractStateData> FetchActualContractviews(DateTime start, DateTime end)
        {
            Contract.Requires(start <= end);
            Contract.Ensures(Contract.Result<IEnumerable<Contractrepositoryview>>()!=null);

            var result = Contractrepositoryviews.Where(DefaultContractConditionReolver.ActualContractExpression(start, end));
            return result;
            
        }
        
        public void UpdateFundsStatistics()
        {
            foreach (var contractdoc in Contractdocs)
            {
                contractdoc.UpdateFundsStatistics();
            }
        }

        public IEnumerable<IContractStateData> FetchActualContractviews(IEnumerable<long> ids)
        {

            Contract.Ensures(Contract.Result<IEnumerable<Contractrepositoryview>>() != null);

            var result = Contractrepositoryviews.Where(c=> ids.Contains(c.Id));
            return result;
        }
        /// <summary>
        /// Получает все договора, от которого зависит заданный договор. 
        /// В коллекцию включаются все  вышестоящие доп. соглашения.</summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>Коллекция всех ДС, от которых зависит этот договор</returns>
        public IEnumerable<Contractdoc>GetDependantContracts(long contractId)
        {
            var contract = Contractdocs.Single(x=>x.Id == contractId);
            var curr = contract.OriginalContract;
            while (curr != null)
            {
                yield return curr;
                curr = curr.OriginalContract;
            }
        }

    }
}
