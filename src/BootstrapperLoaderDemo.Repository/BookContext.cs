using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using BootstrapperLoaderDemo.Core.ManageBooks;

namespace BootstrapperLoaderDemo.Repository
{
    public class BookContext : DbContext
    {
        static BookContext()
        {
            Database.SetInitializer<BookContext>(null);
        }

        public BookContext(string connectionStringName)
            : base(connectionStringName)
        {
            base.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id)
                .Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
