using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.DTO;
using Microsoft.Office.Interop.Excel;

namespace MContracts.ViewModel.Reports
{
    /// <summary>
    /// 5. Пречень субподрядных договоров на 20ХХ год.
    /// </summary>
    public class SubContractRegisterReport_5_ViewModel: ExcelReportViewModel
    {
         public SubContractRegisterReport_5_ViewModel(IContractRepository contractRepository):base(contractRepository)
         {
             Year = DateTime.Today.Year;
         }

         public override string DisplayName
         {
             get
             {
                 return "Перечень договоров с соисполнителями";
             }
             protected set
             {
                 base.DisplayName = value;
             }
         }

         [InputParameter(typeof(int), "Год")]
         public int Year { get; set; }

         public override string Detail
         {
             get { return "Contract"; }
         }

         public Worksheet StageWorkSheet
         {
             get { return Excel.Worksheets["Detail"]; }
         }

         public Worksheet BaseStageWorkSheet
         {
             get { return Excel.Worksheets["BaseStage"]; }
         }

         private static Worksheet ContractTailWorkSheet
         {
             get { return Excel.Worksheets["ContractTail"]; }
         }

         private static Worksheet ContractTypeTailWorkSheet
         {
             get { return Excel.Worksheets["ContractTypeTail"]; }
         }

         private static Worksheet ContractorTypeTailWorkSheet
         {
             get { return Excel.Worksheets["ContractorTypeTail"]; }
         }

         public override string ReportFileName
         {
             get { return "SubContractRegisterReport_5"; }
         }

        protected override DateTime StartPeriod
         {
             get { return DateTimeExtensions.GetFirstYearDay(Year); }
         }

         protected override DateTime EndPeriod
         {
             get { return DateTimeExtensions.GetLastYearDay(Year); }
         }

        private List<SubContractDto> _contracts
         {
             get
             {
                 //У договора должн быть календарный план с этапами
                 IEnumerable<Contractdoc> contractdocs = GetActiveСontractDocs();
                 if (contractdocs != null && contractdocs.Count() > 0)
                 {
                     var e =
                         contractdocs.Aggregate(new List<SubContractDto>(), (a, g) =>
                                      {
                                       //Суб договор должен иметь КП с этапами
                                          a.AddRange(g.SubContracts.Where(s => s.HasSchedule && s.Stages != null && s.Stages.Count() > 0).
                                                       Aggregate(new List<SubContractDto>(), (scs, sub) =>
                                                       {
                                                           scs.AddRange(sub.Stages.Aggregate(new List<SubContractDto>(), (sts, stage) =>
                                                                          {
                                                                           sts.Add(new SubContractDto()
                                                                            {
                                                                                Year = Year,

                                                                                ContractorType = (g.Contractor != null && g.Contractor.Contractortype != null) ? g.Contractor.Contractortype:
                                                                                                 Repository.Contractortypes.FirstOrDefault(x=>x.WellKnownType == WellKnownContractorTypes.Other),

                                                                                ContractType = g.Contracttype ?? Repository.Contracttypes.FirstOrDefault(x=>x.WellKnownType == WellKnownContractTypes.Other),

                                                                                SubContract = sub,

                                                                                GenContract = g,
                                                                                
                                                                                StageGen = stage.GeneralStage,

                                                                                //Ответственные лица по суб договору
                                                                                Directors = sub.Directors(true),

                                                                                //Номер соответствующего в ген. договоре этапа
                                                                                NumStageGen = stage.GeneralStage != null ? stage.GeneralStage.Num : "",


                                                                                //Сроки ЭТАПА генерального договора
                                                                                RunTimeGen = stage.GeneralStage == null ? string.Empty : stage.GeneralStage.RunTime,

                                                                                //Исполнитель по субдоговору (субподрядная организация)
                                                                                SubOrganization = sub.ContractorName,

                                                                                //Номер и дата субдоговора    
                                                                                NumDateSub = sub.ShortName,

                                                                                //Название работы этапа субподрядного договора
                                                                                Subject = stage.Subject ?? "",

                                                                                //Сроки выполнения ЭТАПА субподрядного договора
                                                                                RunTimeSub = stage.RunTime,

                                                                                //Общая сумма  субподрядного договора
                                                                                Price = stage.StageMoneyModel.National.Factor.WithNdsValue / Measure,

                                                                                //Общая сумма, закрытая актаим
                                                                                PriceByAct = stage.Act != null && stage.Act.Signdate.HasValue && stage.Price.HasValue ?
                                                                                             stage.StageMoneyModel.Factor.National.WithNdsValue / Measure : 0,

                                                                                //Номер и дата акта
                                                                                NumDateAct = stage.Act != null ? stage.Act.FullName : "",

                                                                                ContractTypeName = g.ContractTypeName,

                                                                                ContractorTypeName = g.ContractorTypeName,

                                                                                NumDateGen = (g.Internalnum != null ? "Договор № " + g.Internalnum.ToString() : "") +
                                                                                             (g.Subject != null ? " \"" + g.Subject + " \" " : "") +
                                                                                             (g.Approvedat.HasValue ? " от " + g.Approvedat.GetValueOrDefault().ToString("dd.MM.yyyy") : "")
                                                                            });
                                                                           return sts;
                                                                       }));
                                                                   return scs;
                                                               }));
                                                     return a;
                                                 });
                     return e;
                 }
                 return null;
             }
         }
        
