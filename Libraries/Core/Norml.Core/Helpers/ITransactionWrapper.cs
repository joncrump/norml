using System;

namespace Norml.Core.Helpers
{
    public interface ITransactionWrapper : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
