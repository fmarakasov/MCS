using System;
using Devart.Data.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    internal class LinqTestUtilities
    {
        public static void PrintChangeSet(McDataContext ctx)
        {
            Printcs(ctx.GetChangeSet());
        }

        public static void AssertChangeset(McDataContext ctx, int inserts, int updates, int deletes)
        {
            ChangeSet cs = ctx.GetChangeSet();            
            
            Assert.AreEqual(inserts, cs.Inserts.Count, "Ожидалось другое число вставок.");
            Assert.AreEqual(updates, cs.Updates.Count, "Ожидалось другое число обновлений.");
            Assert.AreEqual(deletes, cs.Deletes.Count, "Ожидалось другое число удалений.");
        }

        private static void Printcs(ChangeSet cs)
        {
            Console.WriteLine("--------Inserts----------------");
            Printcs(cs.Inserts);
            Console.WriteLine("--------Updates----------------");
            Printcs(cs.Updates);
            Console.WriteLine("--------Deletes----------------");
            Printcs(cs.Deletes);
        }

        private static void Printcs(System.Collections.IList iList)
        {
            foreach (var item in iList)
            {
                Console.WriteLine("Item.ToString(): " + item.ToString());
                var pi = item.GetType().GetProperties();
                foreach (var propertyInfo in pi)
                {
                    object value = null;
                    try
                    {
                        value = propertyInfo.GetValue(item, null);
                    }
                    catch (Exception e)
                    {

                        value = "Exception: " + e.Message;
                    }
                    Console.WriteLine("\t{0}: {1}", propertyInfo.Name, value);
                }
            }
        }
    }
}