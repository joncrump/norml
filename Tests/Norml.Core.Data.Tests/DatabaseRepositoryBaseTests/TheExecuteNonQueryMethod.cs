using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteNonQueryMethod : MockTestBase<TestableDatabaseRepository>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfQueryInfoIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteNonQuery(null))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            SystemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text), 
//                    Times.Once);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreNull()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo { Parameters = null };

            SystemUnderTest.ExecuteNonQuery(queryInfo);

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

            SystemUnderTest.ExecuteNonQuery(queryInfo);

            throw new NotImplementedException();
//                MockDatabase
//                    .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
//                        Times.Never);
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

            SystemUnderTest.ExecuteNonQuery(queryInfo);

//            MockDatabase
//                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
//                    Times.Once);
        }

        [Test]
        public void WillInvokeDatabaseExecuteNonQueryMethod()
        {
            SystemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

            throw new NotImplementedException();
            
//            MockDatabase
//                .Verify(x => x.ExecuteNonQuery(), Times.Once);
        }
    }
}
