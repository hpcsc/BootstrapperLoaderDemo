using System.Collections.Generic;
using System.Linq;
using BootstrapperLoaderDemo.Core.ManageBooks;
using Microsoft.EntityFrameworkCore;

namespace BootstrapperLoaderDemo.Repository.ManageBooks
{
    public class BookRepository : IBookRepository
    {
        private readonly DbSet<Book> _set;

        public BookRepository(BookContext context)
        {
            _set = context.Set<Book>();
        }

        public IEnumerable<Book> FindAll()
        {
            return _set.ToList();
        }
    }
}