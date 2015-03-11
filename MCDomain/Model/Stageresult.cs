using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using MCDomain.DataAccess;
using CommonBase;


namespace MCDomain.Model
{
    partial class Stageresult : IDataErrorInfo, ICloneable, ISupportStateApproval, IClonableRecursive
    {
        public override string ToString()
        {
            if (this.Ntpsubview != null)
                return this.Ntpsubview.Name + " " + Name;
            
            if (!string.IsNullOrEmpty(Name))
                    return Name;

            return string.Empty;
        }

        private readonly DataErrorHandlers<Stageresult> _errorHandlers = new DataErrorHandlers<Stageresult>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                           new NtpsubviewDataErrorHandler()
                                                                           //new EconomefficiencytypeDataErrorHandler()
                                                                       };

       
        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Stageresult>
        {

            public string GetError(Stageresult source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Stageresult source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Name))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
                }

                if ((source.Stage != null) && (source.Stage.Schedule != null))
                {
                    foreach (var Stage in source.Stage.Schedule.Stages)
                    {
                        foreach (var result in Stage.Stageresults)
                        {
                            if (result.Name == source.Name && result != source)
                            {
                                handled = true;
                                return "Наименование результата должно быть уникально в рамках календарного плана!";
                            }
                        }
                    }
                }

                handled = true;
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: NtpsubviewDataErrorHandler

        private class NtpsubviewDataErrorHandler : IDataErrorHandler<Stageresult>
        {

            public string GetError(Stageresult source, string propertyName, ref bool handled)
            {
                if (propertyName == "Ntpsubview")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Stageresult source, out bool handled)
            {
                handled = false;
                if (source.Ntpsubview == null)
                {
                    handled = true;
                    return "Подвид НТП не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: EconomefficiencytypeDataErrorHandler

        private class EconomefficiencytypeDataErrorHandler : IDataErrorHandler<Stageresult>
        {

            public string GetError(Stageresult source, string propertyName, ref bool handled)
            {
                if (propertyName == "Economefficiencytype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Stageresult source, out bool handled)
            {
                handled = false;
                if (source.Economefficiencytype == null)
                {
                    handled = true;
                    return "Тип экономической эффективности не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region IEditableObject members

/*
        private Economefficiencytype backuptype;
*/
/*
        private bool _inTx = false;
*/
        
        public void BeginEdit()
        {
          /*  if (!_inTx)
            {
                backuptype = this.Economefficiencytype;
                _inTx = true;
            } */
        }

        public void CancelEdit()
        {
            /*
            if (_inTx)
            {
                this.Economefficiencytype = backuptype;
                _inTx = false;
            }
             */
        }

        public void EndEdit()
        {
            /*
            if (_inTx)
            {
                backuptype = null;
                _inTx = false;
            }
             */
        }

        #endregion
    
        public object Clone()
        {
            Stageresult sr = new Stageresult()
                                {
                                   Economefficiencytype = this.Economefficiencytype,
                                   Name = this.Name,
                                   Ntpsubview = this.Ntpsubview
                                };

            Efparameterstageresult nep;
            foreach (Efparameterstageresult ep in Efparameterstageresults)
            {
                nep = (Efparameterstageresult)ep.Clone();
                nep.Stageresult = sr;
                sr.Efparameterstageresults.Add(nep);
            }

            return sr;
        }

        public object CloneRecursively(object owner, object source)
        {
            Stageresult sr = new Stageresult()
            {

                Economefficiencytype = this.Economefficiencytype,
                Name = this.Name,
                Ntpsubview = this.Ntpsubview
            };
            //repository.SubmitChanges();

            Efparameterstageresult nep;
            foreach (Efparameterstageresult ep in Efparameterstageresults)
            {
                nep = (Efparameterstageresult)ep.CloneRecursively(sr, null);
                sr.Efparameterstageresults.Add(nep);
            }

            return sr;
        }               

        private IBindingList parametersBindingList;
        public IBindingList ParametersBindingList
        {
            get
            {
                if (parametersBindingList == null)
                {
                    parametersBindingList = new BindingList<Efparameterstageresult>();
                    foreach (Efparameterstageresult e in Efparameterstageresults.OrderBy(p => p.Economefficiencyparameter.Name))
                    {
                        parametersBindingList.Add(e);
                    }
      
                }
                return parametersBindingList;
            }
        }

        public void ParametersBindingListChanged()
        {
            SendPropertyChanged("ParametersBindingList");
        }

        public Guid Guid
        {
            get
            {
                if (guid == System.Guid.Empty)
                    guid = System.Guid.NewGuid();

                return guid;
            }
        }

        private Guid guid = Guid.Empty;

        partial void OnCreated()
        {
            Ntpsubviewid = EntityBase.ReservedUndifinedOid;
            Economicefficiencytypeid = EntityBase.ReservedUndifinedOid;
        }


        public byte TypeMask
        {
            get { return 4; }
        }
    }
}
