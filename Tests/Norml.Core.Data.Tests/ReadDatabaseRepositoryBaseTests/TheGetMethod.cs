using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.ReadDatabaseRepositoryBaseTests
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
