using EntityComponentState;
using NUnit.Framework;

namespace Tests
{
    public class Vector3Tests
    {
        [Test]
        public void TwoSeperateInstancesWithSameValuesAreEqual()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(1, 2, 3);
            Assert.IsTrue(v1 == v2);
        }

        [Test]
        public void PassByValueTest()
        {
            var v1 = new Vector3(1, 2, 3);
            var oldV1 = v1; // Should pass by value (and not reference)

            var v2 = new Vector3(2, 3, 4);
            
            v1.x = 2; v1.y = 3; v1.z = 4;
            
            Assert.IsTrue(v1 == v2);
            Assert.IsTrue(oldV1 != v2); // These will be equal if pass by reference
        }
    }
}
