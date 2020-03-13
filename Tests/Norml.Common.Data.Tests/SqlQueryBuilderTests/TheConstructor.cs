﻿using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<SqlQueryBuilder>();
        }
    }
}
