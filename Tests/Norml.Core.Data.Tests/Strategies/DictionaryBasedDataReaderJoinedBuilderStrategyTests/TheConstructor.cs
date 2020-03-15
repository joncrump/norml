using NUnit.Framework;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.Strategies.DictionaryBasedDataReaderJoinedBuilderStrategyTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<DictionaryBasedDataReaderJoinedBuilderStrategy>();
        }
    }
}
