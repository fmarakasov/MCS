using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;
using UOW;

namespace MCDomain.Model.RepositoryExtensions
{
    public static class ContractViewRepositoryExtensions
    {
        public static IEnumerable<IContractStateData> FetchActualContractviews(
            this IRepository<Contractrepositoryview> source, DateTime fromDateTime, DateTime toDateTime)
        {
            return source.AsQueryable().Where(DefaultContractConditionReolver.ActualContractExpression(fromDateTime, toDateTime));
        }

        public static IEnumerable<IContractStateData> FetchActualContractviews(
            this IRepository<Contractrepositoryview> source, IEnumerable<long> ids)
        {
            return source.AsQueryable().Where(c => ids.Contains(c.Id));
        }
    }

    public static class UnitOfWorkExtensions
    {
        private static void DeleteContractInternal(IUnitOfWork uof, Contractdoc contractdoc, IList<IContractStateData> deletedcontracts)
        {

            deletedcontracts.Add(contractdoc);

            for (var i = contractdoc.Agreements.Count() - 1; i >= 0; i--)
                DeleteContractInternal(uof, contractdoc.Agreements.ToList()[i], deletedcontracts);

            for (var i = contractdoc.SubContracts.Count() - 1; i >= 0; i--)
                DeleteContractInternal(uof, contractdoc.SubContracts.ToList()[i], deletedcontracts);

            // зачищаем авансы
            contractdoc.Prepayments.Clear();

            // зачищаем соподчиненность генеральный-субподрядный
            contractdoc.Contracthierarchies.Clear();
            contractdoc.Generalcontracthierarchies.Clear();

            // смотрим по каким документам проходили платежи
            //var paymentdocs =
            //  Context.Paymentdocuments.Where(p => contractdoc.Contractpayments.Count(c => c.Paymentdocument.Id == p.Id) > 0).ToList();
            var paymentdocs = contractdoc.Contractpayments.Select(x => x.Paymentdocument).ToList();
            // зачищаем платежные документы по договору
            contractdoc.Contractpayments.Clear();
            // после того, как содержимое удалено можно удалить сами документы 
            for (var i = paymentdocs.Count() - 1; i >= 0; i--)
                uof.Repository<Paymentdocument>().Delete(paymentdocs.ToList()[i]);

            // зачищаем проблемы
            contractdoc.Contracttroubles.Clear();
            // зачищаем связи с функциональными заказчиками
            contractdoc.Functionalcustomercontracts.Clear();

            var schs = contractdoc.Schedulecontracts.Select(s => s.Schedule).ToList();
            // зачищаем календарные планы
            contractdoc.Schedulecontracts.Clear();

            // после того, как содержимое удалено можно удалить сами документы 
            for (var i = schs.Count() - 1; i >= 0; i--)
                if (!contractdoc.HasTheSameSchedule(schs[i]))
                    uof.Repository<Schedule>().Delete(schs.ToList()[i]);

            var ds = contractdoc.Responsibles.Select(d => d.Disposal).ToList();

            ds = ds.Where(d => (d.Contractdocs != null && d.Contractdocs.Any())).ToList();

            // зачищаем ответственных
            contractdoc.Responsibles.Clear();
            // зачищаем распоряжения по этому договору
            for (var i = ds.Count() - 1; i >= 0; i--)
                uof.Repository<Disposal>().Delete(ds[i]);

            // зачищаем процесс согласования
            contractdoc.Approvalprocesses.Clear();

            // зачищаем контракторов 
            contractdoc.Contractorcontractdocs.Clear();
            // зачищаем документы 
            contractdoc.Contractdocdocumentimages.Clear();
            contractdoc.ContractdocFundsFacts.Clear();

            // удаляем сам договор
            uof.Repository<Contractdoc>().Delete(contractdoc);
            

        }

        public static IEnumerable<IContractStateData> DeleteContractdoc(this IUnitOfWork uof, long id)
        {
            var deletedcontracts = new List<IContractStateData>();
            var contract = uof.Repository<Contractdoc>().Single(x => x.Id == id);
            DeleteContractInternal(uof, contract, deletedcontracts);
            //Context.SubmitChanges();
            return deletedcontracts;
        }
    }
}
