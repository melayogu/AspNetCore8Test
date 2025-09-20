using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers;

/// <summary>
/// LoRa 管理系統多版本體驗控制器。
/// </summary>
public class LoRaController : Controller
{
    /// <summary>
    /// LoRa 管理系統第一版頁面。
    /// </summary>
    public IActionResult VersionOne()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統第二版頁面。
    /// </summary>
    public IActionResult VersionTwo()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統第三版頁面。
    /// </summary>
    public IActionResult VersionThree()
    {
        return View();
    }

    /// <summary>
    /// LoRa 管理系統第四版頁面。
    /// </summary>
    public IActionResult VersionFour()
    {
        return View();
    }
}
