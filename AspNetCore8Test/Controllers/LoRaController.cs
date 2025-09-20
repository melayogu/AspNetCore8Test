using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers;

/// <summary>
/// LoRa 管理系統情境展示控制器。
/// </summary>
public class LoRaController : Controller
{
    /// <summary>
    /// 將根路徑導向至第一版頁面，方便快速預覽。
    /// </summary>
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Version1));
    }

    /// <summary>
    /// LoRa 管理系統版本一：著重即時監控與網路健康狀態。
    /// </summary>
    public IActionResult Version1()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統版本二：聚焦事件追蹤與維運排程。
    /// </summary>
    public IActionResult Version2()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統版本三：強化設備生命週期與空間統計分析。
    /// </summary>
    public IActionResult Version3()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統版本四：指揮中心式主題，整合全域概況與自動化策略。
    /// </summary>
    public IActionResult Version4()
    {
        return View();
    }
}
