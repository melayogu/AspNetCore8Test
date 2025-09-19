using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCore8Test.Models.LandSurveyorModels;
using AspNetCore8Test.Services.LandSurveyorServices;

namespace AspNetCore8Test.Controllers.LandSurveyorControllers
{
    public class CaseProgressController : Controller
    {
        private readonly ICaseProgressService _caseProgressService;
        private readonly ICaseService _caseService;

        public CaseProgressController(ICaseProgressService caseProgressService, ICaseService caseService)
        {
            _caseProgressService = caseProgressService;
            _caseService = caseService;
        }

        // GET: CaseProgress
        public async Task<IActionResult> Index(int? caseId)
        {
            IEnumerable<CaseProgress> progresses;
            
            if (caseId.HasValue)
            {
                progresses = await _caseProgressService.GetByCaseIdAsync(caseId.Value);
                ViewBag.CaseId = caseId.Value;
                ViewBag.Case = await _caseService.GetCaseByIdAsync(caseId.Value);
            }
            else
            {
                progresses = await _caseProgressService.GetAllAsync();
            }

            // 取得案件資訊用於顯示
            var cases = await _caseService.GetAllCasesAsync();
            ViewBag.Cases = cases.ToDictionary(c => c.Id, c => c);

            return View(progresses);
        }

        // GET: CaseProgress/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var caseProgress = await _caseProgressService.GetByIdAsync(id);
            if (caseProgress == null)
            {
                return NotFound();
            }

            // 取得相關案件資訊
            var caseInfo = await _caseService.GetCaseByIdAsync(caseProgress.CaseId);
            ViewBag.Case = caseInfo;

            return View(caseProgress);
        }

        // GET: CaseProgress/Create
        public async Task<IActionResult> Create(int? caseId)
        {
            await PopulateCasesDropDown();
            
            var model = new CaseProgress();
            if (caseId.HasValue)
            {
                model.CaseId = caseId.Value;
                ViewBag.SelectedCase = await _caseService.GetCaseByIdAsync(caseId.Value);
            }

            return View(model);
        }

        // POST: CaseProgress/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseProgress caseProgress)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _caseProgressService.CreateAsync(caseProgress);
                    TempData["SuccessMessage"] = "案件進度記錄已成功建立！";
                    return RedirectToAction(nameof(Index), new { caseId = caseProgress.CaseId });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"建立案件進度記錄失敗：{ex.Message}";
                }
            }

            await PopulateCasesDropDown(caseProgress.CaseId);
            return View(caseProgress);
        }

        // GET: CaseProgress/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var caseProgress = await _caseProgressService.GetByIdAsync(id);
            if (caseProgress == null)
            {
                return NotFound();
            }

            await PopulateCasesDropDown(caseProgress.CaseId);
            return View(caseProgress);
        }

        // POST: CaseProgress/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaseProgress caseProgress)
        {
            if (id != caseProgress.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _caseProgressService.UpdateAsync(caseProgress);
                    TempData["SuccessMessage"] = "案件進度記錄已成功更新！";
                    return RedirectToAction(nameof(Index), new { caseId = caseProgress.CaseId });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"更新案件進度記錄失敗：{ex.Message}";
                }
            }

            await PopulateCasesDropDown(caseProgress.CaseId);
            return View(caseProgress);
        }

        // GET: CaseProgress/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var caseProgress = await _caseProgressService.GetByIdAsync(id);
            if (caseProgress == null)
            {
                return NotFound();
            }

            // 取得相關案件資訊
            var caseInfo = await _caseService.GetCaseByIdAsync(caseProgress.CaseId);
            ViewBag.Case = caseInfo;

            return View(caseProgress);
        }

        // POST: CaseProgress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var caseProgress = await _caseProgressService.GetByIdAsync(id);
                var caseId = caseProgress?.CaseId;

                var result = await _caseProgressService.DeleteAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "案件進度記錄已成功刪除！";
                }
                else
                {
                    TempData["ErrorMessage"] = "刪除案件進度記錄失敗！";
                }

                return RedirectToAction(nameof(Index), new { caseId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"刪除案件進度記錄失敗：{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CaseProgress/InitializeWorkflow
        [HttpPost]
        public async Task<IActionResult> InitializeWorkflow(int caseId, string caseType)
        {
            try
            {
                var result = await _caseProgressService.InitializeWorkflowAsync(caseId, caseType);
                if (result)
                {
                    TempData["SuccessMessage"] = $"已成功為案件初始化 {caseType} 工作流程！";
                }
                else
                {
                    TempData["ErrorMessage"] = "初始化工作流程失敗！";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"初始化工作流程失敗：{ex.Message}";
            }

            return RedirectToAction(nameof(Index), new { caseId });
        }

        // API: 取得工作流程模板
        [HttpGet]
        public async Task<IActionResult> GetWorkflowTemplate(string caseType)
        {
            var template = await _caseProgressService.GetWorkflowTemplateAsync(caseType);
            return Json(template);
        }

        private async Task PopulateCasesDropDown(object? selectedCase = null)
        {
            var cases = await _caseService.GetAllCasesAsync();
            ViewBag.CaseId = new SelectList(cases, "Id", "Title", selectedCase);
        }
    }
}