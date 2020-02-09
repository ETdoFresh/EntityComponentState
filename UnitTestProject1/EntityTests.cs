using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void FirstEntityIDEquals0()
        {
            IDAssignment.Clear();
            var entity1 = new Entity();
            Assert.IsTrue(entity1.id == 0);
        }

        [TestMethod]
        public void SecondEntityIDEquals1()
        {
            IDAssignment.Clear();
            var entity1 = new Entity();
            var entity2 = new Entity();
            Assert.IsTrue(entity2.id == 1);
        }

        [TestMethod]
        public void ThirdEntityIDEquals2()
        {
            IDAssignment.Clear();
            var entity1 = new Entity();
            var entity2 = new Entity();
            var entity3 = new Entity();
            Assert.IsTrue(entity3.id == 2);
        }

        [TestMethod]
        public void FourthEntityIDEquals1AfterSecondEntityDestroyed()
        {
            IDAssignment.Clear();
            var entity1 = new Entity();
            var entity2 = new Entity();
            var entity3 = new Entity();
            entity2.Destroy();
            var entity4 = new Entity();
            Assert.IsTrue(entity4.id == 1);
        }

        [TestMethod]
        public void AddNullComponentIsException()
        {
            IDAssignment.Clear();
            var entity = new Entity();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entity.AddComponents(null);
            });
        }

        [TestMethod]
        public void AddComponentAddsComponentToList()
        {
            IDAssignment.Clear();
            var entity = new Entity();
            entity.AddComponents(new Position());
            Assert.IsTrue(entity.components.Count == 1);
            Assert.IsTrue(entity.components[0] is Position);
        }
    }
}
