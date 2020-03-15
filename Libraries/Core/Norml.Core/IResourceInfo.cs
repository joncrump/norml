using System.Reflection;

namespace Norml.Core
{
    public interface IResourcesInfo
    {
        string ResourceName { get; set; }
        string Key { get; set; }
        Assembly Assembly { get; set; }
    }
}
