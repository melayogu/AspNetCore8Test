using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LandSurveyorControllers
{
    public class LandSurveyorDashboardController : Controller
    {
        public IActionResult Index()
        {
            // 模擬儀表板資料
            ViewData["TotalCustomers"] = 15;
            ViewData["ActiveCases"] = 8;
            ViewData["CompletedCases"] = 25;
            ViewData["PendingAppointments"] = 5;
            ViewData["MonthlyRevenue"] = 125000;
            
            return View();
        }
    }
}