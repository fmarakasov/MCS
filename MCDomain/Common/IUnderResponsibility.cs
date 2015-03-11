using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using MCDomain.Model;
using System.ComponentModel;

namespace MCDomain.Common
{
    /// <summary>
    /// Определяет типы с которыми могут быть сопоставлены ответственные лица из организации.
    /// </summary>
    public interface IUnderResponsibility
    {
        /// <summary>
        /// ответственные по договору или этапу для связывания
        /// </summary>
        IBindingList ResponsiblesBindingList { get; }
        /// <summary>
        /// ответственные по договору или этапу
        /// </summary>
        EntitySet<Responsible> Responsibles { get;  }

        void RefreshRespBindingList();

        void RemoveResponsiblesForDisposal(Disposal disposal);
        void SendResponsiblesBindingListChanged();
        
        /// <summary>
        /// строка ответственных для отчета
        /// </summary>
        /// <returns></returns>
        string GetResponsibleNameForReports();

        /// <summary>
        /// распоряжение по данному договору или этапу
        /// </summary>
        Disposal Disposal { get; }

        /// <summary>
        /// отдел, за которым договор или этап закреплен по распоряжению
        /// </summary>
        Department DisposalDepartment { get;  }


        /// <summary>
        /// Возвращает имена зам. директора и руководителя напрвления и руководителей работ
        /// </summary>
        string DirectorsAndChiefs(bool includeordersuperviser);

        /// <summary>
        /// возвращает имена зам директора и руководителя направления
        /// </summary>
        /// <param name="includeordersuperviser"></param>
        /// <returns></returns>
        string Directors { get; }
        /// <summary>
        /// руководитель по договору или этапу (промгаз)
        /// </summary>
        Responsible Chief { get;  }
        /// <summary>
        /// сотрудник - руководитель по договору или этапу 
        /// </summary>
        Employee ChiefEmployee { get;  }

        /// <summary>
        /// для случаев с несколькими руководителями - отдельное свойство
        /// </summary>
        IEnumerable<Responsible> Chiefs { get;  }

        /// <summary>
        /// руководитель направления - ответственный
        /// </summary>
        Responsible Manager { get;  }

        /// <summary>
        /// руководитель направления - служащий
        /// </summary>
        Employee ManagerEmployee { get;  }

        /// <summary>
        /// замдир - ответственный
        /// </summary>
        Responsible Director { get; }

        /// <summary>
        /// замдир - служащийр
        /// </summary>
        Employee DirectorEmployee { get;  }

        /// <summary>
        /// ответственный по договорам
        /// </summary>
        Responsible Curator { get;  }

        /// <summary>
        /// ответственный по договорам, служащий
        /// </summary>
        Employee CuratorEmployee { get;  }


        /// <summary>
        /// ответственный по договорам
        /// </summary>
        Responsible OrderSuperviser { get;  }

        /// <summary>
        /// ответственный по договорам - служащий
        /// </summary>
        Employee OrderSuperviserEmployee { get;  }
        
        /// <summary>
        /// текстовая строка со всеми ответственными во всех ролях по договору или этапу
        /// </summary>
        string DisposalPersons { get;  }

        /// <summary>
        /// если руководитель направления совпадает с руководителем темы, то его не надо показывать
        /// </summary>
        bool IsManagerVisible { get; }
    }
}
