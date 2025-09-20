using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers
{
    public class LoRaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
