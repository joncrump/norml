using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.DatabaseRepositoryBaseTests
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
