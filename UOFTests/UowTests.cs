using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using UOW;


namespace UOWTests
{
    [TestClass]
    public class UowTests
    {
        [TestMethod]
        public void Test()
        {
            //Math.Log(1, 2)
            //for (int i = 'a'; i <='z'; ++i) Console.WriteLine("{0}:{1}", i, (char)i);
            //var s = "xyz";
            //Console.WriteLine(s.Aggregate(new StringBuilder(), (x, c) => x.Append((char)('a'+((c + 5 - 'a') % ('z' - 'a' + 1)))), sb => sb.ToString()));                       
        }
     
        private IKernel _kernel;
        
        [TestInitialize]
        public void Initialize()
        {
            _kernel = new StandardKernel();

            var ctx = new Mock<IDataContext>();
            ctx.Setup(x => x.AsQueryable<object>())
                .Returns(() => Enumerable.Range(1, 10).
                    Cast<object>().AsQueryable());

            _kernel.Bind<IRepository<object>>().To<Repository<object>>();
            _kernel.Bind<IDataContext>().ToConstant(ctx.Object);
            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            
            var repFactory = new Mock<IRepositoryFactory>();
            repFactory.Setup(x => x.CreateRepository<object>(
                ctx.Object)).Returns(_kernel.Get<IRepository<object>>());
            
            _kernel.Bind<IRepositoryFactory>().ToConstant(repFactory.Object);
            
        }

        [TestMethod]
        public void Test_UOF_Repository()
        {
            var uof = _kernel.Get<IUnitOfWork>();
            var rep1 = uof.Repository<object>();
            var rep2 = uof.Repository<object>();
            Assert.IsNotNull(rep1);
            Assert.AreEqual(rep1, rep2);
        }

        [TestMethod]
        public void Test_Repository_AsQueryble()
        {
            var rep = _kernel.Get<IRepository<object>>();
            var result = rep.AsQueryable().ToArray();
            Assert.AreEqual(10, result.Length);                   
        }

        [TestMethod]
        public void Test_DataContext_AsQueryble()
        {
            var ctx = _kernel.Get<IDataContext>();
            var result = ctx.AsQueryable<object>().ToArray();
            Assert.AreEqual(10, result.Length);    
            
        }
    }
}
