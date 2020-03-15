namespace Norml.Core.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<TestableReadDatabaseRepository>();
        }
    }
}
