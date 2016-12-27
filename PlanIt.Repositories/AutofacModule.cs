using System.Diagnostics.CodeAnalysis;
using Autofac;
using PlanIt.Repositories.Abstract;
using PlanIt.Repositories.Concrete;

namespace PlanIt.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AutofacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EFDataContextFactory>().As<IDataContextFactory>();
            builder.RegisterType<EFRepository>().As<IRepository>();
        }
    }
}
