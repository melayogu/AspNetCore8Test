using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public class MockCaseService : ICaseService
    {
        private static readonly List<Case> _cases = new List<Case>
        {
            new Case
            {
                Id = 1,
                CaseNumber = "LS20240001",
                CaseName = "王小明所有權移轉登記",
                CaseType = CaseType.OwnershipTransfer,
                Status = CaseStatus.Processing,
                CustomerId = 1,
                ReceivedDate = DateTime.Now.AddDays(-5),
                ExpectedCompletionDate = DateTime.Now.AddDays(10),
                Description = "台北市大安區房屋所有權移轉登記",
                Fee = 15000,
                PaidAmount = 5000,
                CreatedDate = DateTime.Now.AddDays(-5),
                Notes = "急件處理"
            },
            new Case
            {
                Id = 2,
                CaseNumber = "LS20240002", 
                CaseName = "李美華抵押權設定",
                CaseType = CaseType.MortgageRegistration,
                Status = CaseStatus.Received,
                CustomerId = 2,
                ReceivedDate = DateTime.Now.AddDays(-3),
                ExpectedCompletionDate = DateTime.Now.AddDays(7),
                Description = "新北市板橋區抵押權設定登記",
                Fee = 8000,
                PaidAmount = 8000,
                CreatedDate = DateTime.Now.AddDays(-3)
            },
            new Case
            {
                Id = 3,
                CaseNumber = "LS20240003",
                CaseName = "張志明土地測量",
                CaseType = CaseType.Survey,
                Status = CaseStatus.Completed,
                CustomerId = 3,
                ReceivedDate = DateTime.Now.AddDays(-20),
                ExpectedCompletionDate = DateTime.Now.AddDays(-10),
                ActualCompletionDate = DateTime.Now.AddDays(-8),
                Description = "桃園市龜山區土地界址測量",
                Fee = 12000,
                PaidAmount = 12000,
                CreatedDate = DateTime.Now.AddDays(-20)
            }
        };

        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "王小明", IdNumber = "A123456789", Phone = "0912345678" },
            new Customer { Id = 2, Name = "李美華", IdNumber = "B987654321", Phone = "0987654321" },
            new Customer { Id = 3, Name = "張志明", IdNumber = "C111222333", Phone = "0955666777" }
        };

        public async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            await Task.Delay(10); // 模擬異步操作
            
            // 設定客戶導航屬性
            foreach (var caseItem in _cases)
            {
                caseItem.Customer = _customers.FirstOrDefault(c => c.Id == caseItem.CustomerId) ?? new Customer();
            }
            
            return _cases.OrderByDescending(c => c.CreatedDate);
        }

        public async Task<Case?> GetCaseByIdAsync(int id)
        {
            await Task.Delay(10);
            var caseItem = _cases.FirstOrDefault(c => c.Id == id);
            if (caseItem != null)
            {
                caseItem.Customer = _customers.FirstOrDefault(c => c.Id == caseItem.CustomerId) ?? new Customer();
            }
            return caseItem;
        }

        public async Task<Case> CreateCaseAsync(Case caseEntity)
        {
            await Task.Delay(10);
            caseEntity.Id = _cases.Max(c => c.Id) + 1;
            caseEntity.CreatedDate = DateTime.Now;
            caseEntity.Status = CaseStatus.Received;
            _cases.Add(caseEntity);
            return caseEntity;
        }

        public async Task<Case> UpdateCaseAsync(Case caseEntity)
        {
            await Task.Delay(10);
            var existingCase = _cases.FirstOrDefault(c => c.Id == caseEntity.Id);
            if (existingCase != null)
            {
                existingCase.CaseNumber = caseEntity.CaseNumber;
                existingCase.CaseName = caseEntity.CaseName;
                existingCase.CaseType = caseEntity.CaseType;
                existingCase.Status = caseEntity.Status;
                existingCase.CustomerId = caseEntity.CustomerId;
                existingCase.ReceivedDate = caseEntity.ReceivedDate;
                existingCase.ExpectedCompletionDate = caseEntity.ExpectedCompletionDate;
                existingCase.ActualCompletionDate = caseEntity.ActualCompletionDate;
                existingCase.Description = caseEntity.Description;
                existingCase.Fee = caseEntity.Fee;
                existingCase.PaidAmount = caseEntity.PaidAmount;
                existingCase.LastModifiedDate = DateTime.Now;
                existingCase.Notes = caseEntity.Notes;
                return existingCase;
            }
            throw new ArgumentException("案件不存在");
        }

        public async Task<bool> DeleteCaseAsync(int id)
        {
            await Task.Delay(10);
            var caseToRemove = _cases.FirstOrDefault(c => c.Id == id);
            if (caseToRemove != null)
            {
                return _cases.Remove(caseToRemove);
            }
            return false;
        }

        public async Task<bool> CaseExistsAsync(int id)
        {
            await Task.Delay(10);
            return _cases.Any(c => c.Id == id);
        }

        public async Task<bool> CaseNumberExistsAsync(string caseNumber, int? excludeId = null)
        {
            await Task.Delay(10);
            return _cases.Any(c => c.CaseNumber == caseNumber && (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<IEnumerable<Case>> GetCasesByCustomerIdAsync(int customerId)
        {
            await Task.Delay(10);
            var cases = _cases.Where(c => c.CustomerId == customerId).ToList();
            foreach (var caseItem in cases)
            {
                caseItem.Customer = _customers.FirstOrDefault(c => c.Id == caseItem.CustomerId) ?? new Customer();
            }
            return cases.OrderByDescending(c => c.CreatedDate);
        }

        public async Task<IEnumerable<Case>> GetCasesByStatusAsync(CaseStatus status)
        {
            await Task.Delay(10);
            var cases = _cases.Where(c => c.Status == status).ToList();
            foreach (var caseItem in cases)
            {
                caseItem.Customer = _customers.FirstOrDefault(c => c.Id == caseItem.CustomerId) ?? new Customer();
            }
            return cases.OrderByDescending(c => c.CreatedDate);
        }

        public async Task<IEnumerable<Case>> GetCasesByTypeAsync(CaseType caseType)
        {
            await Task.Delay(10);
            var cases = _cases.Where(c => c.CaseType == caseType).ToList();
            foreach (var caseItem in cases)
            {
                caseItem.Customer = _customers.FirstOrDefault(c => c.Id == caseItem.CustomerId) ?? new Customer();
            }
            return cases.OrderByDescending(c => c.CreatedDate);
        }
    }
}