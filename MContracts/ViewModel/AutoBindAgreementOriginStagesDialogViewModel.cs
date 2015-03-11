using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    class AutoBindAgreementOriginStagesDialogViewmodel: ViewModelBase
    {
        private bool _usestagenum;
        public bool UseStagenum
        {
            get { return _usestagenum; }
            set 
            { 
                _usestagenum = value;
                OnPropertyChanged(()=>UseStagenum);
                OnPropertyChanged(() => ConditionSetted);

            }
        }

        private bool _usestagename;
        public bool UseStagename
        {
            get { return _usestagename; }
            set
            {
                _usestagename = value;
                OnPropertyChanged(()=>UseStagename);
                OnPropertyChanged(()=>ConditionSetted);
            }
        }

        private bool _useobjectcode;
        public bool UseObjectcode
        {
            get { return _useobjectcode; }
            set
            {
                _useobjectcode = value;
                OnPropertyChanged(()=>UseObjectcode);
                OnPropertyChanged(()=>ConditionSetted);
            }
        }



        private bool _clearbinding;
        public bool ClearBinding
        {
            get { return _clearbinding; }
            set
            {
                _clearbinding = value;
                OnPropertyChanged(()=>ClearBinding);
                OnPropertyChanged(()=>ConditionSetted);
            }
        }

        public bool ConditionSetted
        {
            get { return UseStagenum || UseStagename || UseObjectcode || ClearBinding; }
        }

        public AutoBindAgreementOriginStagesDialogViewmodel()
        {

        }

    }
}
