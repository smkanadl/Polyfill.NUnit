namespace NUnit.Framework
{
    [TestFixture]
    public class IsExtensionsTest
    {
        [Test]
        public void EqualTo_GenericOverloadForEnumerables()
        {
            var data = new[] { 1, 2, 3 };
            Assert.That(
                data,
                Is.EqualTo([1, 2, 3])
            );
        }

        [Test]
        public void EquivalentTo_GenericOverloadForEnumerables()
        {
            var data = new[] { 1, 2, 3 };
            Assert.That(
                data,
                Is.EquivalentTo([3, 2, 1])
            );
        }
    }
}
