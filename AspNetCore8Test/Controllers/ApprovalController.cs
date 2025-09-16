using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models;
using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Services;

namespace AspNetCore8Test.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly ApprovalService _approvalService;

        public ApprovalController(ApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        /// <summary>
        /// 簽核首頁
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得簽核待辦列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPendingApprovals()
        {
            try
            {
                var pendingApprovals = await _approvalService.GetPendingApprovalsAsync();
                return Json(new { success = true, data = pendingApprovals });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 取得即時流程列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInProgressApprovals()
        {
            try
            {
                var inProgressApprovals = await _approvalService.GetInProgressApprovalsAsync();
                return Json(new { success = true, data = inProgressApprovals });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 取得簽核歷史列表
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApprovalHistory(int page = 1, int pageSize = 10)
        {
            try
            {
                var historyResult = await _approvalService.GetApprovalHistoryAsync(page, pageSize);
                return Json(new { 
                    success = true, 
                    data = historyResult.Items,
                    totalCount = historyResult.TotalCount,
                    currentPage = page,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 執行簽核動作
        /// </summary>
        /// <param name="actionDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessApproval([FromBody] ApprovalActionDto actionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "請求資料格式錯誤" });
                }

                var result = await _approvalService.ProcessApprovalAsync(actionDto);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 取得簽核詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApprovalDetail(int id)
        {
            try
            {
                var approval = await _approvalService.GetApprovalDetailAsync(id);
                if (approval == null)
                {
                    return Json(new { success = false, message = "找不到簽核項目" });
                }

                return Json(new { success = true, data = approval });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}