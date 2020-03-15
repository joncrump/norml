namespace Norml.Core.Data
{
    public interface IDatabaseFactory
    {
        IDatabaseWrapper GetDatabase(string databaseName);
    }
}
