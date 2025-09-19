using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 環境監測 API 控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EnvironmentalController : ControllerBase
{
    private readonly IEnvironmentalService _environmentalService;

    public EnvironmentalController(IEnvironmentalService environmentalService)
    {
        _environmentalService = environmentalService;
    }

    /// <summary>
    /// 取得所有環境監測記錄
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetAll()
    {
        var monitoring = await _environmentalService.GetAllAsync();
        return Ok(monitoring);
    }

    /// <summary>
    /// 根據ID取得環境監測記錄
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EnvironmentalMonitoringDto>> GetById(int id)
    {
        var monitoring = await _environmentalService.GetByIdAsync(id);
        if (monitoring == null)
        {
            return NotFound($"找不到ID為 {id} 的環境監測記錄");
        }
        return Ok(monitoring);
    }

    /// <summary>
    /// 創建新的環境監測記錄
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EnvironmentalMonitoringDto>> Create([FromBody] CreateEnvironmentalMonitoringDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var monitoring = await _environmentalService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = monitoring.Id }, monitoring);
    }

    /// <summary>
    /// 更新環境監測記錄
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateEnvironmentalMonitoringDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _environmentalService.UpdateAsync(id, dto);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的環境監測記錄");
        }

        return NoContent();
    }

    /// <summary>
    /// 刪除環境監測記錄
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _environmentalService.DeleteAsync(id);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的環境監測記錄");
        }

        return NoContent();
    }

    /// <summary>
    /// 搜尋環境監測記錄
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> Search([FromQuery] string? searchTerm)
    {
        var monitoring = await _environmentalService.SearchAsync(searchTerm ?? "");
        return Ok(monitoring);
    }

    /// <summary>
    /// 根據位置取得環境監測記錄
    /// </summary>
    [HttpGet("location/{location}")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetByLocation(string location)
    {
        var monitoring = await _environmentalService.GetByLocationAsync(location);
        return Ok(monitoring);
    }

    /// <summary>
    /// 根據日期範圍取得環境監測記錄
    /// </summary>
    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var monitoring = await _environmentalService.GetByDateRangeAsync(startDate, endDate);
        return Ok(monitoring);
    }

    /// <summary>
    /// 取得各位置最新的環境監測記錄
    /// </summary>
    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetLatestByLocation()
    {
        var monitoring = await _environmentalService.GetLatestByLocationAsync();
        return Ok(monitoring);
    }

    /// <summary>
    /// 取得空氣品質不良的記錄
    /// </summary>
    [HttpGet("poor-air-quality")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetPoorAirQuality()
    {
        var monitoring = await _environmentalService.GetPoorAirQualityAsync();
        return Ok(monitoring);
    }

    /// <summary>
    /// 取得極端溫度記錄
    /// </summary>
    [HttpGet("extreme-temperature")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetExtremeTemperature()
    {
        var monitoring = await _environmentalService.GetExtremeTemperatureAsync();
        return Ok(monitoring);
    }

    /// <summary>
    /// 取得高噪音記錄
    /// </summary>
    [HttpGet("high-noise")]
    public async Task<ActionResult<IEnumerable<EnvironmentalMonitoringDto>>> GetHighNoise()
    {
        var monitoring = await _environmentalService.GetHighNoiseAsync();
        return Ok(monitoring);
    }

    /// <summary>
    /// 取得環境監測統計
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<EnvironmentalStatsDto>> GetStats()
    {
        var stats = await _environmentalService.GetStatsAsync();
        return Ok(stats);
    }
}

/// <summary>
/// 環境警報 API 控制器
/// </summary>
[ApiController]
[Route("api/environmental-alerts")]
public class EnvironmentalAlertsController : ControllerBase
{
    private readonly IEnvironmentalService _environmentalService;

    public EnvironmentalAlertsController(IEnvironmentalService environmentalService)
    {
        _environmentalService = environmentalService;
    }

    /// <summary>
    /// 取得環境警報列表
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnvironmentalAlertDto>>> GetAlerts([FromQuery] bool includeResolved = false)
    {
        var alerts = await _environmentalService.GetAlertsAsync(includeResolved);
        return Ok(alerts);
    }

    /// <summary>
    /// 創建環境警報
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EnvironmentalAlertDto>> CreateAlert([FromBody] CreateEnvironmentalAlertDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var alert = await _environmentalService.CreateAlertAsync(dto);
            return CreatedAtAction(nameof(GetAlerts), alert);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 解決環境警報
    /// </summary>
    [HttpPut("{alertId}/resolve")]
    public async Task<IActionResult> ResolveAlert(int alertId, [FromBody] ResolveAlertDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _environmentalService.ResolveAlertAsync(alertId, dto.ResolvedBy, dto.Notes);
        if (!success)
        {
            return NotFound($"找不到ID為 {alertId} 的環境警報");
        }

        return NoContent();
    }
}

/// <summary>
/// 解決警報 DTO
/// </summary>
public class ResolveAlertDto
{
    public string ResolvedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}