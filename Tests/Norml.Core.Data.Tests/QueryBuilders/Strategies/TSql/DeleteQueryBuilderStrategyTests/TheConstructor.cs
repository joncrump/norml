﻿using Norml.Core.Data.QueryBuilders.Strategies.TSql;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.QueryBuilders.Strategies.TSql.DeleteQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<DeleteQueryBuilderStrategy>();
        }
    }
}