using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using PlanIt.Repositories.Abstract;
using PlanIt.Repositories.Concrete;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Services
{
    [ExcludeFromCodeCoverage]
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PlanIt"].ConnectionString;
            builder.Register(с => new DefaultDataContextSettings(connectionString)).As<IDataContextSettings>();
            builder.RegisterModule<Repositories.AutofacModule>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PlanService>().As<IPlanService>();
            builder.RegisterType<ProfileService>().As<IProfileService>();
            builder.RegisterType<SharingService>().As<ISharingService>();
            builder.RegisterType<PlanItemService>().As<IPlanItemService>();
        }
    }
}
