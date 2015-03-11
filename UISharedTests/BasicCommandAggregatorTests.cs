using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIShared.ViewModel;

namespace UISharedTests
{
    [TestClass]
    public class BasicCommandAggregatorTests
    {
        [TestMethod]
        public void TestBasicOperations()
        {
            var aggregator = new BasicCommandsAggregatorViewModel<EmptyType>();
            var entitiy = new EmptyType();
            var list = new List<EmptyType>() {new EmptyType()};
            aggregator.LoadEntities = ()=> list;
            aggregator.CreateEntity = () => entitiy;
            aggregator.DeleteEntity = (x) =>
                {
                    Assert.IsNotNull(x);
                    Assert.AreSame(entitiy, x);
                };
            aggregator.UpdateEntity = Assert.IsNotNull;

            Assert.IsNotNull(aggregator.LoadEntities);
            Assert.IsNotNull(aggregator.CreateEntity);
            Assert.IsNotNull(aggregator.DeleteEntity);
            Assert.IsNotNull(aggregator.UpdateEntity);
            Assert.IsNull(aggregator.Selected);
            Assert.IsNotNull(aggregator.Entities);
            Assert.AreEqual(1, aggregator.Entities.Count);

            Assert.IsTrue(aggregator.New.CanExecute(null));
            aggregator.New.Execute(null);
            Assert.AreEqual(2, aggregator.Entities.Count);
            Assert.IsFalse(aggregator.Delete.CanExecute(null));
            Assert.IsFalse(aggregator.Update.CanExecute(null));
            
            aggregator.Selected = entitiy;
            Assert.IsTrue(aggregator.Delete.CanExecute(null));
            Assert.IsTrue(aggregator.Update.CanExecute(null));
            
            aggregator.Update.Execute(null);
            aggregator.Delete.Execute(null);
            Assert.AreEqual(1, aggregator.Entities.Count);
            
            


        }
    }
}
