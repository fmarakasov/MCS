using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class ContractInActionDTO
    {
        /// <summary>
        /// Год, за который строится отчет
        /// </summary>
        public int Year;

        public Schedulecontract Schedulecontract;
        public Contractdoc Contract;
        public Stage Stage;
        public string Ordersuperviser;
        public string Director;
        public string Contractortype;
        public string Appnum;
        public string Chief;
        public string OrderResponsible;
        public string Contractnum;
        public DateTime? Contractdate;
        public string Agreementnum;
        public string Contractstate;
        public string Contracttype;
        public string Customer;
        public string Subject;
        public string Stagenum;
        public string Objectcode;
        public string Stagesubject;
        public decimal Stagecost;
        public DateTime? StageStartsAt;
        public DateTime? StageFinishesAt;
        public DateTime? Actdate;
        public string Actnum;
        public decimal Actcost;
        public string  Actstate;
        public decimal Subcontractcost;
        public decimal Subcontractactcost;
        public string  Contractconditioncomment;
        
        public decimal Owncost
        {
            get { return Stagecost - Subcontractcost; }
        }

        
        
  


    }
}
