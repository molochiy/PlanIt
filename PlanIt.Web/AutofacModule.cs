using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Integration.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;
using PlanIt.Web.Hubs;

namespace PlanIt.Web
{
    [ExcludeFromCodeCoverage]
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationHub>().As<INotificationHub>();
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}