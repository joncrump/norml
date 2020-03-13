using System;
using System.Collections.Generic;
using System.Data;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Common.Extensions;

namespace Norml.Common.Data.Repositories
{
    public abstract class DatabaseRepositoryBase
    {
        private IDatabaseWrapper _database;
        protected readonly IDatabaseFactory DatabaseFactory;
        private readonly string _databaseName;
        private readonly IBuilderStrategyFactory _builderStrategyFactory;

        public IDatabaseWrapper Database
        {
            get
            {
                if (_database.IsNull())
                {
                    _database = DatabaseFactory.GetDatabase(_databaseName);
                }

                return _database;
            }
            set { _database = value; }
        }

        protected DatabaseRepositoryBase(string databaseName, IDatabaseFactory databaseFactory, 
            IBuilderStrategyFactory builderStrategyFactory)
        {
            _databaseName = Guard.ThrowIfNullOrEmpty("databaseName", databaseName);
            DatabaseFactory = Guard.ThrowIfNull("databaseFactory", databaseFactory);
            _builderStrategyFactory = Guard.ThrowIfNull("builderStrategyFactory", builderStrategyFactory);
        }

        public TValue ExecuteSingle<TValue>(QueryInfo queryInfo, BuildMode buildMode = BuildMode.Single)
            where TValue : class, new()
        {
            Guard.ThrowIfNull("queryInfo", queryInfo);

            var strategy = _builderStrategyFactory.GetStrategy(buildMode);

            var value = Database.CreateCommandText(queryInfo.Query, QueryType.Text)
                .WithParameters(queryInfo.Parameters)
                .ExecuteSingle<TValue>(strategy, queryInfo.TableObjectMappings);

            return value;
        }

        public IEnumerable<TValue> ExecuteMultiple<TValue>(QueryInfo queryInfo,  
            BuildMode buildMode = BuildMode.Single) 
            where TValue : class, new()
        {
            Guard.ThrowIfNull("queryInfo", queryInfo);

            var strategy = _builderStrategyFactory.GetStrategy(buildMode);

            var values = Database.CreateCommandText(queryInfo.Query, QueryType.Text)
                .WithParameters(queryInfo.Parameters)
                .ExecuteMultiple<TValue>(strategy, queryInfo.TableObjectMappings);

            return values;
        }

        public void ExecuteTransform<TValue>(QueryInfo queryInfo, Func<IDataReader, TValue> builderDelegate,
            Action<TValue> transformAction)
        {
            Guard.ThrowIfNull("queryInfo", queryInfo);
            Guard.ThrowIfNull("builderDelegate", builderDelegate);
            Guard.ThrowIfNull("transformAction", transformAction);

            if (queryInfo.Parameters.IsNotNullOrEmpty())
            {
                Database
                    .CreateCommandText(queryInfo.Query, QueryType.Text)
                    .WithParameters(queryInfo.Parameters)
                    .ExecuteTransform(builderDelegate, transformAction);
            }
            else
            {
                Database
                    .CreateCommandText(queryInfo.Query, QueryType.Text)
                    .ExecuteTransform(builderDelegate, transformAction);
            }
        }

        public void ExecuteNonQuery(QueryInfo queryInfo)
        {
            Guard.ThrowIfNull("queryInfo", queryInfo);

            if (queryInfo.Parameters.IsNotNullOrEmpty())
            {
                Database
                    .CreateCommandText(queryInfo.Query, QueryType.Text)
                    .WithParameters(queryInfo.Parameters)
                    .ExecuteNonQuery();
            }
            else
            {
                Database.CreateCommandText(queryInfo.Query, QueryType.Text)
                    .ExecuteNonQuery();
            }
        }
    }
}
