using Norml.Common.Data.Repositories;
using Norml.Common.Data.Repositories.Strategies;

namespace Norml.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    public class TestableDatabaseRepository : DatabaseRepositoryBase
    {
        public TestableDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, 
            IBuilderStrategyFactory builderDelegateStrategyFactory) 
            : base(databaseName, databaseFactory, builderDelegateStrategyFactory)
        {
        }
    }
}
