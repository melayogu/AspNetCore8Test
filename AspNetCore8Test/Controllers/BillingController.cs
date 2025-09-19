using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;
        private readonly ICustomerService _customerService;

        public BillingController(IBillingService billingService, ICustomerService customerService)
        {
            _billingService = billingService;
            _customerService = customerService;
        }

        // GET: Billing
        public async Task<IActionResult> Index()
        {
            var bills = await _billingService.GetAllBillsAsync();
            return View(bills);
        }

        // GET: Billing/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var bill = await _billingService.GetBillByIdAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        // GET: Billing/Create
        public async Task<IActionResult> Create()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Customers = customers.ToList();
            return View();
        }

        // POST: Billing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillDto createBillDto)
        {
            if (!ModelState.IsValid)
            {
                var customers = await _customerService.GetAllCustomersAsync();
                ViewBag.Customers = customers.ToList();
                return View(createBillDto);
            }

            try
            {
                await _billingService.CreateBillAsync(createBillDto);
                TempData["SuccessMessage"] = "帳單建立成功！";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "建立帳單時發生錯誤：" + ex.Message);
                var customers = await _customerService.GetAllCustomersAsync();
                ViewBag.Customers = customers.ToList();
                return View(createBillDto);
            }
        }

        // GET: Billing/Payment/5
        public async Task<IActionResult> Payment(int id)
        {
            var bill = await _billingService.GetBillByIdAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            if (bill.Status == "Paid")
            {
                TempData["WarningMessage"] = "此帳單已完成付款。";
                return RedirectToAction(nameof(Details), new { id });
            }

            var paymentDto = new PaymentDto
            {
                BillId = id,
                Amount = bill.BalanceAmount
            };

            ViewBag.Bill = bill;
            return View(paymentDto);
        }

        // POST: Billing/Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                var bill = await _billingService.GetBillByIdAsync(paymentDto.BillId);
                ViewBag.Bill = bill;
                return View(paymentDto);
            }

            try
            {
                var updatedBill = await _billingService.ProcessPaymentAsync(paymentDto);
                if (updatedBill == null)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "付款處理成功！";
                return RedirectToAction(nameof(Details), new { id = paymentDto.BillId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "處理付款時發生錯誤：" + ex.Message);
                var bill = await _billingService.GetBillByIdAsync(paymentDto.BillId);
                ViewBag.Bill = bill;
                return View(paymentDto);
            }
        }

        // GET: Billing/Unpaid
        public async Task<IActionResult> Unpaid()
        {
            var unpaidBills = await _billingService.GetUnpaidBillsAsync();
            ViewData["Title"] = "未付款帳單";
            return View("Index", unpaidBills);
        }

        // GET: Billing/Overdue
        public async Task<IActionResult> Overdue()
        {
            var overdueBills = await _billingService.GetOverdueBillsAsync();
            ViewData["Title"] = "逾期帳單";
            return View("Index", overdueBills);
        }

        // GET: Billing/Customer/5
        public async Task<IActionResult> Customer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerBills = await _billingService.GetBillsByCustomerIdAsync(id);
            ViewData["Title"] = $"{customer.Name} 的帳單記錄";
            ViewBag.Customer = customer;
            return View("Index", customerBills);
        }
    }
}