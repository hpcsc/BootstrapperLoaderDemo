using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Sharpenter.BootstrapperLoader;
using Sharpenter.BootstrapperLoader.Builder;
using Sharpenter.BootstrapperLoader.Helpers;

namespace BootstrapperLoaderDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var bootstrapperLoader = new LoaderBuilder()
                                        .Use(new FileSystemAssemblyProvider(HttpRuntime.BinDirectory, "BootstrapperLoaderDemo.*.dll"))
                                        .ForClass()
                                            .HasConstructorParameter("DefaultConnection")
                                        .Methods()
                                            .Call("ConfigureDevelopment").If(() => HttpContext.Current.IsDebuggingEnabled)
                                        .Build();

            var container = ConfigureIoC(bootstrapperLoader);
            bootstrapperLoader.TriggerConfigure(container.Resolve);
        }

        private static IContainer ConfigureIoC(BootstrapperLoader bootstrapperLoader)
        {
            //Create a container builder
            var builder = new ContainerBuilder();
            //Register all controllers in current assembly with container builder            
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            bootstrapperLoader.Trigger("ConfigureContainer", builder);

            //Build a real container from container builder
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    }
}
