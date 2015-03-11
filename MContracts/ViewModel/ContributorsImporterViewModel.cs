using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Importer;
using MCDomain.DataAccess;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    class ContributorsImporterViewModel : BaseImporterViewModel
    {
        #region Constructor
        public ContributorsImporterViewModel(IContractRepository repository) : base(repository)
        {
           

        }
        #endregion

        #region Properties

        private IList<Contractdoc> _contractdoclist;
        public IList<Contractdoc> Contractdoclist
        {
            get 
            {
                if (_contractdoclist == null) _contractdoclist = new List<Contractdoc>();
                return _contractdoclist;
            }
        }
        
        #endregion

        #region Methods

        protected override BaseImporter CreateImporter()
        {
            return new ContributorImporter(Repository, CurrentSchedule.Schedule, CurrentSchedule.Contractdoc);
        }

        protected override void FillStagesList(IList<Stage> outputstageslist, ImportedStage s, Stage parentstage)
        {
            if (s.Contributor != null)
            {
                if (!outputstageslist.Any(x => (x.Num == s.Num)))
                {

                    // проверяем внесен ли контрибутор в список контрибуторов
                    Contractor cntrctr;
                    if (s.Contributor.Id == 0)
                    {
                        // если не внесен - вносим, сабмитим
                        cntrctr = new Contractor()
                        {
                            Name = s.Contributor.Name,
                            Shortname = s.Contributor.ShortName,
                            Contractortype = Repository.Contractortypes.FirstOrDefault(c => c.Id == -1)
                        };
                        Repository.InsertContractor(cntrctr);
                        //Repository.SubmitChanges();
                        s.Contributor.Id = cntrctr.Id;
                    }
                    else
                    {
                        cntrctr = Repository.Contractors.FirstOrDefault(c => c.Id == s.Contributor.Id);
                    }



                    // проверяем есть ли уже в списке договоров договор с этим контрибутором 
                    Contractdoc subcontract;
                    Ndsalgorithm curndsalgorithm;
                    Nds curnds;

                    curndsalgorithm = (Importer as ContributorImporter).DefaultNdsalgorithm;
                    curnds = (Importer as ContributorImporter).DefaultNds;

                    if (!(Importer as ContributorImporter).UseDefaultNdsalgorithm)
                        if ((s.GeneralContractStage != null) && (s.GeneralContractStage.Ndsalgorithm != null)) curndsalgorithm = s.GeneralContractStage.Ndsalgorithm;

                    if (!(Importer as ContributorImporter).UseDefaultNds)
                        if ((s.GeneralContractStage != null) && (s.GeneralContractStage.Nds != null)) curnds = s.GeneralContractStage.Nds;


                    subcontract = Contractdoclist.FirstOrDefault(c => c.Contractorcontractdocs.Any(x => x.Contractor == cntrctr));

                    Schedulecontract scheduleContract;
                    // если нет - добавляем договор и добавляем его в список
                    if (subcontract == null)
                    {

                        subcontract = Repository.NewContractdoc();
                        subcontract.Subject = s.Subject;
                        subcontract.Contractorcontractdocs.Add(new Contractorcontractdoc() { Contractor = cntrctr });
                        subcontract.Ndsalgorithm = curndsalgorithm;
                        subcontract.Nds = curnds;
                        subcontract.Startat = s.Startsat;
                        subcontract.Endsat = s.Endsat;
                        subcontract.Price = s.Price;

                        var ch = new Contracthierarchy()
                                                         {
                                                             GeneralContractdoc = CurrentConrtactdoc,
                                                             SubContractdoc = subcontract
                                                         };

                        CurrentConrtactdoc.Contracthierarchies.Add(ch);
                        subcontract.Generalcontracthierarchies.Add(ch);
                        //Repository.SubmitChanges();

                        Contractdoclist.Add(subcontract);

                        // добавляем автоматический КП
                        scheduleContract = new Schedulecontract()
                        {
                            Appnum = 1,
                            Schedule = new Schedule()
                            {
                                Currencymeasure = subcontract.Currencymeasure,
                                Worktype = Repository.Worktypes.FirstOrDefault()
                            },
                            Contractdoc = subcontract
                        };
                        scheduleContract.Schedule.Schedulecontracts.Add(scheduleContract);

                        //Repository.SubmitChanges();
                    }
                    else
                    {
                        subcontract.Subject = "Субподрядный договор с двумя и более этапами (требуется переименование)";
                        subcontract.Endsat = s.Endsat;
                        subcontract.Price = subcontract.Price + s.Price;

                        scheduleContract = subcontract.Schedulecontracts.FirstOrDefault();

                        if (scheduleContract == null)
                        {
                            // добавляем автоматический КП
                            scheduleContract = new Schedulecontract()
                            {
                                Appnum = 1,
                                Schedule = new Schedule()
                                {
                                    Currencymeasure = subcontract.Currencymeasure,
                                    Worktype = Repository.Worktypes.FirstOrDefault()
                                },
                                Contractdoc = subcontract
                            };
                            scheduleContract.Schedule.Schedulecontracts.Add(scheduleContract);

                            //Repository.SubmitChanges();
                        }
                    }


                    // закидываем этап к договору

                    Stage rs = new Stage()
                    {
                        Schedule = scheduleContract.Schedule,
                        Num = s.Num,
                        Price = s.Price,
                        Startsat = s.Startsat,
                        Endsat = s.Endsat,
                        Code = s.Code,
                        Subject = s.Subject,
                        GeneralStage = s.GeneralContractStage

                    };



                    rs.Ndsalgorithm = curndsalgorithm;
                    rs.Nds = curnds;




                    if (parentstage != null) rs.ParentStage = parentstage;



                    Ntpsubview ntpsbvw;
                    foreach (ImportedStageResult ir in s.Stageresults)
                    {
                        if ((ir.Ntpsubviewid != 0) && (!ir.UsedDefaultNTPSubView)) ntpsbvw = Repository.Ntpsubviews.FirstOrDefault(p => (p.Id == ir.Ntpsubviewid));
                        else ntpsbvw = (Importer as Importer).DefaultNTPSubview;

                        rs.Stageresults.Add(new Stageresult()
                        {
                            Stage = rs,
                            Name = ir.Name,
                            Ntpsubview = ntpsbvw
                        });
                    }

                    outputstageslist.Add(rs);
                    rs.Num = s.Num;

                    foreach (ImportedStage isc in s.Stages)
                    {
                        FillStagesList(outputstageslist, isc, rs);
                    }
                }
            }
        }

        protected override void InternalSaveResults()
        {
            if (InputStageBindingList != null)
            {

                if (SavingSetting == ImporterSavingSetting.SaveAll)
                {
                    DataContextDebug.DebugPrintRepository(Repository);
                    OutputStageBindingList.Clear();
                    Contractdoclist.Clear();

                    // добавляем закрытые этапы, которые были в ScheduleViewModel
                    // остальные - удаляем

                    foreach (ImportedStage s in Importer.Stages)
                    {
                        FillStagesList(OutputStageBindingList, s, null);
                    }

                    DataContextDebug.DebugPrintRepository(Repository);
                    var ctx = Repository.TryGetContext();
                    ctx.Log = Console.Out;
                }
                NeedSave = true;
                CloseCommand.Execute(null);
            }
        }

        #endregion
    }
}
