using System.Web.Mvc;
using BootstrapperLoaderDemo.Core.ManageBooks;

namespace BootstrapperLoaderDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public HomeController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public ActionResult Index()
        {
            var books = _bookRepository.FindAll();

            return View(books);
        }
    }
}