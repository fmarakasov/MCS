using System;
using System.Diagnostics.Contracts;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using MContracts.ViewModel;
using McReports;
using McReports.Common;
using McReports.ViewModel;
using McUIBase.Factories;
using System.Collections.Generic;

namespace MContracts.Classes
{
    /// <summary>
    /// Вспомогательный класс для формирования отчётов
    /// </summary>
    public static class ActReportCreatorHelper
    {

        public static void CreateActReports(IEnumerable<Act> acts, Contractdoc contextContract,
                                            IUiQueryParametersProvider uiProvider,
                                            ITemplateProvider templateProvider = null)
        {

            Contract.Requires(acts != null);
            Contract.Requires(contextContract != null);

            var avms = new List<BaseActReportViewModel>();

            foreach (var a in acts)
            {
                var template = a.Realacttype.WellKnownType;
                var actReportViewModelType = GetViewModelType(template);



                var actViewModel = CreateActReportViewModel(actReportViewModelType, a, contextContract);

                actViewModel.IsComposite = true;
                avms.Add(actViewModel);
            }

            MainWindowViewModel.Instance.BuildReportAsync(avms.AsEnumerable());

        }

        private static BaseActReportViewModel CreateActReportViewModel(Type actReportViewModelType, Act act, Contractdoc contextContract)
        {

            Contract.Assert(actReportViewModelType != null);
            var actViewModel = Activator.CreateInstance(actReportViewModelType,
                                                        RepositoryFactory.CreateContractRepository())
                                        .CastTo<BaseActReportViewModel>();
            actViewModel.CurrentAct = actViewModel.Repository.Acts.SingleOrDefault(x => x.Id == act.Id);
            Contract.Assert(actViewModel.CurrentAct != null);
            actViewModel.CurrentAct.ContractObject = actViewModel.Repository.TryGetContext()
                                                                 .Contractdocs.SingleOrDefault(
                                                                     x => x.Id == contextContract.Id);
            Contract.Assert(actViewModel.CurrentAct.ContractObject != null);
            return actViewModel;
        }


        private static Type GetViewModelType(WellKnownActtypes template)
        {
            Type actReportViewModelType = null;
            switch (template)
            {
                case WellKnownActtypes.Undefined:
                    break;
                case WellKnownActtypes.GazpromNiokr:
                    actReportViewModelType = typeof(Act1ReportViewModel);

                    break;
                case WellKnownActtypes.RegionGasHolding:
                    actReportViewModelType = typeof(Act2ReportViewModel);

                    break;
                case WellKnownActtypes.Gazoraspredelenie:

                    actReportViewModelType = typeof(Act3ReportViewModel);
                    break;
                case WellKnownActtypes.MezhRegionGas:
                    actReportViewModelType = typeof(Act4ReportViewModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("template");
            }

            return actReportViewModelType;
        }
        
        /// <summary>
        ///  Вывод акта в отчётную форму
        /// </summary>
        /// <param name="act">Экземпляр акта</param>
        /// <param name="contextContract"> </param>
        /// <param name="template">Тип шаблона акта </param>
        /// <param name="templateProvider">Провайдер шаблона отчёта </param>
        public static void CreateActReport(Act act, Contractdoc contextContract,
                                           IUiQueryParametersProvider uiProvider,
                                           ITemplateProvider templateProvider = null)
        {

            Contract.Requires(act != null);
            Contract.Requires(contextContract != null);
            var template = act.Realacttype.WellKnownType;
            var actReportViewModelType = GetViewModelType(template);



            var actViewModel = CreateActReportViewModel(actReportViewModelType, act, contextContract);
            actViewModel.IsComposite = false ;
            MainWindowViewModel.Instance.BuildReportAsync(actViewModel);


        }

    }
}