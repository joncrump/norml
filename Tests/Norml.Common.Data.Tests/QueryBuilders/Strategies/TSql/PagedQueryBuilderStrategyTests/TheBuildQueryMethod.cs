using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.PagedQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<PagedQueryBuilderStrategy>
    {
        [Test]
        public void WillBuildQuery()
        {
            string expectedQuery = null;

            expectedQuery =
                "SET @sortColumn = LOWER(@sortColumn); SET @sortOrder = LOWER(@sortOrder); WITH SortedItems AS ( SELECT ClientId, FirstName, MiddleName, LastName, ROW_NUMBER() OVER ( ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END ASC  ) AS RowNumber, COUNT(ClientId) OVER() AS TotalRecords FROM dbo.Clients (NOLOCK) WHERE [FirstName] LIKE ('Jo%') ) SELECT ClientId, FirstName, MiddleName, LastName, [TotalRecords], ([TotalRecords] + @rowsPerPage - 1) / @rowsPerPage AS NumberOfPages FROM SortedItems WHERE SortedItems.RowNumber BETWEEN ((@pageNumber - 1) * @rowsPerPage) + 1 AND @rowsPerPage * @pageNumber  AND [FirstName] LIKE ('Jo%') ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END ASC ;";

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ClientDataModel>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Clients",
                    FieldMappings =  new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("ClientId", "@clientId", SqlDbType.UniqueIdentifier, isIdentity:true)},
                            {"FirstName", new FieldParameterMapping("FirstName", "@firstName", SqlDbType.NVarChar)},
                            {"MiddleName", new FieldParameterMapping("MiddleName", "@middleName", SqlDbType.NVarChar)},
                            {"LastName", new FieldParameterMapping("LastName", "@lastName", SqlDbType.NVarChar)}
                        }
                });

            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new QueryContainer("[FirstName] LIKE ('Jo%')"));

            dynamic parameters = new ExpandoObject();
            Expression<Func<ClientDataModel, bool>> predicate = m => m.FirstName.StartsWith("Jo%");

            parameters.PagingInfo = new PagingInfo
            {
                PageNumber = 2,
                RowsPerPage = 50,
                SortColumn = "firstname",
                SortOrder = "desc"
            };

            parameters.Predicate = predicate;
            parameters.IncludeParameters = true;
            parameters.CanDirtyRead = true;
            parameters.DesiredFields = null;

            QueryInfo query = SystemUnderTest.BuildQuery<ClientDataModel>(parameters);

            Asserter.AssertEquality(expectedQuery, query.Query);

            Assert.AreEqual(4, query.Parameters.Count());
        }

        public class ClientDataModel
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }
    }
}
