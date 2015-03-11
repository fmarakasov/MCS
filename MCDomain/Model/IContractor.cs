using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using CommonBase;

namespace MCDomain.Model
{
    public interface IContractor
    {
        string Name { get; }
        string Account { get;  }
        Contractortype Contractortype { get; }
        string ContractorFullDesc { get; }
        IEnumerable<Person> Persons { get; }
        string Address { get; }
        string Bank { get;  }
        string Bik { get;  }
        string Correspaccount { get;  }
        string Inn { get; }
        string Kpp { get; }
        string Ogrn { get; }
        string Okved { get; }
        string PasportSeries { get; }
        string PasportNum { get; }
        string FamilyName { get; }
        string FirstName { get; }
        string MiddleName { get; }
        string Zip { get;  }
    }

    
    class NullContractorProxy : IContractor, INull
    {
        public static readonly NullContractorProxy Instance = new NullContractorProxy();

        public NullContractorProxy(IEnumerable<Person> persons, string contractorFullDesc)
        {
            ContractorFullDesc = contractorFullDesc;
            Persons = persons;
        }

        public NullContractorProxy()
        {
            // TODO: Complete member initialization
        }

        public string Name { get { return "N/А"; } }
        public Contractortype Contractortype { get { return null; } }
        public string ContractorFullDesc { get; private set; }
        public IEnumerable<Person> Persons { get; private set; }

        public string Account
        {
            get { return "N/A"; }
        }

        public string Zip
        {
            get { return "N/A"; }
        }


        public string Bik
        {
            get { return "N/A"; }
        }

        public string Bank
        {
            get { return "N/A"; }
        }

        public string Correspaccount
        {
            get { return "N/A"; }
        }

        public string Address
        {
            get { return "N/A"; }
        }


        public string Inn
        {
            get { return "N/A"; }
        }

        public string Ogrn
        {
            get { return "N/A"; }
        }

        public string Okved
        {
            get { return "N/A"; }
        }

        public string PasportSeries
        {
            get { return "N/A"; }
        }

        public string PasportNum
        {
            get { return "N/A"; }
        }

        public string FamilyName
        {
            get { return "N/A"; }
        }

        public string FirstName
        {
            get { return "N/A"; }
        }

        public string MiddleName
        {
            get { return "N/A"; }
        }

        public string Kpp
        {
            get { return "N/A"; }
        }

        public bool IsNull { get { return true; } }
    
    }

    /// <summary>
    /// Реализует контрагента, который содержит данные о всех контрагентах заданного договора
    /// </summary>
    class ContractorAggregator : IContractor
    {
        public Contractdoc Contractdoc { get; private set; }

        public ContractorAggregator(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);
            Contractdoc = contractdoc;
        }

        private string AggregateStringProperties(Func<Contractor, string> selector, string delimeter="; ")
        {
            if (Contractdoc.Contractorcontractdocs.Any(x => x.Contractor != null))
            {
                    var tmp =
                        Contractdoc.Contractorcontractdocs.Where(x => x.Contractor != null).Select(x => selector(x.Contractor));
                    return tmp != null ? tmp.Aggregate((x, y) => x + delimeter + y) : string.Empty;
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Получает имена контрагентов в виде строки, разделённой знаком ";"
        /// </summary>
        public string Name
        {
            get
            {
                return AggregateStringProperties(x => x.ToString());
            }
        }

        /// <summary>
        /// Получает тип первого в списке контрагента 
        /// </summary>
        public Contractortype Contractortype
        {
            get { return Contractdoc.Contractorcontractdocs.FirstOrDefault(x=>x.Contractor!=null).With(x => x.Contractor.Contractortype); }
        }

        public string ContractorFullDesc
        {
            get { return AggregateStringProperties(x => x.ContractorFullDesc); }
        }

        public string Kpp
        {
            get { return AggregateStringProperties(x => x.Kpp); }
        }


        public string Address
        {
            get { return AggregateStringProperties(x => x.Address); }
        }

        public string Bik
        {
            get { return AggregateStringProperties(x => x.Bik); }
        }

        public string Bank
        {
            get { return AggregateStringProperties(x => x.Bank); }
        }

        public string Correspaccount
        {
            get { return AggregateStringProperties(x => x.Correspaccount); }
        }

        public string Account
        {
            get { return AggregateStringProperties(x => x.Account); }
        }

        public IEnumerable<Person> Persons { get { return Contractdoc.Contractorcontractdocs.SelectMany(x => x.Contractor.Persons); }}

        public string Inn
        {
            get { return AggregateStringProperties(x => x.Inn); }
        }

        public string Ogrn
        {
            get { return AggregateStringProperties(x => x.Ogrn); }
        }

        public string Okved
        {
            get { return AggregateStringProperties(x => x.Okved); }
        }

        public string PasportSeries
        {
           get { return AggregateStringProperties(x => x.Pasportseries, " "); }
        }

        public string PasportNum
        {
            get { return AggregateStringProperties(x => x.Pasportnumber); }
        }

        public string FamilyName
        {
            get { return AggregateStringProperties(x => x.Familyname); }
        }

        public string FirstName
        {
            get { return AggregateStringProperties(x => x.Firstname); }
        }

        public string MiddleName
        {
            get { return AggregateStringProperties(x => x.Middlename); }
        }

        public string Zip
        {
            get { return AggregateStringProperties(x => x.Zip); }
        }
    }
}
