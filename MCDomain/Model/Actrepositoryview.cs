using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;

namespace MCDomain.Model
{
    public partial class Actrepositoryview : IAct
    {
        /// <summary>
        /// Получает данные об общей сумме по акту
        /// </summary>
        public MoneyModel ActMoney { get; set; }

     
        /// <summary>
        /// Получает данные по сумме к перечислению
        /// </summary>
        public MoneyModel TransferSumMoney { get; set; }

    }
}
