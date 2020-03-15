namespace Norml.Core.Data.QueryBuilders
{
    public enum QueryKind
    {
        SelectSingleTable = 1,
        SelectJoinTable,
        Insert,
        Update,
        Delete,
        Count,
        PagedSingle
    }
}
