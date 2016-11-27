using Autofac;
using Autofac.Integration.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Web
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}