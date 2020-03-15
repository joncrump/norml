namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<TestableDatabaseRepository>();
        }
    }
}
