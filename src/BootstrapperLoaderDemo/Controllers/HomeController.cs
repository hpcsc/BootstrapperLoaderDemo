using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var books = _bookRepository.FindAll();

            return View(books);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
