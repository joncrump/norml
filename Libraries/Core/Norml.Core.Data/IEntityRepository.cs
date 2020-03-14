using Norml.Common.Data.Repositories;

namespace Norml.Common.Data
{
    public interface IEntityRepository<TModel> : IWriteRepository<TModel>,
        IReadRepository<TModel>
    {
    }
}
