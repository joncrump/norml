using NUnit.Framework;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.SelectJoinedQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<SelectJoinedQueryBuilderStrategy>();
        }
    }
}
