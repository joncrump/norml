using System;

namespace Norml.Core.Helpers
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception, RethrowPolicy policy = RethrowPolicy.Rethrow);
    }
}
