using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;

namespace MTests
{
    [TestClass]
    public class NtpsubviewTest
    {
        [TestMethod]
        public void InsertSubviewTest()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                //Ntpsubview item = new Ntpsubview()
                //                      {
                //                          Name = "Новый элемент",
                //                          Shortname = "НЭ"
                //                      };

                //IBindingList list = new BindingList<Ntpview>(repository.Ntpviews);

                //item.Ntpview = list[2] as Ntpview;
                //repository.InsertNtpsubview(item);
              
                //LinqTestUtilities.PrintChangeSet(repository.Context);

                //LinqTestUtilities.AssertChangeset(repository.Context, 1, 1, 0); 
                
                //repository.SubmitChanges();

                //repository.DeleteNtpsubview(item);
                //repository.SubmitChanges();
            }

        }
    }
}
