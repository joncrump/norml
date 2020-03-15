using System.Reflection;

namespace Norml.Core.Helpers
{
    public interface IResourcesHelper
    {
        TValue GetResource<TValue>(string resourceName, string key, Assembly assembly);
    }
}
