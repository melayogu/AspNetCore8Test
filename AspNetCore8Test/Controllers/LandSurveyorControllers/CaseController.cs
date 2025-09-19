using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCore8Test.Models.LandSurveyorModels;
using AspNetCore8Test.Services.LandSurveyorServices;

namespace AspNetCore8Test.Controllers.LandSurveyorControllers
{
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;
        private readonly ICustomerService _customerService;

        public CaseController(ICaseService caseService, ICustomerService customerService)
        {
            _caseService = caseService;
            _customerService = customerService;
        }

        // GET: Case
        public async Task<IActionResult> Index()
        {
            var cases = await _caseService.GetAllCasesAsync();
            return View(cases);
        }

        // GET: Case/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
            {
                return NotFound();
            }

            return View(caseEntity);
        }

        // GET: Case/Create
        public async Task<IActionResult> Create()
        {
            await LoadCustomers();
            return View();
        }

        // POST: Case/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CaseNumber,CaseName,CaseType,CustomerId,ReceivedDate,ExpectedCompletionDate,Description,Fee,Notes")] Case caseEntity)
        {
            if (ModelState.IsValid)
            {
                // 檢查案件編號是否已存在
                if (await _caseService.CaseNumberExistsAsync(caseEntity.CaseNumber))
                {
                    ModelState.AddModelError("CaseNumber", "此案件編號已存在");
                    await LoadCustomers();
                    return View(caseEntity);
                }

                await _caseService.CreateCaseAsync(caseEntity);
                TempData["SuccessMessage"] = "案件已成功建立！";
                return RedirectToAction(nameof(Index));
            }
            await LoadCustomers();
            return View(caseEntity);
        }

        // GET: Case/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
            {
                return NotFound();
            }
            await LoadCustomers();
            return View(caseEntity);
        }

        // POST: Case/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaseNumber,CaseName,CaseType,Status,CustomerId,ReceivedDate,ExpectedCompletionDate,ActualCompletionDate,Description,Fee,PaidAmount,CreatedDate,LastModifiedDate,Notes")] Case caseEntity)
        {
            if (id != caseEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 檢查案件編號是否已存在（排除自己）
                    if (await _caseService.CaseNumberExistsAsync(caseEntity.CaseNumber, caseEntity.Id))
                    {
                        ModelState.AddModelError("CaseNumber", "此案件編號已存在");
                        await LoadCustomers();
                        return View(caseEntity);
                    }

                    caseEntity.LastModifiedDate = DateTime.Now;
                    await _caseService.UpdateCaseAsync(caseEntity);
                    TempData["SuccessMessage"] = "案件資料已成功更新！";
                }
                catch (Exception)
                {
                    if (!await _caseService.CaseExistsAsync(caseEntity.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            await LoadCustomers();
            return View(caseEntity);
        }

        // GET: Case/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _caseService.GetCaseByIdAsync(id.Value);
            if (caseEntity == null)
            {
                return NotFound();
            }

            return View(caseEntity);
        }

        // POST: Case/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _caseService.DeleteCaseAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "案件已成功刪除！";
            }
            else
            {
                TempData["ErrorMessage"] = "刪除案件時發生錯誤！";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewData["CustomerId"] = new SelectList(customers, "Id", "Name");
        }
    }
}