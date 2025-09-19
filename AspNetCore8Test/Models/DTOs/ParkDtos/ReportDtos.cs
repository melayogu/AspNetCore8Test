using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 綜合統計報告 DTO
/// </summary>
public class OverviewReportDto
{
    public int TotalFacilities { get; set; }
    public int ActiveFacilities { get; set; }
    public int TotalPlants { get; set; }
    public int HealthyPlants { get; set; }
    public int TotalEvents { get; set; }
    public int ActiveEvents { get; set; }
    public int TotalVisitorActivities { get; set; }
    public int PendingMaintenance { get; set; }
    public int ActiveEmergencies { get; set; }
    public decimal AverageVisitorRating { get; set; }
    public DateTime LastUpdated { get; set; }
    
    public FacilitySummary FacilitySummary { get; set; } = new();
    public PlantSummary PlantSummary { get; set; } = new();
    public EventSummary EventSummary { get; set; } = new();
    public EnvironmentalSummary EnvironmentalSummary { get; set; } = new();
}

/// <summary>
/// 設施摘要
/// </summary>
public class FacilitySummary
{
    public int TotalCount { get; set; }
    public int GoodCondition { get; set; }
    public int NeedsRepair { get; set; }
    public int OutOfService { get; set; }
    public decimal MaintenanceCostThisMonth { get; set; }
}

/// <summary>
/// 植物摘要
/// </summary>
public class PlantSummary
{
    public int TotalCount { get; set; }
    public int Healthy { get; set; }
    public int NeedsCare { get; set; }
    public int Diseased { get; set; }
    public int RecentlyPlanted { get; set; }
}

/// <summary>
/// 事件摘要
/// </summary>
public class EventSummary
{
    public int UpcomingEvents { get; set; }
    public int CompletedEvents { get; set; }
    public int PendingMaintenance { get; set; }
    public int ResolvedEmergencies { get; set; }
}

/// <summary>
/// 環境摘要
/// </summary>
public class EnvironmentalSummary
{
    public string LatestAirQuality { get; set; } = string.Empty;
    public string LatestWaterQuality { get; set; } = string.Empty;
    public int ActiveAlerts { get; set; }
    public decimal AverageTemperature { get; set; }
    public decimal AverageHumidity { get; set; }
}

/// <summary>
/// 設施使用報告 DTO
/// </summary>
public class FacilityUsageReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<FacilityUsageData> UsageData { get; set; } = new();
    public List<MaintenanceFrequency> MaintenanceFrequency { get; set; } = new();
    public List<PopularityRanking> PopularityRankings { get; set; } = new();
    public decimal TotalMaintenanceCost { get; set; }
    public int TotalMaintenanceEvents { get; set; }
}

/// <summary>
/// 設施使用數據
/// </summary>
public class FacilityUsageData
{
    public int FacilityId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;
    public int UsageCount { get; set; }
    public int MaintenanceEvents { get; set; }
    public decimal MaintenanceCost { get; set; }
    public string ConditionStatus { get; set; } = string.Empty;
}

/// <summary>
/// 維護頻率
/// </summary>
public class MaintenanceFrequency
{
    public string FacilityType { get; set; } = string.Empty;
    public int MaintenanceCount { get; set; }
    public decimal AverageCost { get; set; }
    public decimal AverageDuration { get; set; }
}

/// <summary>
/// 受歡迎程度排名
/// </summary>
public class PopularityRanking
{
    public string ItemName { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public int UsageScore { get; set; }
    public int Rank { get; set; }
}

/// <summary>
/// 維護成本報告 DTO
/// </summary>
public class MaintenanceCostReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
    public List<CostByCategory> CostsByCategory { get; set; } = new();
    public List<MonthlyTrend> MonthlyTrends { get; set; } = new();
    public List<CostByFacility> CostsByFacility { get; set; } = new();
    public decimal AverageCostPerEvent { get; set; }
    public List<CostPrediction> Predictions { get; set; } = new();
}

/// <summary>
/// 分類成本
/// </summary>
public class CostByCategory
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int EventCount { get; set; }
    public decimal Percentage { get; set; }
}

