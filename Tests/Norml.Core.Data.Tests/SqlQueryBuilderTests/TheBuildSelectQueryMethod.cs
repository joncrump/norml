﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.QueryBuilders.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildSelectQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            var factory = Mocks.Get<IQueryBuilderStrategyFactory>();

            factory
                .Setup(x => x.GetBuilderStrategy(It.IsAny<QueryKind>()))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [Test]
        public void WillInvokeBuilderStrategyIfBuildModeIsSingle()
        {
            SystemUnderTest.BuildSelectQuery(It.IsAny<Expression<Func<TestClass, bool>>>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
// ReSharper disable once RedundantArgumentDefaultValue
                It.IsAny<string>(), BuildMode.Single);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), 
                    Times.Once);
        }

        [Test]
        public void WillInvokeBuilderStrategyIfBuildModeIsJoined()
        {
            SystemUnderTest.BuildSelectQuery(It.IsAny<Expression<Func<TestClass, bool>>>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>(), BuildMode.Joined);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectJoinTable),
                    Times.Once);
        }
    }
}
