using Norml.Core.Data.Repositories;
using Norml.Core.Data.Repositories.Strategies;

namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
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
