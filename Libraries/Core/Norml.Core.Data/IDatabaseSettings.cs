using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norml.Common.Data
{
    public interface IDatabaseSettings
    {
        IDictionary<string, string> ConnectionStrings { get; }
    }
}
