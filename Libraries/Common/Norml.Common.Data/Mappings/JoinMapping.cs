using System.Runtime.Serialization;

namespace Norml.Common.Data.Mappings
{
    public class JoinMapping : IJoinMapping
    {
        public JoinType JoinType { get; set; }
        public string LeftKey { get; set; }
        public string RightKey { get; set; }
        public string JoinTable { get; set; }
        public string JoinTableLeftKey { get; set; }
        public string JoinTableRightKey { get; set; }
        public JoinType JoinTableJoinType { get; set; }
        public string ParentProperty { get; set; }
        public string ChildProperty { get; set; }
    }
}