using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;
using FluentValidation;

namespace AspNetCore8Test.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomerController(
            ICustomerService customerService,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerService = customerService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var customers = await _customerService.SearchCustomersAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
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
        public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(createCustomerDto);
            }

            // 檢查客戶編號是否已存在
            if (await _customerService.CustomerNumberExistsAsync(createCustomerDto.CustomerNumber))
            {
                ModelState.AddModelError(nameof(createCustomerDto.CustomerNumber), 
                    $"客戶編號 {createCustomerDto.CustomerNumber} 已存在");
                return View(createCustomerDto);
            }

            try
            {
                await _customerService.CreateCustomerAsync(createCustomerDto);
                TempData["SuccessMessage"] = "客戶建立成功！";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "建立客戶時發生錯誤：" + ex.Message);
                return View(createCustomerDto);
            }
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateCustomerDto
            {
                Id = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                Name = customer.Name,
                Type = customer.Type,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                TaxId = customer.TaxId,
                IsActive = customer.IsActive,
                Status = customer.Status
            };

            return View(updateDto);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerDto updateCustomerDto)
        {
            if (id != updateCustomerDto.Id)
            {
                return BadRequest();
            }

            var validationResult = await _updateValidator.ValidateAsync(updateCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(updateCustomerDto);
            }

            try
            {
                var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
                if (customer == null)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "客戶資料更新成功！";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "更新客戶資料時發生錯誤：" + ex.Message);
                return View(updateCustomerDto);
            }
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
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
            try
            {
                var success = await _customerService.DeleteCustomerAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "客戶刪除成功！";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "刪除客戶時發生錯誤：" + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}