﻿namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
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
