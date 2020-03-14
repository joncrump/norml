namespace Norml.Common.Data
{
    public interface IDatabaseFactory
    {
        IDatabaseWrapper GetDatabase(string databaseName);
    }
}
