using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;

namespace AspNetCore8Test.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IBillingService _billingService;
        private readonly IPipelineService _pipelineService;

        public DashboardController(
            ICustomerService customerService,
            IBillingService billingService,
            IPipelineService pipelineService)
        {
            _customerService = customerService;
            _billingService = billingService;
            _pipelineService = pipelineService;
        }

        public async Task<IActionResult> Index()
        {
            // 獲取統計數據
            var customers = await _customerService.GetAllCustomersAsync();
            var bills = await _billingService.GetAllBillsAsync();
            var unpaidBills = await _billingService.GetUnpaidBillsAsync();
            var overdueBills = await _billingService.GetOverdueBillsAsync();
            var pipelines = await _pipelineService.GetAllPipelinesAsync();
            var alerts = await _pipelineService.GetPipelineAlertsAsync(true);

            // 準備儀表板數據
            var dashboardData = new
            {
                // 客戶統計
                TotalCustomers = customers.Count(),
                ActiveCustomers = customers.Count(c => c.IsActive),
                ResidentialCustomers = customers.Count(c => c.Type == "住宅"),
                CommercialCustomers = customers.Count(c => c.Type == "商業"),
                IndustrialCustomers = customers.Count(c => c.Type == "工業"),

                // 帳單統計
                TotalBills = bills.Count(),
                TotalRevenue = bills.Sum(b => b.TotalAmount),
                PaidRevenue = bills.Sum(b => b.PaidAmount),
                UnpaidBills = unpaidBills.Count(),
                UnpaidAmount = unpaidBills.Sum(b => b.BalanceAmount),
                OverdueBills = overdueBills.Count(),
                OverdueAmount = overdueBills.Sum(b => b.BalanceAmount),

                // 管線統計
                TotalPipelines = pipelines.Count(),
                ActivePipelines = pipelines.Count(p => p.Status == "Active"),
                MaintenancePipelines = pipelines.Count(p => p.Status == "Maintenance"),
                TotalLength = pipelines.Sum(p => p.Length),
                ActiveAlerts = alerts.Count(),
                CriticalAlerts = alerts.Count(a => a.Severity == "Critical"),
                HighAlerts = alerts.Count(a => a.Severity == "High"),

                // 最近活動
                RecentCustomers = customers.OrderByDescending(c => c.RegisterDate).Take(5),
                RecentBills = bills.OrderByDescending(b => b.BillDate).Take(5),
                RecentAlerts = alerts.OrderByDescending(a => a.AlertTime).Take(5)
            };

            return View(dashboardData);
        }

        [HttpGet]
        public async Task<IActionResult> GetChartData(string chartType)
        {
            switch (chartType.ToLower())
            {
                case "revenue":
                    return await GetRevenueChartData();
                case "customers":
                    return await GetCustomerChartData();
                case "pipelines":
                    return await GetPipelineChartData();
                default:
                    return BadRequest("Invalid chart type");
            }
        }

        private async Task<IActionResult> GetRevenueChartData()
        {
            var bills = await _billingService.GetAllBillsAsync();
            
            // 按月統計收入
            var monthlyRevenue = bills
                .GroupBy(b => new { b.BillDate.Year, b.BillDate.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}/{g.Key.Month:D2}",
                    Total = g.Sum(b => b.TotalAmount),
                    Paid = g.Sum(b => b.PaidAmount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Json(monthlyRevenue);
        }

        private async Task<IActionResult> GetCustomerChartData()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            
            var customerTypes = customers
                .GroupBy(c => c.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Json(customerTypes);
        }

        private async Task<IActionResult> GetPipelineChartData()
        {
            var pipelines = await _pipelineService.GetAllPipelinesAsync();
            
            var pipelineStatus = pipelines
                .GroupBy(p => p.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Length = g.Sum(p => p.Length)
                })
                .ToList();

            return Json(pipelineStatus);
        }

        [HttpGet]
        public async Task<IActionResult> GetSystemStats()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var bills = await _billingService.GetAllBillsAsync();
            var alerts = await _pipelineService.GetPipelineAlertsAsync(true);

            var stats = new
            {
                TotalCustomers = customers.Count(),
                UnpaidBills = bills.Count(b => b.Status == "Pending" || b.Status == "Partial"),
                ActiveAlerts = alerts.Count(),
                SystemStatus = alerts.Any(a => a.Severity == "Critical") ? "Critical" : 
                              alerts.Any(a => a.Severity == "High") ? "Warning" : "Normal"
            };

            return Json(stats);
        }
    }
}