using System.Collections.Generic;
using System.Data;

namespace Norml.Common.Data.Constants
{
    public static class DatabaseTypes
    {
        public static Dictionary<string, SqlDbType> FieldMappings => new Dictionary<string, SqlDbType>
        {
            { "nvarchar", SqlDbType.NVarChar},
            { "int", SqlDbType.Int },
            { "xml", SqlDbType.Xml },
            { "guid", SqlDbType.UniqueIdentifier },
            { "smalldatetime", SqlDbType.SmallDateTime }
        };
    }
}
