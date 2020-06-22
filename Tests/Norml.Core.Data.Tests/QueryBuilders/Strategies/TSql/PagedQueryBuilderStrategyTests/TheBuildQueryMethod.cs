using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Moq;
using Norml.Core.Data.Mappings;
using Norml.Core.Data.QueryBuilders.Strategies.TSql;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.QueryBuilders.Strategies.TSql.PagedQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<PagedQueryBuilderStrategy>
    {
        [Test]
        public void WillBuildQuery()
        {
            var expectedQuery =
                "SET @sortColumn = LOWER(@sortColumn); SET @sortOrder = LOWER(@sortOrder); WITH SortedItems AS ( SELECT ClientId, FirstName, MiddleName, LastName, ROW_NUMBER() OVER ( ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN ClientId END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN ClientId END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN MiddleName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN MiddleName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN LastName END ASC  ) AS RowNumber, COUNT() OVER() AS TotalRecords FROM dbo.Clients (NOLOCK) WHERE [FirstName] LIKE ('Jo%') ) SELECT ClientId, FirstName, MiddleName, LastName, [TotalRecords], ([TotalRecords] + @rowsPerPage - 1) / @rowsPerPage AS NumberOfPages FROM SortedItems WHERE SortedItems.RowNumber BETWEEN ((@pageNumber - 1) * @rowsPerPage) + 1 AND @rowsPerPage * @pageNumber  AND [FirstName] LIKE ('Jo%') ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN ClientId END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN ClientId END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN MiddleName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN MiddleName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = '' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = '' THEN LastName END ASC ;";

            var dataMapping = new Mock<IDataMapper>();

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ClientDataModel>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MappingKind>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Clients",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("ClientId", "@clientId", SqlDbType.UniqueIdentifier, isIdentity:true)},
                            {"FirstName", new FieldParameterMapping("FirstName", "@firstName", SqlDbType.NVarChar)},
                            {"MiddleName", new FieldParameterMapping("MiddleName", "@middleName", SqlDbType.NVarChar)},
                            {"LastName", new FieldParameterMapping("LastName", "@lastName", SqlDbType.NVarChar)}
                        }
                });

            dataMapping
                .Setup(x => x.GetMappingFor<ClientDataModel>())
                .Returns(GetTypeMapping);

            Mocks.Get<IConfiguration>()
                .Setup(x => x[Constants.Configuration.MappingKind])
                .Returns(MappingKind.Attribute.ToString);

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(dataMapping.Object);

            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new QueryContainer("[FirstName] LIKE ('Jo%')"));

            Mocks.Get<IConfiguration>()
                .Setup(x => x[Constants.Configuration.MappingKind])
                .Returns("Attribute");

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

        private TypeMapping GetTypeMapping()
        {
            return new TypeMapping
            {
                DataSource = "dbo.Clients",
                PropertyMappings = new List<PropertyMapping>
                {
                    new PropertyMapping
                    {
                        PropertyName = "Id",
                        Field = "ClientId",
                        ParameterName = "@clientId",
                        DatabaseType = SqlDbType.UniqueIdentifier,
                        IsIdentity = true,
                        MappedType = typeof(Guid)
                    },
                    new PropertyMapping
                    {
                        PropertyName = "FirstName",
                        Field = "FirstName",
                        ParameterName = "@firstName",
                        DatabaseType = SqlDbType.NVarChar
                    },
                    new PropertyMapping
                    {
                        PropertyName = "MiddleName",
                        Field = "MiddleName",
                        ParameterName = "@middleName",
                        DatabaseType = SqlDbType.NVarChar
                    },
                    new PropertyMapping
                    {
                        PropertyName = "LastName",
                        Field = "LastName",
                        ParameterName = "@lastName",
                        DatabaseType = SqlDbType.NVarChar
                    }
                }
            };
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
