using Autofac;
using Autofac.Integration.Mvc;
using Photo.Core;
using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Photo
{
    public class DependencyInjection
    {
        internal static void RegisterResovler()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            BuildCommonDependencies(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void BuildCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<MemeRepository>().As<IMemeRepository>();
        }
    }
}