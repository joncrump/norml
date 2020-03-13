using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Common.Extensions;
using Norml.Common.Helpers;

namespace Norml.Common.Data.Repositories
{
    public class PagingRepositoryHelper<TInterface, TModel> : ReadDatabaseRepositoryBase<TInterface, TModel>
        where TModel : class, TInterface, new()
    {
        protected readonly IBuilder<IDataReader, PagingModel> PagingModelBuilder;
        protected readonly IDataReaderBuilder Builder;

        protected PagingRepositoryHelper(string databaseName, IDatabaseFactory databaseFactory, 
            IMapper mapper, IQueryBuilder queryBuilder, IDataReaderBuilder builder, 
            IBuilderStrategyFactory builderStrategyFactory,
            IBuilder<IDataReader, PagingModel> pagingModelBuilder) 
            : base(databaseName, databaseFactory, mapper, queryBuilder,  
                builderStrategyFactory)
        {
            PagingModelBuilder = Guard.ThrowIfNull("pagingModelBuilder", pagingModelBuilder);
            Builder = Guard.ThrowIfNull("builder", builder);
        }

        public virtual PagingModel GetCriteriaByPage(Expression filterExpression = null, PagingInfo pagingInfo = null, 
            bool includeParameters = true)
        {
            if (pagingInfo.IsNull())
            {
                pagingInfo = new PagingInfo
                {
                    PageNumber = 1,
                    RowsPerPage = 50,
                    SortColumn = "Id",
                    SortOrder = "DESC"
                };
            }

            PagingModel pagingModel = null;
            var models = new List<object>();
            var castExpression = filterExpression as Expression<Func<TModel, bool>>;

            var queryInfo = QueryBuilder.BuildPagedQuery(pagingInfo, castExpression,
                includeParameters: includeParameters);

            Database.CreateCommandText(queryInfo.Query, QueryType.Text)
                .WithParameters(queryInfo.Parameters)
                .ExecuteMultiple(r =>
                {
                    if (pagingModel.IsNull())
                    {
                        pagingModel = PagingModelBuilder.Build(r);
                    }

                    models.Add(Builder.Build<TModel>(r));

                    return pagingModel;
                });

            if (pagingModel.IsNotNull())
            {
                pagingModel.Items = models;
                pagingModel.CurrentPage = pagingInfo.PageNumber;
                pagingModel.RowsPerPage = pagingInfo.RowsPerPage;
            }

            return pagingModel;
        }
    }
}