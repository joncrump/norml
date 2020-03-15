using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Moq;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteTransformMethod : MockTestBase<TestableDatabaseRepository>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfQueryInfoIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () =>
                        SystemUnderTest.ExecuteTransform(null, It.IsAny<Func<IDataReader, object>>(),
                            It.IsAny<Action<object>>()))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfBuilderDelegateIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), null,
                        It.IsAny<Action<object>>()))
                .AndVerifyHasParameter("builderDelegate");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfTransformActionIsNull()
        {
            Func<IDataReader, object> builderDelegate = r => r;

            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate,
                        null))
                .AndVerifyHasParameter("transformAction");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate, 
                transformAction);

            throw new NotImplementedException();
            
//            MockDatabase
//                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text), 
//                    Times.Once);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreNull()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo {Parameters = null};

            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
//                    Times.Never);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreEmpty()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo
            {
                Parameters = Enumerable.Empty<IDbDataParameter>()
            };

            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

            throw new NotImplementedException();
            
//            MockDatabase
//                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
//                    Times.Never);
        }

        [Test]
        public void WillInvokeDatabaseWithParametersMethodIfParametersHaveValues()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter()
                }
            };

            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

            throw new NotImplementedException();
            
//            MockDatabase
//                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
//                    Times.Once);
        }

        [Test]
        public void WillInvokeDatabaseExecuteTransformMethod()
        {
            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), 
                builderDelegate, transformAction);

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.ExecuteTransform(It.IsAny<Func<IDataReader, object>>(), 
//                    It.IsAny<Action<object>>()), Times.Once);
        }

        private void FakeMethod(object value)
        {
        }
    }
}
