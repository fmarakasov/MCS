using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class TroubleContractDto
    {
        public Contractdoc Contractdoc;
        public Stage Stage;
        
        
        public decimal PrevyearItem;
        public decimal PrevyearMoney;
        public decimal CurrentyearItem;
        public decimal CurrentyearMoney;


    }
}
