using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class ObjectIdEntityBase : EntityBase, IObjectId
    {
        private long _id;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    [TestClass]
    public class EntityBaseTest
    {
        [TestMethod]
        public void ReservedAsUndefinedForNonIdObjects()
        {
            EntityBase entityBase = new EntityBase();
            Assert.IsFalse(entityBase.ReservedAsUndefined);

        }

        [TestMethod]
        public void ReservedAsUndefinedForIdObjectsShouldReturnTrue()
        {
            EntityBase entityBase = new ObjectIdEntityBase(){Id = -1};
            Assert.IsTrue(entityBase.ReservedAsUndefined);
        }

        [TestMethod]
        public void ReservedAsUndefinedForIdObjectsShouldReturnFalse()
        {
            EntityBase entityBase = new ObjectIdEntityBase() { Id = 1 };
            Assert.IsFalse(entityBase.ReservedAsUndefined);

        }
    }
}
