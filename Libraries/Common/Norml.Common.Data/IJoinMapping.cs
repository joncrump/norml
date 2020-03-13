namespace Norml.Common.Data
{
    public interface IJoinMapping
    {
        JoinType JoinType { get; set; }
        string LeftKey { get; set; }
        string RightKey { get; set; }
        string JoinTable { get; set; }
        JoinType JoinTableJoinType { get; set; }
        string JoinTableLeftKey { get; set; }
        string JoinTableRightKey { get; set; }
    }
}
