namespace Norml.Common
{
    public enum RetryPolicy
    {
        ThrowException = 1,
        SwallowExceptionAndSendToEvent,
        SwallowAllExceptions,
    }
}
