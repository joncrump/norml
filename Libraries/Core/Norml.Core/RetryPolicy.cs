namespace Norml.Core
{
    public enum RetryPolicy
    {
        ThrowException = 1,
        SwallowExceptionAndSendToEvent,
        SwallowAllExceptions,
    }
}
