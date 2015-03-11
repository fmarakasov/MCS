using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using UIShared.Commands;
using McUIBased.Commands;

namespace MContracts.ViewModel
{
    public class HierarchicalCatalogViewModel : CatalogViewModel
    {
        private RelayCommand outdentCommand;
        private RelayCommand indentCommand;
        private RelayCommand upCommand;
        private RelayCommand downCommand;
        
        public HierarchicalCatalogViewModel(IContractRepository repository)
            : base(repository)
        {

        }

        public ICommand OutdentCommand
        {
            get
            {
                return outdentCommand ??
                       (outdentCommand = new RelayCommand(Outdent, x => CanOutdent));
            }
        }

        public ICommand IndentCommand
        {
            get
            {
                return indentCommand ??
                       (indentCommand = new RelayCommand(Indent, x => CanIndent));
            }
        }

        public ICommand UpCommand
        {
            get
            {
                return upCommand ??
                       (upCommand = new RelayCommand(Up, x => CanUp));
            }
        }

        public ICommand DownCommand
        {
            get
            {
                return downCommand ??
                       (downCommand = new RelayCommand(Down, x => CanDown));
            }
        }

        private void Outdent(object o)
        {
            var index = Objects.IndexOf(SelectedItem);

            var parent = Objects.Where<IHierarchical>(x => ((SelectedItem as IHierarchical).Level - (x as IHierarchical).Level == 0) && (index > Objects.IndexOf(x as IHierarchical))).LastOrDefault();
            (SelectedItem as IHierarchical).Parent = parent;

            OnPropertyChanged("IsModified");
        }

        private void Indent(object o)
        {
            int index = Objects.IndexOf(SelectedItem);

            var parent = Objects.Where<IHierarchical>(x => ((SelectedItem as IHierarchical).Level - (x as IHierarchical).Level == 2 || (SelectedItem as IHierarchical).Level - (x as IHierarchical).Level == 0) && (index > Objects.IndexOf(x as IHierarchical))).LastOrDefault();
            (SelectedItem as IHierarchical).Parent = parent;

            OnPropertyChanged("IsModified");
        }

        public bool CanOutdent
        {
            get
            {
                if (Objects != null)
                {
                    int index = Objects.IndexOf(SelectedItem);
                    if (index == -1 || index == 0 || (SelectedItem as IHierarchical).Level - (Objects[index - 1] as IHierarchical).Level >= 1)
                    {
                        return false;
                    }

                    return true;
                }
                
                return false;
            }
        }

        public bool CanIndent
        {
            get
            {
                if (Objects != null)
                {
                    int index = Objects.IndexOf(SelectedItem);
                    if (index == -1 || index == 0 || (Objects[index - 1] as IHierarchical).Level > (SelectedItem as IHierarchical).Level || (SelectedItem as IHierarchical).Level == 0)
                    {
                        return false;
                    }

                    return true;
                }
                
                return false;
            }
        }

        private void Up(object o)
        {
            int index = Objects.IndexOf(SelectedItem);
            Objects.Move(index, index - 1);

            OnPropertyChanged("IsModified");
            OnPropertyChanged("CollectionView");
        }

        private void Down(object o)
        {
            int index = Objects.IndexOf(SelectedItem);
            Objects.Move(index, index + 1);

            OnPropertyChanged("IsModified");
            OnPropertyChanged("CollectionView");
        }

        public bool CanUp
        {
            get
            {
                if (Objects != null)
                {
                    int index = Objects.IndexOf(SelectedItem);
                    if (index == -1 || index == 0 || (Objects[index - 1] as IHierarchical).Level != (SelectedItem as IHierarchical).Level)
                    {
                        return false;
                    }

                    return SelectedItem != null && index >= (SelectedItem as IHierarchical).Level;
                }
                
                return false;
            }
        }

        public bool CanDown
        {
            get
            {
                if (Objects != null)
                {
                    int index = Objects.IndexOf(SelectedItem);
                    if (index == -1 
                        || index == Objects.Count - 1 
                        || (Objects[index + 1] as IHierarchical).Level != (SelectedItem as IHierarchical).Level 
                        //|| (Objects[index + 2] as IHierarchical).Level != (SelectedItem as IHierarchical).Level
                        )
                    {
                        return false;
                    }

                    return true;
                }
                
                return false;
            }
        }

    }
}
