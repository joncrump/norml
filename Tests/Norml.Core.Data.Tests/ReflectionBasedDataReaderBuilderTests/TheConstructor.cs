namespace Norml.Core.Data.Tests.ReflectionBasedDataReaderBuilderTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<ReflectionBasedDataReaderBuilder>();
        }
    }
}
