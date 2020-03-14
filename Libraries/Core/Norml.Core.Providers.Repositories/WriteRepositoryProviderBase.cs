using System;
using System.Linq.Expressions;
using Norml.Common;


//namespace Norml.Providers.Repositories.Common
//{
//    public abstract class WriteRepositoryProviderBase<TInterface, TRepository> 
//        : ReadRepositoryProviderBase<TInterface, TRepository>
//        where TRepository : IEntityRepository<TInterface>, IPagingRepository
//    {
//        protected WriteRepositoryProviderBase(TRepository repository) 
//            : base(repository)
//        {
//        }

//        public virtual TInterface Save(TInterface model, bool isNew, 
//            Action<TInterface> insertAction = null,
//            Action<TInterface> updateAction = null,
//            Expression updateExpression = null)
//        {
//            Guard.EnsureIsNotNull("model", model);

//            var value = Repository.Save(model, isNew, insertAction, updateAction, updateExpression);

//            return value;
//        }

//        public virtual void Delete(TInterface model)
//        {
//            Guard.EnsureIsNotNull("model", model);

//            Repository.Delete(model);
//        }
//    }
//}