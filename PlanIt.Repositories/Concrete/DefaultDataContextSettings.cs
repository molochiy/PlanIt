using PlanIt.Repositories.Abstract;

namespace PlanIt.Repositories.Concrete
{
    public class DefaultDataContextSettings: IDataContextSettings
    {
        #region Constructors

        public DefaultDataContextSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region IDataContextSettings

        public string ConnectionString { get; }

        #endregion
    }
}
