using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using System.Windows.Input;
using UIShared.Commands;
using McUIBase.ViewModel;
using CommonBase;

namespace MContracts.ViewModel
{
    public class EditCatalogViewModel: RepositoryViewModel
    {
        public EditCatalogViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }
        
        public ActionType ActionType
        {
            get;
            set;
        }
        
        private object objectForEdit;
        public object ObjectForEdit
        {
            get
            {
                return this.objectForEdit;
            }
            set
            {
                this.objectForEdit = value;
                OnPropertyChanged("ObjectForEdit");
            }
        }

        private IBindingList economefficiencytypes;
        public IBindingList Economefficiencytypes
        {
            get
            {
                if (economefficiencytypes == null)
                economefficiencytypes = new BindingList<Economefficiencytype>(Repository.Economefficiencytypes);

                return economefficiencytypes;
            }
        }

        private IBindingList economefficiencyparameters;
        public IBindingList Economefficiencyparameters
        {
            get
            {
                if (economefficiencyparameters == null)
                economefficiencyparameters = new BindingList<Economefficiencyparameter>(Repository.Economefficiencyparameters);

                return economefficiencyparameters;
            }
        }
        private IBindingList _authorities;
        public IBindingList Authorities
        {
            get
            {
                if (_authorities == null)
                    _authorities = new BindingList<Authority>(Repository.Authorities);

                return _authorities;
            }
        }

        private IEnumerable<Ntpview> ntpviews;
        public IEnumerable<Ntpview> Ntpviews
        {
            get
            {
                if (ntpviews == null)
                    ntpviews = Repository.Ntpviews;

                return ntpviews;
            }
        }

        private IEnumerable<Funccustomerperson> funccustomerpersons;
        public IEnumerable<Funccustomerperson> Funccustomerpersons
        {
            get
            {
                if (funccustomerpersons == null)
                    funccustomerpersons = Repository.Funccustomerpersons;

                return funccustomerpersons;
            }
        }

        private IEnumerable<Role> roles;
        public IEnumerable<Role> Roles
        {
            get
            {
                if (roles == null)
                    roles = Repository.Roles;

                return roles;
            }
        }

        private IEnumerable<Degree> degrees;
        public IEnumerable<Degree> Degrees
        {
            get
            {
                if (degrees == null)
                    degrees = Repository.Degrees;

                return degrees;
            }
        }

        public IEnumerable<Department> departments;
        public IEnumerable<Department> Departments
        {
            get
            {
                if (departments == null)
                    departments = Repository.Departments;
                return departments;
            }
        }

        private IEnumerable<Quarters> quarters; 
        public IEnumerable<Quarters> Quarters
        {
            get { return quarters ?? (quarters = new List<Quarters>() { 
                CommonBase.Quarters.Первый, 
                CommonBase.Quarters.Второй, 
                CommonBase.Quarters.Третий, 
                CommonBase.Quarters.Четвертый });
            } // здесь надо бы на тип Quarters завязаться
        }

        public IEnumerable<Department> ParentDepartments
        {
            get
            {
                if (ObjectForEdit is Department)
                {
                    return Departments.Where(p => (p != ObjectForEdit)).OrderBy(p => p.Name);
                    
                }
                else return null;

            }
        }


        private IEnumerable<Transferacttype> transferacttypes;
        public IEnumerable<Transferacttype> Transferacttypes
        {
            get
            {
                if (transferacttypes == null)
                    transferacttypes = Repository.Transferacttypes;

                return transferacttypes;
            }
        }

        private IEnumerable<Document> documents;
        public IEnumerable<Document> Documents
        {
            get
            {
                if (documents == null)
                    documents = Repository.Documents;

                return documents;
            }
        }

        private IEnumerable<Contractortype> contractortypes;
        public IEnumerable<Contractortype> Contractortypes
        {
            get
            {
                if (contractortypes == null)
                    contractortypes = Repository.Contractortypes;

                return contractortypes;
            }
        }

        public Contractortype SelectedContractortype
        {
            get { return (ObjectForEdit as Contractor).With(x => x.Contractortype); }
            set
            {
                (ObjectForEdit as Contractor).Do(x => x.Contractortype = value);
                //OnPropertyChanged(() => SelectedContractortype);
            }
        }

        private IEnumerable<Education> educations;
        public IEnumerable<Education> Educations
        {
            get
            {
                return educations ?? Repository.TryGetContext().Educations;
            }
        }
        
        private IEnumerable<Contractor> contractors;
        public IEnumerable<Contractor> Contractors
        {
            get
            {
                if (contractors == null)
                    contractors = Repository.Contractors;

                return contractors;
            }
        }

        private IEnumerable<Property> properties;
        public IEnumerable<Property> Properties
        {
            get
            {
                if (properties == null)
                    properties = Repository.Properties;

                return properties;
            }
        }

