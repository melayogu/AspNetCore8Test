using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers;

/// <summary>
/// 碧湖公園管理系統主控制器
/// </summary>
public class ParkController : Controller
{
    /// <summary>
    /// 公園管理首頁
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// 設施管理頁面
    /// </summary>
    public IActionResult Facilities()
    {
        return View();
    }

    /// <summary>
    /// 植栽管理頁面
    /// </summary>
    public IActionResult Plants()
    {
        return View();
    }

    /// <summary>
    /// 環境監測頁面
    /// </summary>
    public IActionResult Environmental()
    {
        return View();
    }

    /// <summary>
    /// 遊客服務頁面
    /// </summary>
    public IActionResult Visitors()
    {
        return View();
    }

    /// <summary>
    /// 活動管理頁面
    /// </summary>
    public IActionResult Activities()
    {
        return View();
    }

    /// <summary>
    /// 回饋管理頁面
    /// </summary>
    public IActionResult Feedback()
    {
        return View();
    }

    /// <summary>
    /// 安全監控頁面
    /// </summary>
    public IActionResult Security()
    {
        return View();
    }

    /// <summary>
    /// 數據分析頁面
    /// </summary>
    public IActionResult Analytics()
    {
        return View();
    }

    /// <summary>
    /// 營運報告頁面
    /// </summary>
    public IActionResult Reports()
    {
        return View();
    }
}