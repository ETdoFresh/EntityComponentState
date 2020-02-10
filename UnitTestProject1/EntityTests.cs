using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#pragma warning disable IDE0059 // Unnecessary assignment of a value [Supressing in the name of human readability]
namespace UnitTestProject1
{
    [TestClass]
    public class EntityTests
    {
        [TestInitialize]
        public void OnTestStart()
        {
            IDAssignment.Clear();
        }

        [TestMethod]
        public void FirstEntityIDEquals0()
        {
            var entity1 = new Entity();
            Assert.IsTrue(entity1.id == 0);
        }

        [TestMethod]
        public void SecondEntityIDEquals1()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            Assert.IsTrue(entity2.id == 1);
        }

        [TestMethod]
        public void ThirdEntityIDEquals2()
        {

            var entity1 = new Entity();

            var entity2 = new Entity();
            var entity3 = new Entity();
            Assert.IsTrue(entity3.id == 2);
        }

        [TestMethod]
        public void FourthEntityIDEquals1AfterSecondEntityDestroyed()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var entity3 = new Entity();
            entity2.Destroy();
            var entity4 = new Entity();
            Assert.IsTrue(entity4.id == 1);
        }

        [TestMethod]
        public void ThrowExceptionIfAddComponentNull()
        {
            var entity = new Entity();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entity.AddComponents(null);
            });
        }

        [TestMethod]
        public void ThrowExceptionIfAddComponentMixComponentAndNull()
        {
            var entity = new Entity();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entity.AddComponents(new Position(), null);
            });
        }

        [TestMethod]
        public void AddComponentAddsComponentToList()
        {
            var entity = new Entity();
            entity.AddComponents(new Position());
            Assert.IsTrue(entity.components.Count == 1);
            Assert.IsTrue(entity.components[0] is Position);
        }

        [TestMethod]
        public void TwoEntitiesAreDifferent()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            Assert.IsTrue(entity1 != entity2);
        }

        [TestMethod]
        public void OneEntityWithDifferentReferencesAreEqual()
        {
            var entity1 = new Entity();
            var entity2 = entity1;
            Assert.IsTrue(entity1 == entity2);
        }

        [TestMethod]
        public void OneEntityClonedIsEqualToOriginal()
        {
            var entity1 = new Entity();
            var entity2 = entity1.Clone();
            Assert.IsTrue(entity1 == entity2);
        }

        [TestMethod]
        public void OneEntityClonedWithOneDifferentButEqualComponentIsEqual()
        {
            var entity1 = new Entity();
            var entity2 = entity1.Clone();
            var component1 = new Position();
            var component2 = new Position();
            entity1.AddComponents(component1);
            entity2.AddComponents(component2);

        }
    }
}
#pragma warning restore IDE0059 // Unnecessary assignment of a value