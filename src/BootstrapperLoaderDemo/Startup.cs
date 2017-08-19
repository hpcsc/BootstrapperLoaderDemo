using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Sharpenter.BootstrapperLoader;
using Sharpenter.BootstrapperLoader.Builder;
using Sharpenter.BootstrapperLoader.Helpers;

namespace BootstrapperLoaderDemo
{
    public class Startup
    {
        private readonly BootstrapperLoader _bootstrapperLoader;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            _bootstrapperLoader = new LoaderBuilder()
                        .Use(new FileSystemAssemblyProvider(PlatformServices.Default.Application.ApplicationBasePath, "BootstrapperLoaderDemo.*.dll"))
                        .ForClass()
                            .HasConstructorParameter(Configuration)
                            .Methods()
                                .Call("ConfigureDevelopment").If(env.IsDevelopment)
                        .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            _bootstrapperLoader.Trigger("ConfigureContainer", services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            _bootstrapperLoader.TriggerConfigure(app.ApplicationServices.GetService);
        }
    }
}
