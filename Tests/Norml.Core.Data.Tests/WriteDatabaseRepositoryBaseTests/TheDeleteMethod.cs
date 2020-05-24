using System;
using System.Linq.Expressions;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheDeleteMethod : MockTestBase<TestableWriteDatabaseRepository>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilder>()
                .Setup(x => x.BuildDeleteQuery(It.IsAny<Expression<Func<TestModel, bool>>>(), 
                    It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.Delete(null, It.IsAny<Expression<Func<TestModel, bool>>>()))
                .AndVerifyHasParameter("model");
        }

        [Test]
        public void WillInvokeQueryBuilderDeleteQuery()
        {
            SystemUnderTest.Delete(Mock.Of<ITestModel>());

            Mocks.Get<IQueryBuilder>()
                .Verify(x => x.BuildDeleteQuery<ITestModel>(null, null), Times.Once);
        }
    }
}
