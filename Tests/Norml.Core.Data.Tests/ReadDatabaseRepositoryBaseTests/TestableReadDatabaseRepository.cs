namespace Norml.Core.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    public class TestableReadDatabaseRepository : ReadDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableReadDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
