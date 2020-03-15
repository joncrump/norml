using System.Reflection;

namespace Norml.Core
{
    public class ResourcesInfo : IResourcesInfo
    {
        public string ResourceName { get; set; }
        public string Key { get; set; }
        public Assembly Assembly { get; set; }
    }
}
