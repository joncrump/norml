namespace Norml.Common.Data
{
    public interface IBuilder<in TDataSource>
    {
        TItem Build<TItem>(TDataSource dataSource, BuilderOptions builderOptions = null);
    }

    public interface IBuilder<in TDataSource, out TModel>
    {
        TModel Build(TDataSource dataSource);
    }
}
