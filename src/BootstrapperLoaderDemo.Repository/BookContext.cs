using BootstrapperLoaderDemo.Core.ManageBooks;
using Microsoft.EntityFrameworkCore;

namespace BootstrapperLoaderDemo.Repository
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(builder =>
            {
                builder.HasKey(b => b.Id);
                builder.Property(b => b.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
