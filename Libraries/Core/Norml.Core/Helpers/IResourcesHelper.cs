using System.Reflection;

namespace Norml.Common.Helpers
{
    public interface IResourcesHelper
    {
        TValue GetResource<TValue>(string resourceName, string key, Assembly assembly);
    }
}
