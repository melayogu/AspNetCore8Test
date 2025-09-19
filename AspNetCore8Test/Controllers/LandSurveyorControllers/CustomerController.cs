using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models.LandSurveyorModels;
using AspNetCore8Test.Services.LandSurveyorServices;

namespace AspNetCore8Test.Controllers.LandSurveyorControllers
{
    public class LandSurveyorCustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public LandSurveyorCustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IdNumber,Phone,Email,Address,Notes,IsActive")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // 檢查身分證字號是否已存在
                if (await _customerService.IdNumberExistsAsync(customer.IdNumber))
                {
                    ModelState.AddModelError("IdNumber", "此身分證字號已存在");
                    return View(customer);
                }

                await _customerService.CreateCustomerAsync(customer);
                TempData["SuccessMessage"] = "客戶資料已成功建立！";
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IdNumber,Phone,Email,Address,CreatedDate,LastModifiedDate,Notes,IsActive")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 檢查身分證字號是否已存在（排除自己）
                    if (await _customerService.IdNumberExistsAsync(customer.IdNumber, customer.Id))
                    {
                        ModelState.AddModelError("IdNumber", "此身分證字號已存在");
                        return View(customer);
                    }

                    customer.LastModifiedDate = DateTime.Now;
                    await _customerService.UpdateCustomerAsync(customer);
                    TempData["SuccessMessage"] = "客戶資料已成功更新！";
                }
                catch (Exception)
                {
                    if (!await _customerService.CustomerExistsAsync(customer.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "客戶資料已成功刪除！";
            }
            else
            {
                TempData["ErrorMessage"] = "刪除客戶資料時發生錯誤！";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _customerService.CustomerExistsAsync(id);
        }
    }
}