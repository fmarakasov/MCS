using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Devart.Data.Linq;

namespace MCDomain.DataAccess
{
    public static class DataContextDebug
    {
        public static string GetDebugString(this Model.McDataContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return GetDebugString(context.GetChangeSet());
        }

        public static string GetDebugString(ChangeSet changeSet)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--------Inserts----------------");
            AppendChangesets(sb, changeSet.Inserts);
                 sb.AppendLine("--------Updates----------------");
            AppendChangesets(sb, changeSet.Updates);
            sb.AppendLine("--------Deletes----------------");
            AppendChangesets(sb, changeSet.Deletes);
            return sb.ToString();

        }

        private static void AppendChangesets(StringBuilder sb, System.Collections.IList list)
        {
            foreach (var item in list)
            {
                sb.AppendLine(string.Format("Item.GetType(): {0} Item.ToString(): {1}", item.GetType(),
                                              item));
            }
            
         }

        private static void Printcs(ChangeSet cs)
        {
            Debug.WriteLine(GetDebugString(cs));         
        }

     
        /// <summary>
        /// Распечатать данные о контексте в консоль отладки
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="message">дополнительное сообщение в окно отладки</param>
        public static void DebugPrintRepository<T>(this T repository, string message="") where T : class, IContractRepository
        {
            if (repository == null) throw new ArgumentNullException("repository");
            DebugPrintRepository(repository.TryGetContext().GetChangeSet(), message);
        }

        public static void DebugPrintRepository(this ChangeSet changeSet, string message="")  
        {
            Contract.Requires(changeSet != null);
#if DEBUG

            var cs = changeSet;
                Debug.WriteLine("======================================================");
                if (message != "")
                    Debug.WriteLine(message);

                Debug.WriteLine(cs);
                Printcs(cs);
            

#endif
        }
    }
}
