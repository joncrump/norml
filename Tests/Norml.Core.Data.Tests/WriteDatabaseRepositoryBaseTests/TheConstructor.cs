namespace Norml.Core.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<TestableWriteDatabaseRepository>();
        }
    }
}
