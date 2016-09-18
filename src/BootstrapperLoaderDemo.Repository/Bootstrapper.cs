using BootstrapperLoaderDemo.Core.ManageBooks;
using BootstrapperLoaderDemo.Repository.ManageBooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BootstrapperLoaderDemo.Repository
{
    public class Bootstrapper
    {
        private readonly IConfigurationRoot _configuration;

        public Bootstrapper(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureContainer(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<BookContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            );            
        }

        public void ConfigureDevelopment(BookContext context)
        {
            DbInitializer.Seed(context);
        }
    }
}
