namespace Norml.Core.Models.Enums
{
    public enum RetrievalMode
    {
        TryAll = 1,
        FailOnFirst = 2,
        TryCacheAndThenTryAll = 3,
        TryCacheAndThenFailOnFirst = 4
    }
}
