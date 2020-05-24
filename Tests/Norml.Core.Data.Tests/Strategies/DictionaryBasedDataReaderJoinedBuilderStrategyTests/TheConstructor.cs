using Norml.Core.Data.Repositories.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.Strategies.DictionaryBasedDataReaderJoinedBuilderStrategyTests
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