/// <summary>
/// 月度趨勢
/// </summary>
public class MonthlyTrend
{
    public string Month { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int EventCount { get; set; }
    public decimal ChangePercentage { get; set; }
}

/// <summary>
/// 設施成本
/// </summary>
public class CostByFacility
{
    public string FacilityName { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }
    public int MaintenanceCount { get; set; }
    public decimal AverageCost { get; set; }
}

/// <summary>
/// 成本預測
/// </summary>
public class CostPrediction
{
    public string Period { get; set; } = string.Empty;
    public decimal PredictedCost { get; set; }
    public decimal ConfidenceLevel { get; set; }
}

/// <summary>
/// 遊客活動報告 DTO
/// </summary>
public class VisitorActivityReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalActivities { get; set; }
    public int TotalParticipants { get; set; }
    public decimal AverageRating { get; set; }
    public List<ActivityPopularity> ActivityPopularity { get; set; } = new();
    public List<ParticipationTrend> ParticipationTrends { get; set; } = new();
    public List<FeedbackSummary> FeedbackSummaries { get; set; } = new();
    public List<ReservationAnalysis> ReservationAnalysis { get; set; } = new();
}

/// <summary>
/// 活動受歡迎程度
/// </summary>
public class ActivityPopularity
{
    public string ActivityType { get; set; } = string.Empty;
    public int ActivityCount { get; set; }
    public int TotalParticipants { get; set; }
    public decimal AverageRating { get; set; }
    public decimal PopularityScore { get; set; }
}

/// <summary>
/// 參與趨勢
/// </summary>
public class ParticipationTrend
{
    public string Period { get; set; } = string.Empty;
    public int Participants { get; set; }
    public int Activities { get; set; }
    public decimal AverageParticipantsPerActivity { get; set; }
}

/// <summary>
/// 回饋摘要
/// </summary>
public class FeedbackSummary
{
    public string FeedbackType { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal AverageRating { get; set; }
    public List<string> CommonComments { get; set; } = new();
}

/// <summary>
/// 預約分析
/// </summary>
public class ReservationAnalysis
{
    public string ActivityType { get; set; } = string.Empty;
    public int TotalReservations { get; set; }
    public int ConfirmedReservations { get; set; }
    public int CancelledReservations { get; set; }
    public decimal ConfirmationRate { get; set; }
    public decimal CancellationRate { get; set; }
}

/// <summary>
/// 環境監測報告 DTO
/// </summary>
public class EnvironmentalReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AirQualityAnalysis AirQuality { get; set; } = new();
    public WaterQualityAnalysis WaterQuality { get; set; } = new();
    public WeatherAnalysis Weather { get; set; } = new();
    public List<EnvironmentalAlert> Alerts { get; set; } = new();
    public List<TrendData> Trends { get; set; } = new();
}

/// <summary>
/// 空氣品質分析
/// </summary>
public class AirQualityAnalysis
{
    public decimal AverageAqi { get; set; }
    public decimal AveragePm25 { get; set; }
    public decimal AveragePm10 { get; set; }
    public string OverallStatus { get; set; } = string.Empty;
    public int GoodDays { get; set; }
    public int ModerateDays { get; set; }
    public int UnhealthyDays { get; set; }
}

/// <summary>
/// 水質分析
/// </summary>
public class WaterQualityAnalysis
{
    public decimal AveragePh { get; set; }
    public decimal AverageDissolvedOxygen { get; set; }
    public decimal AverageTurbidity { get; set; }
    public string OverallStatus { get; set; } = string.Empty;
    public int ExcellentDays { get; set; }
    public int GoodDays { get; set; }
    public int PoorDays { get; set; }
}

/// <summary>
/// 天氣分析
/// </summary>
public class WeatherAnalysis
{
    public decimal AverageTemperature { get; set; }
    public decimal AverageHumidity { get; set; }
    public decimal TotalRainfall { get; set; }
    public int SunnyDays { get; set; }
    public int RainyDays { get; set; }
    public int CloudyDays { get; set; }
}

/// <summary>
/// 環境警報
/// </summary>
public class EnvironmentalAlert
{
    public string AlertType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// 趨勢數據
/// </summary>
public class TrendData
{
    public string Metric { get; set; } = string.Empty;
    public List<DataPoint> DataPoints { get; set; } = new();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal ChangePercentage { get; set; }
}

/// <summary>
/// 數據點
/// </summary>
public class DataPoint
{
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public string Label { get; set; } = string.Empty;
}

/// <summary>
/// 事件報告 DTO
/// </summary>
public class EventReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventStatistics Statistics { get; set; } = new();
    public List<EventTypeAnalysis> EventTypeAnalysis { get; set; } = new();
    public List<EventOutcome> EventOutcomes { get; set; } = new();
    public List<ResponseTimeAnalysis> ResponseTimes { get; set; } = new();
}

