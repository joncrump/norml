using System;
using System.Linq.Expressions;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Common.Extensions;
using Norml.Common.Helpers;

namespace Norml.Common.Data.Repositories
{
    public abstract class EntityModelDatabaseRepositoryBase<TInterface, TModel> :
            WriteDatabaseRepositoryBase<TInterface, TModel>
        where TModel : class, TInterface, new()
        where TInterface : IModel
    {
        protected EntityModelDatabaseRepositoryBase(
            string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, 
            IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) 
            : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }

        public override TInterface Save(TInterface model, bool isNew, 
// ReSharper disable OptionalParameterHierarchyMismatch
            Action<TInterface> insertAction = null, 
            Action<TInterface> updateAction = null, 
            Expression updateExpression = null
            // ReSharper restore OptionalParameterHierarchyMismatch
            )
        {
            Guard.ThrowIfNull("model", model);

            var mappedModel = Mapper.Map<TModel>(model);

            if (insertAction.IsNotNull() && updateAction.IsNotNull() && updateExpression.IsNotNull())
            {
                return base.Save(mappedModel, isNew, insertAction, updateAction, updateExpression);
            }

            Expression<Func<TModel, bool>> filterExpression = m => m.Id == mappedModel.Id;

            return base.Save(model, model.IsNew(), m =>
            {
                m.Id = Guid.NewGuid();
                m.EnteredDate = DateTime.Now;
                m.UpdatedDate = DateTime.Now;
            }, m =>
            {
                m.UpdatedDate = DateTime.Now;
            }, filterExpression);
        }

        public void Delete(TInterface model)
        {
            Guard.ThrowIfNull("model", model);

            Delete(model, m => m.Id == model.Id);
        }
    }
}
