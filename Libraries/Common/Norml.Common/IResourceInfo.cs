using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Norml.Common
{
    public interface IResourcesInfo
    {
        string ResourceName { get; set; }
        string Key { get; set; }
        Assembly Assembly { get; set; }
    }
}
