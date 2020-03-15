using Norml.Core.Data.Repositories;

namespace Norml.Core.Data
{
    public interface IEntityRepository<TModel> : IWriteRepository<TModel>,
        IReadRepository<TModel>
    {
    }
}
