using System;

namespace Norml.Common.Helpers
{
    public interface ITransactionWrapper : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
