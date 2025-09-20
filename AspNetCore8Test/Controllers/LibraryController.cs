using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models;
using AspNetCore8Test.Services;

namespace AspNetCore8Test.Controllers
{
    public class LibraryController : Controller
    {
        private readonly Services.LibraryService _libraryService;

        public LibraryController(Services.LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public IActionResult Index()
        {
            var model = new LibraryViewModel
            {
                LibraryHours = _libraryService.GetTodayHours(),
                BorrowingStats = _libraryService.GetBorrowingStats(),
                LatestNews = _libraryService.GetLatestNews(),
                NewArrivals = _libraryService.GetNewArrivals(),
                FeaturedBooks = _libraryService.GetFeaturedBooks(),
                PopularBooks = _libraryService.GetPopularBooks(),
                CurrentEvent = _libraryService.GetCurrentEvent()
            };

            return View(model);
        }

        public IActionResult Search(string query, string category = "all")
        {
            var results = _libraryService.SearchBooks(query, category);
            ViewBag.Query = query;
            ViewBag.Category = category;
            return View(results);
        }

        public IActionResult BookDetails(int id)
        {
            var book = _libraryService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult MyAccount()
        {
            // 這裡應該根據實際的用戶認證系統來獲取用戶資訊
            var userAccount = _libraryService.GetUserAccount("current_user");
            return View(userAccount);
        }

        public IActionResult Services()
        {
            var services = _libraryService.GetLibraryServices();
            return View(services);
        }

        public IActionResult Events()
        {
            var events = _libraryService.GetUpcomingEvents();
            return View(events);
        }

        public IActionResult Resources()
        {
            var resources = _libraryService.GetDigitalResources();
            return View(resources);
        }
    }
}