        private IEnumerable<Position> positions;
        public IEnumerable<Position> Positions
        {
            get
            {
                if (positions == null)
                    positions = Repository.Positions;

                return positions;
            }
        }




        private IEnumerable<Contractorposition> contractorpositions;
        public IEnumerable<Contractorposition> Contractorpositions
        {
            get
            {
                if (contractorpositions == null)
                    contractorpositions = Repository.Contractorpositions.OrderBy(p => p, new CompareContractorpositions());

                return contractorpositions;
            }
        }

        private IEnumerable<Troublesregistry> troublesregistres;
        public IEnumerable<Troublesregistry> TroublesRegistry
        {
            get
            {
                if (troublesregistres == null)
                {
                    troublesregistres = Repository.TroublesRegistry;
                    OnPropertyChanged("TroublesRegistry");
                }

                return troublesregistres;
            }
        }

        private IEnumerable<Functionalcustomertype> functionalcustomertypes;
        public IEnumerable<Functionalcustomertype> Functionalcustomertypes
        {
            get
            {
                if (functionalcustomertypes == null)
                    functionalcustomertypes = Repository.Functionalcustomertypes.OrderBy(p => p.Name);

                return functionalcustomertypes;
            }
        }

        private IEnumerable<Functionalcustomer> functionalcustomers;
        public IEnumerable<Functionalcustomer> Functionalcustomers
        {
            get
            {
                if (functionalcustomers == null)
                    functionalcustomers = Repository.Functionalcustomers.OrderBy(p => p.Name);

                return functionalcustomers;
            }
        }

        public IEnumerable<Functionalcustomer> ParentFunctionalcustomers
        {
            get
            {
                if (ObjectForEdit is Functionalcustomer) 
                {
                    if ((ObjectForEdit as Functionalcustomer).Contractor != null)
                    {
                        return Functionalcustomers.Where(p => (p.Contractor == (ObjectForEdit as Functionalcustomer).Contractor && p != ObjectForEdit)).OrderBy(p => p.Name);
                    }
                    else
                        return null;
                }
                else return null; 

            }
        }

        private IEnumerable<Person> persons;
        public IEnumerable<Person> Persons
        {
            get
            {
                if (persons == null)
                    persons = Repository.Persons.OrderBy(p => p.GetShortFullNameRev());

                return persons;
            }
        }

        private IEnumerable<Employee> employees;
        public IEnumerable<Employee> Employees
        {
            get
            {
                if (employees == null)
                    employees = Repository.Employees.OrderBy(p => p.GetShortFullNameRev());
                return employees;
            }
        }
        
        private BindingList<Responsiblefororder> _currentresponsibles;

        public BindingList<Responsiblefororder> CurrentResponsibles
        {
            get
            {
                if (_currentresponsibles == null)
                {
                    _currentresponsibles = new BindingList<Responsiblefororder>();
                    foreach (Responsiblefororder r in Repository.Responsiblefororders.Where(p => p.Department == ObjectForEdit as Department).OrderBy(p => p.Employee.GetShortFullNameRev()).ToList())
                    {
                        _currentresponsibles.Add(r);
                    }
                }
                return _currentresponsibles;
            }
        }


        protected override void Save()
        {
 	       
        }

        public void SendPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        protected override bool CanSave()
        {
            return true;
        }

        private ICommand _addresponsiblecommand;
        /// <summary>
        /// Получает команду добавления ответственного по договорам в список
        /// </summary>
        public ICommand AddResponsibleCommand
        {
            get
            {
                return _addresponsiblecommand ??
                       (_addresponsiblecommand = new RelayCommand<Employee>(AddResponsible));
            }
        }

        private ICommand _removeresponsiblecommand;
        /// <summary>
        /// Получает команду удаления функционального заказчика из списка
        /// </summary>
        public ICommand RemoveResponsibleCommand
        {
            get
            {
                if (_removeresponsiblecommand == null)
                    _removeresponsiblecommand =
                        new RelayCommand<Responsiblefororder>(RemoveResponsible);
                return _removeresponsiblecommand;
            }
        }

        private void AddResponsible(Employee responsible)
        {
            
            if (responsible == null) return;
            if (!CurrentResponsibles.Any(x => x.Employee.Id == responsible.Id))
            {
                Responsiblefororder resp = new Responsiblefororder() { Department = (ObjectForEdit as Department), Employee = responsible};
                CurrentResponsibles.Add(resp);
                OnPropertyChanged("CurrentResponsibles");
            }
        }

        private void RemoveResponsible(Responsiblefororder responsible)
        {
            if (responsible == null) return;
            if (CurrentResponsibles.Contains(responsible))
            {
                responsible.Department = null;
                CurrentResponsibles.Remove(responsible);
            }
        }
    }
}
