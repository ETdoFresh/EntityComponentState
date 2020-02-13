using ConsoleApp1;
using EntityComponentState;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class ComponentTests
    {
        [TestMethod]
        public void TwoPositionComponentsWithSameValuesNotAttachedToEntitiesAreEqual()
        {
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(1, 2, 3);
            Assert.IsTrue(positionComponent1 == positionComponent2);
        }

        [TestMethod]
        public void TwoPositionComponentsWithDifferentValuesNotAttachedToEntitiesAreEqual()
        {
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(4, 5, 6);
            Assert.IsTrue(positionComponent1 != positionComponent2);
        }

        [TestMethod]
        public void TwoPositionComponentsWithSameValuesOnDifferentEntitiesAreNotEqual()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(1, 2, 3);
            entity1.AddComponent(positionComponent1);
            entity2.AddComponent(positionComponent2);
            Assert.IsTrue(positionComponent1 != positionComponent2);
        }

        [TestMethod]
        public void TwoPositionComponentsWithSameValuesOnSameEntityAreEqual()
        {
            var entity1 = new Entity();
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(1, 2, 3);
            entity1.AddComponent(positionComponent1);
            entity1.AddComponent(positionComponent2);
            Assert.IsTrue(positionComponent1 == positionComponent2);
        }

        [TestMethod]
        public void TwoPositionComponentsWithDifferentValuesOnSameEntityAreNotEqual()
        {
            var entity1 = new Entity();
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(4, 5, 6);
            entity1.AddComponent(positionComponent1);
            entity1.AddComponent(positionComponent2);
            Assert.IsTrue(positionComponent1 != positionComponent2);
        }

        [TestMethod]
        public void TwoPositionComponentsWithDifferentValuesOnDifferentEntitiesAreNotEqual()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var positionComponent1 = new Position(1, 2, 3);
            var positionComponent2 = new Position(4, 5, 6);
            entity1.AddComponent(positionComponent1);
            entity2.AddComponent(positionComponent2);
            Assert.IsTrue(positionComponent1 != positionComponent2);
        }

        [TestMethod]
        public void ClonedComponentsAreEqual()
        {
            var entity1 = new Entity();
            var positionComponent1 = new Position(1, 2, 3);
            var rotationComponent1 = new Rotation();
            entity1.AddComponent(positionComponent1, rotationComponent1);

            var positionComponent2 = positionComponent1.Clone();
            var rotationComponent2 = rotationComponent1.Clone();
            Assert.IsTrue(positionComponent1 == positionComponent2);
            Assert.IsTrue(rotationComponent1 == rotationComponent2);
        }
    }
}