/// <summary>
/// 事件統計
/// </summary>
public class EventStatistics
{
    public int TotalParkEvents { get; set; }
    public int TotalMaintenanceEvents { get; set; }
    public int TotalEmergencyEvents { get; set; }
    public int CompletedEvents { get; set; }
    public int CancelledEvents { get; set; }
    public decimal SuccessRate { get; set; }
}

/// <summary>
/// 事件類型分析
/// </summary>
public class EventTypeAnalysis
{
    public string EventType { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal AverageDuration { get; set; }
    public decimal SuccessRate { get; set; }
    public decimal AverageCost { get; set; }
}

/// <summary>
/// 事件結果
/// </summary>
public class EventOutcome
{
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Participants { get; set; }
    public decimal Rating { get; set; }
    public decimal Cost { get; set; }
}

/// <summary>
/// 回應時間分析
/// </summary>
public class ResponseTimeAnalysis
{
    public string EventType { get; set; } = string.Empty;
    public decimal AverageResponseTime { get; set; }
    public decimal FastestResponse { get; set; }
    public decimal SlowestResponse { get; set; }
    public string ResponseTimeRating { get; set; } = string.Empty;
}

/// <summary>
/// 植物健康報告 DTO
/// </summary>
public class PlantHealthReportDto
{
    public PlantHealthOverview Overview { get; set; } = new();
    public List<PlantTypeHealth> PlantTypeHealth { get; set; } = new();
    public List<DiseaseAnalysis> DiseaseAnalysis { get; set; } = new();
    public List<CareRecommendation> CareRecommendations { get; set; } = new();
    public List<PlantGrowthTrend> GrowthTrends { get; set; } = new();
}

/// <summary>
/// 植物健康概況
/// </summary>
public class PlantHealthOverview
{
    public int TotalPlants { get; set; }
    public int HealthyPlants { get; set; }
    public int PlantsNeedingCare { get; set; }
    public int DiseasedPlants { get; set; }
    public decimal OverallHealthScore { get; set; }
    public int RecentlyPlanted { get; set; }
}

/// <summary>
/// 植物類型健康狀況
/// </summary>
public class PlantTypeHealth
{
    public string PlantType { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public int HealthyCount { get; set; }
    public decimal HealthPercentage { get; set; }
    public string CommonIssues { get; set; } = string.Empty;
}

/// <summary>
/// 疾病分析
/// </summary>
public class DiseaseAnalysis
{
    public string DiseaseName { get; set; } = string.Empty;
    public int AffectedPlants { get; set; }
    public List<string> AffectedSpecies { get; set; } = new();
    public string Severity { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
}

/// <summary>
/// 照護建議
/// </summary>
public class CareRecommendation
{
    public string PlantType { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime SuggestedDate { get; set; }
    public string CareType { get; set; } = string.Empty;
}

/// <summary>
/// 植物生長趨勢
/// </summary>
public class PlantGrowthTrend
{
    public string PlantType { get; set; } = string.Empty;
    public List<GrowthData> GrowthData { get; set; } = new();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal GrowthRate { get; set; }
}

/// <summary>
/// 生長數據
/// </summary>
public class GrowthData
{
    public DateTime Date { get; set; }
    public decimal Height { get; set; }
    public string HealthStatus { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 月度報告 DTO
/// </summary>
public class MonthlyReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public MonthlySummary Summary { get; set; } = new();
    public List<MonthlyMetric> Metrics { get; set; } = new();
    public List<Achievement> Achievements { get; set; } = new();
    public List<Challenge> Challenges { get; set; } = new();
    public List<Improvement> Improvements { get; set; } = new();
}

/// <summary>
/// 月度摘要
/// </summary>
public class MonthlySummary
{
    public int TotalEvents { get; set; }
    public int VisitorActivities { get; set; }
    public decimal MaintenanceCost { get; set; }
    public int EmergencyIncidents { get; set; }
    public decimal VisitorSatisfaction { get; set; }
    public string OverallRating { get; set; } = string.Empty;
}

/// <summary>
/// 月度指標
/// </summary>
public class MonthlyMetric
{
    public string MetricName { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal ChangePercentage { get; set; }
    public string ChangeDirection { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// 成就
/// </summary>
public class Achievement
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AchievedDate { get; set; }
    public string Category { get; set; } = string.Empty;
}

/// <summary>
/// 挑戰
/// </summary>
public class Challenge
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty;
    public string ProposedSolution { get; set; } = string.Empty;
}

/// <summary>
/// 改進
/// </summary>
public class Improvement
{
    public string Area { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
}

/// <summary>
/// 年度報告 DTO
/// </summary>
public class YearlyReportDto
{
    public int Year { get; set; }
    public YearlySummary Summary { get; set; } = new();
    public List<QuarterlyComparison> QuarterlyComparisons { get; set; } = new();
    public List<AnnualTrend> AnnualTrends { get; set; } = new();
    public List<YearlyAchievement> Achievements { get; set; } = new();
    public YearlyFinancials Financials { get; set; } = new();
    public List<FutureProjection> FutureProjections { get; set; } = new();
}

/// <summary>
/// 年度摘要
/// </summary>
public class YearlySummary
{
    public int TotalEvents { get; set; }
    public int TotalVisitors { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCosts { get; set; }
    public decimal NetIncome { get; set; }
    public decimal VisitorSatisfaction { get; set; }
    public int NewPlantations { get; set; }
    public int FacilityImprovements { get; set; }
}

/// <summary>
/// 季度比較
/// </summary>
public class QuarterlyComparison
{
    public string Quarter { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Costs { get; set; }
    public int Events { get; set; }
    public int Visitors { get; set; }
    public decimal Satisfaction { get; set; }
}

/// <summary>
/// 年度趨勢
/// </summary>
public class AnnualTrend
{
    public string Metric { get; set; } = string.Empty;
    public List<MonthlyData> MonthlyData { get; set; } = new();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal YearOverYearChange { get; set; }
}

/// <summary>
/// 月度數據
/// </summary>
public class MonthlyData
{
    public string Month { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal ChangeFromPrevious { get; set; }
}

/// <summary>
/// 年度成就
/// </summary>
public class YearlyAchievement
{
    public string Category { get; set; } = string.Empty;
    public string Achievement { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

/// <summary>
/// 年度財務
/// </summary>
public class YearlyFinancials
{
    public decimal TotalBudget { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal BudgetUtilization { get; set; }
    public List<ExpenseCategory> ExpenseBreakdown { get; set; } = new();
    public List<RevenueSource> RevenueBreakdown { get; set; } = new();
}

/// <summary>
/// 支出類別
/// </summary>
public class ExpenseCategory
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public decimal BudgetedAmount { get; set; }
    public decimal Variance { get; set; }
}

/// <summary>
/// 收入來源
/// </summary>
public class RevenueSource
{
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal Achievement { get; set; }
}

/// <summary>
/// 未來預測
/// </summary>
public class FutureProjection
{
    public string Metric { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal ProjectedValue { get; set; }
    public decimal ConfidenceLevel { get; set; }
    public string Timeframe { get; set; } = string.Empty;
}

/// <summary>
/// 趨勢分析 DTO
/// </summary>
public class TrendAnalysisDto
{
    public string Metric { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<TrendDataPoint> DataPoints { get; set; } = new();
    public TrendStatistics Statistics { get; set; } = new();
    public List<TrendInsight> Insights { get; set; } = new();
}

/// <summary>
/// 趨勢數據點
/// </summary>
public class TrendDataPoint
{
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public decimal MovingAverage { get; set; }
    public decimal Deviation { get; set; }
}

/// <summary>
/// 趨勢統計
/// </summary>
public class TrendStatistics
{
    public decimal Average { get; set; }
    public decimal Minimum { get; set; }
    public decimal Maximum { get; set; }
    public decimal StandardDeviation { get; set; }
    public decimal TrendSlope { get; set; }
    public string TrendDirection { get; set; } = string.Empty;
    public decimal CorrelationCoefficient { get; set; }
}

/// <summary>
/// 趨勢洞察
/// </summary>
public class TrendInsight
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Significance { get; set; }
    public DateTime DetectedAt { get; set; }
}

/// <summary>
/// 效率分析 DTO
/// </summary>
public class EfficiencyAnalysisDto
{
    public OperationalEfficiency Operational { get; set; } = new();
    public ResourceEfficiency Resource { get; set; } = new();
    public CostEfficiency Cost { get; set; } = new();
    public List<EfficiencyRecommendation> Recommendations { get; set; } = new();
    public List<BenchmarkComparison> Benchmarks { get; set; } = new();
}

/// <summary>
/// 營運效率
/// </summary>
public class OperationalEfficiency
{
    public decimal TaskCompletionRate { get; set; }
    public decimal AverageResponseTime { get; set; }
    public decimal ResourceUtilization { get; set; }
    public decimal ProcessEfficiency { get; set; }
    public string OverallRating { get; set; } = string.Empty;
}

/// <summary>
/// 資源效率
/// </summary>
public class ResourceEfficiency
{
    public decimal StaffUtilization { get; set; }
    public decimal EquipmentUtilization { get; set; }
    public decimal SpaceUtilization { get; set; }
    public decimal EnergyEfficiency { get; set; }
    public string OverallRating { get; set; } = string.Empty;
}

/// <summary>
/// 成本效率
/// </summary>
public class CostEfficiency
{
    public decimal CostPerVisitor { get; set; }
    public decimal CostPerEvent { get; set; }
    public decimal MaintenanceCostRatio { get; set; }
    public decimal BudgetEfficiency { get; set; }
    public string OverallRating { get; set; } = string.Empty;
}

/// <summary>
/// 效率建議
/// </summary>
public class EfficiencyRecommendation
{
    public string Area { get; set; } = string.Empty;
    public string CurrentIssue { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public decimal ExpectedImprovement { get; set; }
    public decimal ImplementationCost { get; set; }
    public string Priority { get; set; } = string.Empty;
}

/// <summary>
/// 基準比較
/// </summary>
public class BenchmarkComparison
{
    public string Metric { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal BenchmarkValue { get; set; }
    public decimal Variance { get; set; }
    public string Performance { get; set; } = string.Empty;
}

/// <summary>
/// 檔案數據 DTO
/// </summary>
public class FileDataDto
{
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

/// <summary>
/// 預測分析 DTO
/// </summary>
public class PredictionAnalysisDto
{
    public string Metric { get; set; } = string.Empty;
    public DateTime ForecastStartDate { get; set; }
    public DateTime ForecastEndDate { get; set; }
    public List<PredictionDataPoint> Predictions { get; set; } = new();
    public PredictionAccuracy Accuracy { get; set; } = new();
    public List<PredictionScenario> Scenarios { get; set; } = new();
}

/// <summary>
/// 預測數據點
/// </summary>
public class PredictionDataPoint
{
    public DateTime Date { get; set; }
    public decimal PredictedValue { get; set; }
    public decimal ConfidenceIntervalLower { get; set; }
    public decimal ConfidenceIntervalUpper { get; set; }
    public decimal Probability { get; set; }
}

/// <summary>
/// 預測準確性
/// </summary>
public class PredictionAccuracy
{
    public decimal MeanAbsoluteError { get; set; }
    public decimal RootMeanSquareError { get; set; }
    public decimal AccuracyPercentage { get; set; }
    public string ModelType { get; set; } = string.Empty;
}

/// <summary>
/// 預測情境
/// </summary>
public class PredictionScenario
{
    public string Scenario { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal ProbabilityOfOccurrence { get; set; }
    public decimal ImpactMagnitude { get; set; }
    public List<string> Recommendations { get; set; } = new();
}

/// <summary>
/// 成本效益分析 DTO
/// </summary>
public class CostBenefitAnalysisDto
{
    public DateTime AnalysisDate { get; set; }
    public CostAnalysis Costs { get; set; } = new();
    public BenefitAnalysis Benefits { get; set; } = new();
    public decimal NetPresentValue { get; set; }
    public decimal ReturnOnInvestment { get; set; }
    public decimal PaybackPeriod { get; set; }
    public string RecommendationSummary { get; set; } = string.Empty;
}

/// <summary>
/// 成本分析
/// </summary>
public class CostAnalysis
{
    public decimal TotalCosts { get; set; }
    public decimal OperationalCosts { get; set; }
    public decimal MaintenanceCosts { get; set; }
    public decimal StaffCosts { get; set; }
    public decimal UtilityCosts { get; set; }
    public decimal OtherCosts { get; set; }
}

/// <summary>
/// 效益分析
/// </summary>
public class BenefitAnalysis
{
    public decimal TotalBenefits { get; set; }
    public decimal RevenueGenerated { get; set; }
    public decimal CostSavings { get; set; }
    public decimal EnvironmentalBenefits { get; set; }
    public decimal SocialBenefits { get; set; }
    public decimal IntangibleBenefits { get; set; }
}

/// <summary>
/// KPI 報告 DTO
/// </summary>
public class KpiReportDto
{
    public DateTime ReportDate { get; set; }
    public List<Kpi> Kpis { get; set; } = new();
    public KpiSummary Summary { get; set; } = new();
    public List<KpiTrend> Trends { get; set; } = new();
    public string OverallPerformance { get; set; } = string.Empty;
}

/// <summary>
/// KPI
/// </summary>
public class Kpi
{
    public string Name { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal TargetValue { get; set; }
    public decimal PreviousValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal PerformanceRatio { get; set; }
    public string Trend { get; set; } = string.Empty;
}

/// <summary>
/// KPI 摘要
/// </summary>
public class KpiSummary
{
    public int TotalKpis { get; set; }
    public int KpisOnTarget { get; set; }
    public int KpisAboveTarget { get; set; }
    public int KpisBelowTarget { get; set; }
    public decimal OverallPerformanceScore { get; set; }
}

/// <summary>
/// KPI 趨勢
/// </summary>
public class KpiTrend
{
    public string KpiName { get; set; } = string.Empty;
    public List<KpiDataPoint> DataPoints { get; set; } = new();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal ChangeRate { get; set; }
}

/// <summary>
/// KPI 數據點
/// </summary>
public class KpiDataPoint
{
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public decimal Target { get; set; }
}

/// <summary>
/// 自定義報告請求 DTO
/// </summary>
public class CustomReportRequestDto
{
    [Required]
    public string ReportName { get; set; } = string.Empty;
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    [Required]
    public List<string> Metrics { get; set; } = new();
    
    public List<string> Filters { get; set; } = new();
    public List<string> GroupBy { get; set; } = new();
    public string SortBy { get; set; } = string.Empty;
    public string SortDirection { get; set; } = "asc";
    public bool IncludeCharts { get; set; } = true;
    public bool IncludeTrends { get; set; } = true;
    public string OutputFormat { get; set; } = "json";
}

/// <summary>
/// 自定義報告 DTO
/// </summary>
public class CustomReportDto
{
    public string ReportName { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CustomMetric> Metrics { get; set; } = new();
    public List<CustomChart> Charts { get; set; } = new();
    public List<CustomTable> Tables { get; set; } = new();
    public Dictionary<string, object> Filters { get; set; } = new();
}

/// <summary>
/// 自定義指標
/// </summary>
public class CustomMetric
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<CustomDataPoint> DataPoints { get; set; } = new();
}

/// <summary>
/// 自定義數據點
/// </summary>
public class CustomDataPoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime? Date { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();
}

/// <summary>
/// 自定義圖表
/// </summary>
public class CustomChart
{
    public string ChartType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<ChartSeries> Series { get; set; } = new();
    public ChartOptions Options { get; set; } = new();
}

/// <summary>
/// 圖表系列
/// </summary>
public class ChartSeries
{
    public string Name { get; set; } = string.Empty;
    public List<decimal> Data { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public string Color { get; set; } = string.Empty;
}

/// <summary>
/// 圖表選項
/// </summary>
public class ChartOptions
{
    public string XAxisTitle { get; set; } = string.Empty;
    public string YAxisTitle { get; set; } = string.Empty;
    public bool ShowLegend { get; set; } = true;
    public bool ShowGrid { get; set; } = true;
    public string Theme { get; set; } = "default";
}

/// <summary>
/// 自定義表格
/// </summary>
public class CustomTable
{
    public string Title { get; set; } = string.Empty;
    public List<string> Headers { get; set; } = new();
    public List<List<object>> Rows { get; set; } = new();
    public CustomTableOptions Options { get; set; } = new();
}

/// <summary>
/// 自定義表格選項
/// </summary>
public class CustomTableOptions
{
    public bool ShowBorders { get; set; } = true;
    public bool AllowSorting { get; set; } = true;
    public bool AllowFiltering { get; set; } = true;
    public int PageSize { get; set; } = 10;
}