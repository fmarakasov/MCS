using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using System.ComponentModel;

namespace MContracts.DTO
{
    public class EfparameterstageresultDto : EntityDto<Efparameterstageresult>, INotifyPropertyChanged
    {


        public long Id { get; set; }

        public decimal? Value { get; set; }

        public Economefficiencyparameter Economefficiencyparameter { get; set; }

        public StageresultDto Stageresult { get; set; }


        public override void InitializeEntity(Efparameterstageresult entity)
        {
            entity.Id = Id;
            entity.Value = Value;
            entity.Economefficiencyparameter = Economefficiencyparameter;
        }

    }
}
