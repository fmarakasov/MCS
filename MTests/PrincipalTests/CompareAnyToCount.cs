using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests.PrincipalTests
{
    [TestClass]
    public class CompareAnyToCount
    {
        [TestMethod]
        public void CompareDevartLinqAndCount()
        {
            using (var ctx = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                ctx.Context.Log = Console.Out;
                Console.WriteLine(ctx.Context.Contractdocs.Count());
                Console.WriteLine(ctx.Context.Contractdocs.Any());
            }
        }
    }
}
