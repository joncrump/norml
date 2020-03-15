using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Helpers;
using Norml.Tests.Common.Base;


namespace Norml.Common.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheSaveMethod : MockTestBase<TestableWriteDatabaseRepository>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IMapper>()
                .Setup(x => x.Map<TestModel>(It.IsAny<ITestModel>()))
                .Returns(ObjectCreator.CreateNew<TestModel>());

            Mocks.Get<IMapper>()
                .Setup(x => x.Map<TestModel>(It.IsAny<ITestModel>()))
                .Returns(ObjectCreator.CreateNew<TestModel>());

            Mocks.Get<IQueryBuilder>()
                .Setup(x => x.BuildInsertQuery(It.IsAny<TestModel>(), It.IsAny<bool>(),
                    It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());

            Mocks.Get<IQueryBuilder>()
                .Setup(x => x.BuildUpdateQuery(It.IsAny<TestModel>(), It.IsAny<Expression<Func<TestModel, bool>>>(),
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.Save(null, It.IsAny<bool>(),
                        It.IsAny<Action<ITestModel>>(), It.IsAny<Action<ITestModel>>(),
                        It.IsAny<Expression>()))
                .AndVerifyHasParameter("model");
        }

        [Test]
        public void WillInvokeMapper()
        {
            SystemUnderTest.Save(Mock.Of<ITestModel>(), true, null, null, null);

            Mocks.Get<IMapper>()
                .Verify(x => x.Map<TestModel>(It.IsAny<ITestModel>()), 
                    Times.Once);
        }

        [Test]
        public void WillInvokeInsertActionIfIsNew()
        {
            var invoked = false;
            Action<ITestModel> insertAction = null;

            insertAction = t => invoked = true;
            SystemUnderTest.Save(Mock.Of<ITestModel>(), true, insertAction, null, null);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void WillInvokeQueryBuilderInsertQuery()
        {
            SystemUnderTest.Save(Mock.Of<ITestModel>(), true, It.IsAny<Action<ITestModel>>(), 
                It.IsAny<Action<ITestModel>>(), It.IsAny<Expression>());

            Mocks.Get<IQueryBuilder>()
                .Verify(x => x.BuildInsertQuery(It.IsAny<TestModel>(), false, It.IsAny<bool>(), 
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WillInvokeExecuteNonQueryIfModelIsNew()
        {
            throw new NotImplementedException();
            //SystemUnderTest.Save(Mock.Of<ITestModel>(), true, It.IsAny<Action<ITestModel>>(),
            //    It.IsAny<Action<ITestModel>>(), It.IsAny<Expression>());

            //MockDatabase
            //    .Verify(x => x.ExecuteNonQuery(), Times.Once);
        }

        [Test]
        public void WillInvokeUpdateActionIfNotIsNew()
        {
            Action<ITestModel> updateAction = null;
            var invoked = false;
            Expression updateExpression = null;

            updateAction = t => invoked = true;
            updateExpression = Expression.Parameter(typeof (TestModel));
            SystemUnderTest.Save(Mock.Of<ITestModel>(), false, null, updateAction,
                updateExpression);

            Assert.IsTrue(invoked);
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfUpdateExpressionIsNullAndIsNewIsFalse()
        {
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.Save(Mock.Of<ITestModel>(), false, null, null, null))
                .AndVerifyMessageContains("Don't know how to update model.  Update expression is null");
        }

        [Test]
        public void WillInvokeQueryBuilderBuildUpdateQuery()
        {
            Expression updateExpression = null;

            updateExpression = Expression.Parameter(typeof (TestModel));
            SystemUnderTest.Save(Mock.Of<ITestModel>(), false, null, null, updateExpression);

            Mocks.Get<IQueryBuilder>()
                .Verify(x => x.BuildUpdateQuery(It.IsAny<TestModel>(), It.IsAny<Expression<Func<TestModel, bool>>>(), 
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), 
                        Times.Once);
        }
    }
}
