using Norml.Core.Extensions;

namespace Norml.Core
{
    public class PagingInfo
    {
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        public override string ToString()
        {
            return "{0}{1}{2}{3}".FormatString(PageNumber, RowsPerPage, SortColumn, SortOrder);
        }
    }
}
