using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 報告分析控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// 取得綜合統計報告
    /// </summary>
    [HttpGet("overview")]
    public async Task<ActionResult<OverviewReportDto>> GetOverviewReport()
    {
        var report = await _reportService.GetOverviewReportAsync();
        return Ok(report);
    }

    /// <summary>
    /// 取得設施使用報告
    /// </summary>
    [HttpGet("facility-usage")]
    public async Task<ActionResult<FacilityUsageReportDto>> GetFacilityUsageReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetFacilityUsageReportAsync(startDate, endDate);
        return Ok(report);
    }

    /// <summary>
    /// 取得維護成本報告
    /// </summary>
    [HttpGet("maintenance-costs")]
    public async Task<ActionResult<MaintenanceCostReportDto>> GetMaintenanceCostReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetMaintenanceCostReportAsync(startDate, endDate);
        return Ok(report);
    }

    /// <summary>
    /// 取得遊客活動報告
    /// </summary>
    [HttpGet("visitor-activities")]
    public async Task<ActionResult<VisitorActivityReportDto>> GetVisitorActivityReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetVisitorActivityReportAsync(startDate, endDate);
        return Ok(report);
    }

    /// <summary>
    /// 取得環境監測報告
    /// </summary>
    [HttpGet("environmental")]
    public async Task<ActionResult<EnvironmentalReportDto>> GetEnvironmentalReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetEnvironmentalReportAsync(startDate, endDate);
        return Ok(report);
    }

    /// <summary>
    /// 取得事件統計報告
    /// </summary>
    [HttpGet("events")]
    public async Task<ActionResult<EventReportDto>> GetEventReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetEventReportAsync(startDate, endDate);
        return Ok(report);
    }

    /// <summary>
    /// 取得植物健康報告
    /// </summary>
    [HttpGet("plant-health")]
    public async Task<ActionResult<PlantHealthReportDto>> GetPlantHealthReport()
    {
        var report = await _reportService.GetPlantHealthReportAsync();
        return Ok(report);
    }

    /// <summary>
    /// 取得月度報告
    /// </summary>
    [HttpGet("monthly/{year}/{month}")]
    public async Task<ActionResult<MonthlyReportDto>> GetMonthlyReport(int year, int month)
    {
        var report = await _reportService.GetMonthlyReportAsync(year, month);
        return Ok(report);
    }

    /// <summary>
    /// 取得年度報告
    /// </summary>
    [HttpGet("yearly/{year}")]
    public async Task<ActionResult<YearlyReportDto>> GetYearlyReport(int year)
    {
        var report = await _reportService.GetYearlyReportAsync(year);
        return Ok(report);
    }

    /// <summary>
    /// 取得趨勢分析
    /// </summary>
    [HttpGet("trends")]
    public async Task<ActionResult<TrendAnalysisDto>> GetTrendAnalysis([FromQuery] string metric, [FromQuery] int days = 30)
    {
        var analysis = await _reportService.GetTrendAnalysisAsync(metric, days);
        return Ok(analysis);
    }

    /// <summary>
    /// 取得效率分析
    /// </summary>
    [HttpGet("efficiency")]
    public async Task<ActionResult<EfficiencyAnalysisDto>> GetEfficiencyAnalysis()
    {
        var analysis = await _reportService.GetEfficiencyAnalysisAsync();
        return Ok(analysis);
    }

    /// <summary>
    /// 匯出報告為 Excel
    /// </summary>
    [HttpGet("export/{reportType}")]
    public async Task<IActionResult> ExportReport(string reportType, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var fileData = await _reportService.ExportReportAsync(reportType, startDate, endDate);
            
            return File(
                fileData.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileData.FileName
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 取得預測分析
    /// </summary>
    [HttpGet("predictions")]
    public async Task<ActionResult<PredictionAnalysisDto>> GetPredictionAnalysis([FromQuery] string metric, [FromQuery] int forecastDays = 30)
    {
        var analysis = await _reportService.GetPredictionAnalysisAsync(metric, forecastDays);
        return Ok(analysis);
    }

    /// <summary>
    /// 取得成本效益分析
    /// </summary>
    [HttpGet("cost-benefit")]
    public async Task<ActionResult<CostBenefitAnalysisDto>> GetCostBenefitAnalysis([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var analysis = await _reportService.GetCostBenefitAnalysisAsync(startDate, endDate);
        return Ok(analysis);
    }

    /// <summary>
    /// 取得關鍵效能指標 (KPI)
    /// </summary>
    [HttpGet("kpis")]
    public async Task<ActionResult<KpiReportDto>> GetKpiReport()
    {
        var report = await _reportService.GetKpiReportAsync();
        return Ok(report);
    }

    /// <summary>
    /// 取得自定義報告
    /// </summary>
    [HttpPost("custom")]
    public async Task<ActionResult<CustomReportDto>> GetCustomReport([FromBody] CustomReportRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var report = await _reportService.GetCustomReportAsync(request);
            return Ok(report);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}