using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.Repositories;
using Norml.Core.Data.Repositories.Strategies;
using Norml.Core.Helpers;

namespace Norml.Core.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    public class TestableEntityDatabaseRepository : EntityModelDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableEntityDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
