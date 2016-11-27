namespace PlanIt.Repositories.Abstract
{
    public interface IDataContextFactory
    {
        IDataContext NewInstance(bool explicitOpenConnection = false);
    }
}
