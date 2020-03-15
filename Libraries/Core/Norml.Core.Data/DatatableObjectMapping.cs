using System.Collections.Generic;
using System.Data;

namespace Norml.Core.Data
{
    public class DatatableObjectMapping : IDatatableObjectMapping
    {
        public DatatableObjectMapping(DataTable dataTable, IDictionary<string, string> columnMappings)
        {
            DataTable = dataTable;
            ColumnMappings = columnMappings;
        }

        public DataTable DataTable { get; set; }
        public IDictionary<string, string> ColumnMappings { get; set; }
    }
}
