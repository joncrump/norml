using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.Tests.ReadDatabaseRepositoryBaseTests;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheGetMethod : MockTestBase<TestableReadDatabaseRepository>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilder>()
                .Setup(x => x.BuildSelectQuery(It.IsAny<Expression<Func<TestModel, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<BuildMode>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [Test]
        public void WillInvokeQueryBuilderBuildSelectQuery()
        {
            SystemUnderTest.Get();

            Mocks.Get<IQueryBuilder>()
                .Verify(x => x.BuildSelectQuery(It.IsAny<Expression<Func<TestModel, bool>>>(), 
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), 
                    It.IsAny<BuildMode>()), Times.Once);
        }
    }
}
