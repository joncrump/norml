using System;

namespace Norml.Common.Helpers
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception, RethrowPolicy policy = RethrowPolicy.Rethrow);
    }
}
