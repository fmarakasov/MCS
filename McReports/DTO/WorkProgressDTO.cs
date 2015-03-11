using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using CommonBase;

namespace McReports.DTO
{
    public class WorkProgressDTO
    {
        /// <summary>
        /// год, за который строится отчет
        /// </summary>
        public int Year;
        /// <summary>
        /// квартал, за который строится отчет
        /// </summary>
        public Quarters Quarter;
        /// <summary>
        /// договор, по которому готовится отчет
        /// </summary>
        public Contractdoc Contractdoc;
        /// <summary>
        /// тип договора 
        /// </summary>
        public Contracttype Contracttype;
        /// <summary>
        /// руководитель направления и руководитель работ
        /// </summary>
        public string Directors;
        /// <summary>
        /// план работ, всего
        /// </summary>
        public decimal OverallPlan;
        /// <summary>
        /// план работ, соисполнители
        /// </summary>
        public decimal CoworkersPlan;
        /// <summary>
        /// план работ, собственные силы
        /// </summary>
        public decimal OwnPlan
        {
            get { return OverallPlan - CoworkersPlan; }
        }
        /// <summary>
        /// план работ по подписанным, всего 
        /// </summary>
        public decimal SignedOverallPlan;
        /// <summary>
        /// план работ по подписанным, соисполнители
        /// </summary>
        public decimal SignedCoworkersPlan;
        /// <summary>
        /// план работ по подписанным, собственные силы
        /// </summary>
        public decimal SignedOwnPlan
        {
            get { return SignedOverallPlan - SignedCoworkersPlan; }
        }
        /// <summary>
        /// факт, всего
        /// </summary>
        public decimal OverallFact;
        /// <summary>
        /// факт, соисполнители
        /// </summary>
        public decimal CoworkersFact;
        /// <summary>
        /// факт, собственные силы
        /// </summary>
        public decimal OwnFact
        {
            get { return OverallFact - CoworkersFact; }
        }


        /// <summary>
        /// акт на подписи, всего
        /// </summary>
        public decimal OverallWaitingForSignature;
        /// <summary>
        /// акт на подписи, соисполнители
        /// </summary>
        public decimal CoworkersWaitingForSignature;
        /// <summary>
        /// акт на подписи, собственные силы
        /// </summary>
        public decimal OwnWaitingForSignature
        {
            get { return OverallWaitingForSignature - CoworkersWaitingForSignature; }
        }

        /// <summary>
        /// в УИР, всего
        /// </summary>
        public decimal OverallUIR;
        /// <summary>
        /// в УИР, соисполнители
        /// </summary>
        public decimal CoworkersUIR;
        /// <summary>
        /// в УИР, собственные силы
        /// </summary>
        public decimal OwnUIR
        {
            get { return OverallUIR - CoworkersUIR; }
        }


        /// <summary>
        /// у ФЗ, всего
        /// </summary>
        public decimal OverallFZ;
        /// <summary>
        /// у ФЗ, соисполнители
        /// </summary>
        public decimal CoworkersFZ;
        /// <summary>
        /// у ФЗ, собственные силы
        /// </summary>
        public decimal OwnFZ
        {
            get { return OverallFZ - CoworkersFZ; }
        }

        /// <summary>
        /// работы на стадии завершения, всего
        /// </summary>
        public decimal OverallCompletion;
        /// <summary>
        /// работы на стадии завершения, соисполнители
        /// </summary>
        public decimal CoworkersCompletion;
        /// <summary>
        /// работы на стадии завершения, собственные силы
        /// </summary>
        public decimal OwnCompletion
        {
            get { return OverallCompletion - CoworkersCompletion; }
        }
        
        /// <summary>
        /// работы не могут быть завершены, всего
        /// </summary>
        public decimal OverallNoCompletion;
        /// <summary>
        /// работы не могут быть завершены, соисполнители
        /// </summary>
        public decimal CoworkersNoCompletion;

        /// <summary>
        /// работы не могут быть завершены, собственные силы
        /// </summary>
        public decimal OwnNoCompletion
        {
            get { return OverallNoCompletion - CoworkersNoCompletion; }
        }

    }
}
