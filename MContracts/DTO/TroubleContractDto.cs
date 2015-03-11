using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.DTO
{
    public class TroubleContractDto
    {
        public long ContractId { get; set; }
        public long TroubleId { get; set; }
        public string Name { get; set; }
        
        private Contracttrouble _entity;
        
        public static TroubleContractDto AsDto(Trouble trouble)
        {
            Contract.Requires(trouble != null);
            Contract.Ensures(Contract.Result<TroubleContractDto>()!=null);
            return new TroubleContractDto() {TroubleId = trouble.Id, Name = trouble.ToString()};
        }
        
        public static TroubleContractDto AsDto(Contracttrouble trouble)
        {
            Contract.Requires(trouble != null);
            Contract.Ensures(Contract.Result<TroubleContractDto>() != null);
            var newDto = AsDto(trouble.Trouble);
            newDto.ContractId = trouble.Contractdocid;
            newDto.Name = trouble.Trouble.ToString();
            return newDto;
        }
 
        public Contracttrouble AsEntity
        {
            get
            {
                if (_entity == null)
                    _entity = new Contracttrouble() { Contractdocid = ContractId, Troubleid = TroubleId };
                return _entity;
            }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
