﻿using System.Collections.Generic;

namespace Norml.Core.Data.Repositories
{
    public interface IBulkRepository<in TInterface>
    {
        void InsertBulk(IEnumerable<TInterface> models);
    }
}
