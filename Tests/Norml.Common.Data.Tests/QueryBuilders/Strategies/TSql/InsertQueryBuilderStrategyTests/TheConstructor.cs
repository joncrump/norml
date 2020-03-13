using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.InsertQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<InsertQueryBuilderStrategy>();
        }
    }
}
