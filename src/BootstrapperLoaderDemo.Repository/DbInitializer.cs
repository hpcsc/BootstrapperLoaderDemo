using System.Collections.Generic;
using System.Data.Entity;
using BootstrapperLoaderDemo.Core.ManageBooks;

namespace BootstrapperLoaderDemo.Repository
{
    public class DbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext context)
        {
            var books = new List<Book>
            {
                new Book("C# in Depth", "Jon Skeet", "Manning Publications"),
                new Book("Professional ASP.NET Design Patterns", "Scott Millett", "Wrox"),
                new Book("Microsoft .NET - Architecting Applications for the Enterprise", "Dino Esposito, Andrea Saltarello", "Microsoft Press"),
                new Book("Adaptive Code via C#", "Gary McLean Hall", "Microsoft Press"),
                new Book("Dependency Injection in .NET", "Mark Seemann", "Manning Publications")
            };

            books.ForEach(book => context.Set<Book>().Add(book));

            context.SaveChanges();
        }
    }
}