        private IEnumerable<Contractdoc> GetActiveСontractDocs()
         {
             //должны  отображаться данные из: 
             //a)	календарных планов субподрядных договоров  к тем этапам генерального договора, работы по которым подлежат сдаче в отчетном периоде;
             //b)	календарных планов субподрядных договоров к тем этапам генерального договора, работы по которым подлежали сдаче в предыдущие периоды, но сданы не были;
             //c)	актов сдачи-приемки, подписанные для этапов субподрядных договоров, удовлетворяющих вышеперечисленным условиям. 
             return Repository.Contracts.Where(
                 //Необходимо, чтоб договор имел календарный план с запланированными этапами и был генеральным
                 x => x.HasSchedule && x.Stages != null && x.Stages.Count() > 0 && x.IsGeneral).Select(x => x.Actual).Distinct().Where(x =>
                 (
                                  // состояние хотя бы одного из этапа было или неопределенное или подлежит сдаче или просроченное и договор был подписан
                                x.Stages.FirstOrDefault(st =>
                                    st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Undefined ||
                                    st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.HaveToEnd ||
                                    st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue) != null));
         }

        protected override void BuildReport()
        {
            CurrentButtomPosition = 4;

            SetHeader();

            //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
            IEnumerable<SubContractDto> subContractStages = _contracts;

            //Группировка по типу контрагента
            IEnumerable<IGrouping<Contractortype, SubContractDto>> contractorTypeGroups = subContractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, SubContractDto> contractorGroup in contractorTypeGroups)
            {
                BuildContractorType(contractorGroup.Key);

                //Группировка по типу договора
                IEnumerable<IGrouping<Contracttype, SubContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x=>x.Key.Reportorder);
                foreach (IGrouping<Contracttype, SubContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildContractType(contractorGroup.Key, contractTypeGroup.Key);
                     IEnumerable<IGrouping<Contractdoc, SubContractDto>> genContractGroups = contractTypeGroup.GroupBy(x => x.GenContract, x => x);
                     foreach (IGrouping<Contractdoc, SubContractDto> genContractGroup in genContractGroups)
                     {
                         BuildContract(genContractGroup.Key);

                         foreach (SubContractDto stage in genContractGroup)
                         {
                             BuildStage(stage);
                         }

                         BuildContractTail(genContractGroup);
                     }

                    BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);
                }

                BuildContractorTypeTail(contractorGroup);
            }
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }

        private void BuildContractorType(Contractortype ContractorGroup)
        {
            Range r = CopyRowRange(ContractorWorkSheet, 1);
            r.Replace("#ContractorType#", ContractorGroup.Name ?? "");
            r.Rows.AutoFit();
        }

        private void BuildContractType(Contractortype ContractorType, Contracttype ContractType)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", ContractType.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(ContractorType.Name) ?? "");
            r.Rows.AutoFit();
        }

      
        private void BuildContract(Contractdoc contract)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            InsertBigFieldIntoReport("#ContractSubject#", contract.FullName, r);
            r.Rows.AutoFit();
        }

        private void BuildStage(SubContractDto stage)
        {
            Range r = CopyRowRange(StageWorkSheet, 1);
            InsertBigFieldIntoReport("#Directors#", stage.Directors, r);
            InsertBigFieldIntoReport("#NumStageGen#", stage.NumStageGen, r);
            InsertBigFieldIntoReport("#RunTimeGen#", stage.RunTimeGen, r);
            InsertBigFieldIntoReport("#SubOrganization#", stage.SubOrganization, r);
            InsertBigFieldIntoReport("#NumDateSub#", stage.NumDateSub, r);
            InsertBigFieldIntoReport("#Subject#", stage.Subject, r);
            InsertBigFieldIntoReport("#RunTimeSub#", stage.RunTimeSub, r);
            InsertBigFieldIntoReport("#Price#", stage.Price.ToString(NumberFormat), r);
            InsertBigFieldIntoReport("#PriceByAct#", stage.PriceByAct.ToString(NumberFormat), r);
            InsertBigFieldIntoReport("#NumDateAct#", stage.NumDateAct, r);

            r.Rows.AutoFit();
        }

        private void BuildContractTail(IGrouping<Contractdoc, SubContractDto> contract)
        {
            Range r = CopyRowRange(ContractTailWorkSheet, 1);
            InsertBigFieldIntoReport("#Price#", contract.Sum(x => x.Price).ToString(NumberFormat), r);
            InsertBigFieldIntoReport("#PriceByAct#", contract.Sum(x => x.PriceByAct).ToString(NumberFormat), r);
            r.Rows.AutoFit();
        }


        private void BuildContractTypeTail(IGrouping<Contracttype, SubContractDto> contractTypeGroup, Contractortype contractorType)
        {
            Range r = CopyRowRange(ContractTypeTailWorkSheet, 1);
            InsertBigFieldIntoReport("#ContractType#", contractTypeGroup.Key.Name ?? "", r);
            InsertBigFieldIntoReport("#ContractorType#", contractorType.Name ?? "", r);
            InsertBigFieldIntoReport("#Price#", contractTypeGroup.Sum(x => x.Price).ToString(NumberFormat), r);
            InsertBigFieldIntoReport("#PriceByAct#", contractTypeGroup.Sum(x => x.PriceByAct).ToString(NumberFormat), r);
            r.Rows.AutoFit();
        }

        private void BuildContractorTypeTail(IGrouping<Contractortype, SubContractDto> contractorTypeGroup)
        {
            Range r = CopyRowRange(ContractorTypeTailWorkSheet, 1);
            InsertBigFieldIntoReport("#ContractorType#", contractorTypeGroup.Key.Name ?? "", r);
            InsertBigFieldIntoReport("#Price#", contractorTypeGroup.Sum(x => x.Price).ToString(NumberFormat), r);
            InsertBigFieldIntoReport("#PriceByAct#", contractorTypeGroup.Sum(x => x.PriceByAct).ToString(NumberFormat), r);
            r.Rows.AutoFit();
        }

       
    }
}
