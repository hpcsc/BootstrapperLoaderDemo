using System.Data.Entity;
using Autofac;
using BootstrapperLoaderDemo.Core.ManageBooks;
using BootstrapperLoaderDemo.Repository.ManageBooks;

namespace BootstrapperLoaderDemo.Repository
{
    public class Bootstrapper
    {
        private readonly string _connectionStringName;

        public Bootstrapper(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<BookContext>().AsSelf().InstancePerLifetimeScope()
                .WithParameter("connectionStringName", _connectionStringName);
        }

        public void ConfigureDevelopment(BookContext context)
        {
            Database.SetInitializer(new DbInitializer());
        }
    }
}
