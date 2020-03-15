using System.Collections.Generic;

namespace Norml.Core.Tests.Common.Helpers
{
    public interface IObjectCreator
    {
        T CreateNew<T>(IDictionary<string, object> parameters = null) where T : class, new();
    }
}
