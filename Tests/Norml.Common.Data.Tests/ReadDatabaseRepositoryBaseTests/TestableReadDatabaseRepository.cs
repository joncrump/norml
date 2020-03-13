using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.Repositories;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Common.Helpers;

namespace Norml.Common.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    public class TestableReadDatabaseRepository : ReadDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableReadDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
