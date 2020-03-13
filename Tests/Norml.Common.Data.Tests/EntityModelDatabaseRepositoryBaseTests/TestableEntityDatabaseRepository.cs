using System.Data;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.Repositories;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Common.Helpers;

namespace Norml.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    public class TestableEntityDatabaseRepository : EntityModelDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableEntityDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
