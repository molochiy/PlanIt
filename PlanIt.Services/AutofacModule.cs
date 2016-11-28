using System.Configuration;
using Autofac;
using PlanIt.Repositories.Abstract;
using PlanIt.Repositories.Concrete;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PlanIt"].ConnectionString;
            builder.Register(с => new DefaultDataContextSettings(connectionString)).As<IDataContextSettings>();
            builder.RegisterModule<Repositories.AutofacModule>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PlanService>().As<IPlanService>();
        }
    }
}
