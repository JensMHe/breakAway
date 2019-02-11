using System.Configuration;
using System.Web.Mvc;
using BreakAway.Entities;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using BreakAway.Services;
using BreakAway.Services.Filters;

namespace BreakAway.Installers
{
    public class DefaultInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .BasedOn<IController>()
                                      .LifestyleTransient());

            container.Register(Component.For<Repository>().ImplementedBy<SqlRepository>());

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            container.Register(Component.For<IBreakAwayContext>().UsingFactoryMethod(() => new BreakAwayContext(connectionString)));

            container.Register(Component.For<IContactFilterService>().ImplementedBy<ContactFilterService>());

            container.Register(Classes.FromAssemblyContaining<IFilterBy>().BasedOn<IFilterBy>().WithService.FromInterface());

        }
    }
}