using System.Collections.Generic;
using System.Data;

namespace Norml.Core.Data
{
    public interface IDatatableObjectMapping
    {
        DataTable DataTable { get; set; }
        IDictionary<string, string> ColumnMappings { get; set; }
    }
}
