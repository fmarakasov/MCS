using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class ContractRepositoryDto : EntityDto<Contractdoc>
    {
        public double Id { get; set; }
        public string Subject { get; set; }
        public bool IsSubGeneral { get; set; }
        public bool IsAggreement { get; set; }
        public bool InternalNum { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public WellKnownContractStates State { get; set; }
        public ContractCondition Condition { get; set; }
        public string Contractor { get; set; }
        public string Manager { get; set; }
        public decimal Price { get; set; }
        public decimal FoundsDisbursed { get; set; }
        public decimal FoundsLeft { get; set; }
        public bool IsDeleted { get; set; }
        
        public override void InitializeEntity(Contractdoc entity)
        {
            
        }
    }
}
