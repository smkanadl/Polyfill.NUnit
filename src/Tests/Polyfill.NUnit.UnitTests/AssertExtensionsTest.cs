namespace NUnit.Framework
{
    [TestFixture]
    public class AssertExtensionsTest
    {
        [Test]
        public void EnterScope()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That((bool?)true, Is.True);
                Assert.That((bool?)false, Is.False);
            }
        }

        [Test]
        public void EnterScope_Throws()
        {
            var e = Assert.Throws<MultipleAssertException>(() =>
            {
                // Simulate failing test constraints
                using (Assert.EnterMultipleScope())
                {
                    Assert.That((bool?)true, Is.False);
                    Assert.That((bool?)false, Is.True);
                }
            });

            Assert.That(e.TestResult.AssertionResults, Has.Count.EqualTo(2));
        }
    }
}
