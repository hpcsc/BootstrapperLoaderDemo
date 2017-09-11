using System.Collections.Generic;

namespace BootstrapperLoaderDemo.Core.ManageBooks
{
    public interface IBookRepository
    {
        IEnumerable<Book> FindAll();
    }
}
