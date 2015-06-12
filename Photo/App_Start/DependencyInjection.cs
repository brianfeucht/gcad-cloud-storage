using Autofac;
using Autofac.Integration.Mvc;
using Photo.Azure;
using Photo.Core;
using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Photo
{
    public class DependencyInjection
    {
        private static QueueProcessor queueProcessor;

        internal static void RegisterResovler()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            BuildCommonDependencies(builder);
            //LocalVersionDependencies(builder);
            AzureVersionDependencies(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            SetupAzureInfrastructure(container);

            queueProcessor = container.Resolve<QueueProcessor>();
            queueProcessor.Start();
        }

        private static void BuildCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<MemeRepository>().AsImplementedInterfaces();
            builder.RegisterType<MemeGenerator>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QueueProcessor>().SingleInstance();
        }

        private static void AzureVersionDependencies(ContainerBuilder builder)
        {
            builder.Register(c => new AzureFileStorage("gcadstoragedemo")).AsImplementedInterfaces();
            builder.Register(c => new AzureQueue("gcadstoragedemo")).AsImplementedInterfaces();
            builder.Register(c => new AzureTable("gcadstoragedemo")).AsImplementedInterfaces();
        }

        private static void SetupAzureInfrastructure(IContainer container)
        {
            Task.WhenAll(
                ((AzureFileStorage)container.Resolve<ICloudFileStorage>()).EnsureContainerIsCreated(),
                ((AzureQueue)container.Resolve<ICloudQueue>()).EnsureQueueHasBeenCreated(),
                ((AzureTable)container.Resolve<ICloudTable>()).EnsureTableHasBeenCreated());
        }

        private static void LocalVersionDependencies(ContainerBuilder builder)
        {
            builder.Register(c => new LocalFileStorage(HostingEnvironment.MapPath("~/Uploads/"), "/Uploads/")).AsImplementedInterfaces();
            //This needs to be a singleton
            builder.RegisterType<LocalQueue>().AsImplementedInterfaces().SingleInstance();
            builder.Register(c => new LocalTable(HostingEnvironment.MapPath("~/App_Data/Raven/"))).AsImplementedInterfaces();
        }
    }
}