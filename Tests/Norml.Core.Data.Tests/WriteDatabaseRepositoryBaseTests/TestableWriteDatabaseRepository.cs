namespace Norml.Core.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    public class TestableWriteDatabaseRepository : WriteDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableWriteDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
