using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 報告服務介面 (暫時存根)
/// </summary>
public interface IReportService
{
    Task<OverviewReportDto> GetOverviewReportAsync();
    Task<FacilityUsageReportDto> GetFacilityUsageReportAsync(DateTime? startDate, DateTime? endDate);
    Task<MaintenanceCostReportDto> GetMaintenanceCostReportAsync(DateTime? startDate, DateTime? endDate);
    Task<VisitorActivityReportDto> GetVisitorActivityReportAsync(DateTime? startDate, DateTime? endDate);
    Task<EnvironmentalReportDto> GetEnvironmentalReportAsync(DateTime? startDate, DateTime? endDate);
    Task<EventReportDto> GetEventReportAsync(DateTime? startDate, DateTime? endDate);
    Task<PlantHealthReportDto> GetPlantHealthReportAsync();
    Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month);
    Task<YearlyReportDto> GetYearlyReportAsync(int year);
    Task<TrendAnalysisDto> GetTrendAnalysisAsync(string metric, int days);
    Task<EfficiencyAnalysisDto> GetEfficiencyAnalysisAsync();
    Task<PredictionAnalysisDto> GetPredictionAnalysisAsync(string metric, int forecastDays);
    Task<CostBenefitAnalysisDto> GetCostBenefitAnalysisAsync(DateTime? startDate, DateTime? endDate);
    Task<KpiReportDto> GetKpiReportAsync();
    Task<CustomReportDto> GetCustomReportAsync(CustomReportRequestDto request);
    Task<(byte[] Data, string FileName)> ExportReportAsync(string reportType, DateTime? startDate, DateTime? endDate);
}

/// <summary>
/// 報告服務實作 (暫時存根)
/// </summary>
public class ReportService : IReportService
{
    public Task<OverviewReportDto> GetOverviewReportAsync()
    {
        return Task.FromResult(new OverviewReportDto());
    }

    public Task<FacilityUsageReportDto> GetFacilityUsageReportAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new FacilityUsageReportDto());
    }

    public Task<MaintenanceCostReportDto> GetMaintenanceCostReportAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new MaintenanceCostReportDto());
    }

    public Task<VisitorActivityReportDto> GetVisitorActivityReportAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new VisitorActivityReportDto());
    }

    public Task<EnvironmentalReportDto> GetEnvironmentalReportAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new EnvironmentalReportDto());
    }

    public Task<EventReportDto> GetEventReportAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new EventReportDto());
    }

    public Task<PlantHealthReportDto> GetPlantHealthReportAsync()
    {
        return Task.FromResult(new PlantHealthReportDto());
    }

    public Task<MonthlyReportDto> GetMonthlyReportAsync(int year, int month)
    {
        return Task.FromResult(new MonthlyReportDto());
    }

    public Task<YearlyReportDto> GetYearlyReportAsync(int year)
    {
        return Task.FromResult(new YearlyReportDto());
    }

    public Task<TrendAnalysisDto> GetTrendAnalysisAsync(string metric, int days)
    {
        return Task.FromResult(new TrendAnalysisDto());
    }

    public Task<EfficiencyAnalysisDto> GetEfficiencyAnalysisAsync()
    {
        return Task.FromResult(new EfficiencyAnalysisDto());
    }

    public Task<PredictionAnalysisDto> GetPredictionAnalysisAsync(string metric, int forecastDays)
    {
        return Task.FromResult(new PredictionAnalysisDto());
    }

    public Task<CostBenefitAnalysisDto> GetCostBenefitAnalysisAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(new CostBenefitAnalysisDto());
    }

    public Task<KpiReportDto> GetKpiReportAsync()
    {
        return Task.FromResult(new KpiReportDto());
    }

    public Task<CustomReportDto> GetCustomReportAsync(CustomReportRequestDto request)
    {
        return Task.FromResult(new CustomReportDto());
    }

    public Task<(byte[] Data, string FileName)> ExportReportAsync(string reportType, DateTime? startDate, DateTime? endDate)
    {
        var emptyData = new byte[0];
        return Task.FromResult((emptyData, "report.xlsx"));
    }
}