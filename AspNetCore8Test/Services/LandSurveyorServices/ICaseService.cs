using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public interface ICaseService
    {
        Task<IEnumerable<Case>> GetAllCasesAsync();
        Task<Case?> GetCaseByIdAsync(int id);
        Task<Case> CreateCaseAsync(Case caseEntity);
        Task<Case> UpdateCaseAsync(Case caseEntity);
        Task<bool> DeleteCaseAsync(int id);
        Task<bool> CaseExistsAsync(int id);
        Task<bool> CaseNumberExistsAsync(string caseNumber, int? excludeId = null);
        Task<IEnumerable<Case>> GetCasesByCustomerIdAsync(int customerId);
        Task<IEnumerable<Case>> GetCasesByStatusAsync(CaseStatus status);
        Task<IEnumerable<Case>> GetCasesByTypeAsync(CaseType caseType);
    }
}