using System.Collections.Generic;
using System.Data;

namespace Norml.Common.Data
{
    public interface IDatatableObjectMapping
    {
        DataTable DataTable { get; set; }
        IDictionary<string, string> ColumnMappings { get; set; }
    }
}
