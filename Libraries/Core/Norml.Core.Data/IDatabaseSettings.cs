using System.Collections.Generic;

namespace Norml.Core.Data
{
    public interface IDatabaseSettings
    {
        IDictionary<string, string> ConnectionStrings { get; }
    }
}
