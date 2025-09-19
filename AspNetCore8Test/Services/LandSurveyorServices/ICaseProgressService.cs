using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public interface ICaseProgressService
    {
        Task<IEnumerable<CaseProgress>> GetAllAsync();
        Task<CaseProgress?> GetByIdAsync(int id);
        Task<IEnumerable<CaseProgress>> GetByCaseIdAsync(int caseId);
        Task<CaseProgress> CreateAsync(CaseProgress caseProgress);
        Task<CaseProgress> UpdateAsync(CaseProgress caseProgress);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<string>> GetWorkflowTemplateAsync(string caseType);
        Task<bool> InitializeWorkflowAsync(int caseId, string caseType);
        Task<Dictionary<CaseStatus, int>> GetProgressStatisticsAsync();
    }
}