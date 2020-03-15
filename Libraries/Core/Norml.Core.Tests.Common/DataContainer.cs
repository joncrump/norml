using System.Collections.Generic;
using System.Linq;

namespace Norml.Core.Tests.Common
{
    public class DataContainer
    {
        public int NumberOfRecords { get; set; }
        public IList<ColumnInfo> Results { get; set; }

        public DataContainer()
        {
            Results = new List<ColumnInfo>();
        }

        public DataContainer(int numberOfRecords, IEnumerable<ColumnInfo> results)
        {
            NumberOfRecords = numberOfRecords;
            Results = results.ToList();
        }
    }
}