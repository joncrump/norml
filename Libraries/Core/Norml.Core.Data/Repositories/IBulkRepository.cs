using System.Collections.Generic;

namespace Norml.Common.Data.Repositories
{
    public interface IBulkRepository<in TInterface>
    {
        void InsertBulk(IEnumerable<TInterface> models);
    }
}
