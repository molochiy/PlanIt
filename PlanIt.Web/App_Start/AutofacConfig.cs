using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace PlanIt.Web
{
    public class AutofacConfig
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule<Services.AutofacModule>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}