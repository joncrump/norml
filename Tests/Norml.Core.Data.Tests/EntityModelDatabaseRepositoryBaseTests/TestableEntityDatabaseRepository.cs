namespace Norml.Core.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    public class TestableEntityDatabaseRepository : EntityModelDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableEntityDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
