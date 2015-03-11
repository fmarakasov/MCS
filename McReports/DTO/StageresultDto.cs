using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace McReports.DTO
{

    public class ComparativeStageresultDto
    {
        public Stageresult Stageresult;
        public Stage Stage;
        public Contractdoc Contractdoc;

        public decimal PrevEconEffectMln;
        public decimal EconEffectMln;

        public decimal PrevyearItem;
        public decimal PrevyearMoney;
        public decimal CurrentyearItem;
        public decimal CurrentyearMoney;

    }

    public class StageresultDto
    {

        #region Properties
        public long Id { get; set; }

        public Stageresult Stageresult { get; set; }
        public string   Stagename { get; set; }
        public string   Name { get; set; }
        public string   Contractdocnum { get; set; }
        public string   Contractdocdate { get; set; }
        public string   Contractorname { get; set; }
        public string   FunctionalCustomerName { get; set; }
        public decimal  NTRUKres { get; set; }
        
        public string   IntroductionActNum { get; set; }
        public DateTime? IntroductionActDate { get; set; }
        public string   EfficiencyComment { get; set; }
        public decimal  Period { get; set; }


        public decimal EconEffect { get; set; }
        public decimal EconEffectMln { get; set; }

        #endregion

    }
}