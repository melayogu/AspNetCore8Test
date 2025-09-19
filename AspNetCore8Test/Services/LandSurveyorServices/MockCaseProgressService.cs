using AspNetCore8Test.Models.LandSurveyorModels;

namespace AspNetCore8Test.Services.LandSurveyorServices
{
    public class MockCaseProgressService : ICaseProgressService
    {
        private static List<CaseProgress> _caseProgresses = new List<CaseProgress>();
        private static int _nextId = 1;

        static MockCaseProgressService()
        {
            InitializeSampleData();
        }

        private static void InitializeSampleData()
        {
            var sampleProgresses = new List<CaseProgress>
            {
                new CaseProgress
                {
                    Id = _nextId++,
                    CaseId = 1,
                    Title = "案件受理與資料審查",
                    Description = "接收客戶申請文件，進行初步資料完整性檢查",
                    Status = CaseStatus.Completed,
                    CompletedDate = DateTime.Now.AddDays(-5),
                    CreatedDate = DateTime.Now.AddDays(-10),
                    AssignedTo = "張地政",
                    Notes = "文件齊全，可進入下一階段"
                },
                new CaseProgress
                {
                    Id = _nextId++,
                    CaseId = 1,
                    Title = "現場會勘安排",
                    Description = "安排現場會勘時間，通知相關當事人",
                    Status = CaseStatus.Processing,
                    CreatedDate = DateTime.Now.AddDays(-3),
                    AssignedTo = "李測量",
                    Notes = "已聯繫客戶，預定明日進行會勘"
                },
                new CaseProgress
                {
                    Id = _nextId++,
                    CaseId = 2,
                    Title = "建物現況調查",
                    Description = "進行建物結構與現況調查作業",
                    Status = CaseStatus.Received,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    AssignedTo = "王建築",
                    Notes = "等待業主配合時間安排"
                },
                new CaseProgress
                {
                    Id = _nextId++,
                    CaseId = 3,
                    Title = "地籍資料查詢",
                    Description = "至地政事務所查詢相關地籍資料",
                    Status = CaseStatus.Completed,
                    CompletedDate = DateTime.Now.AddDays(-2),
                    CreatedDate = DateTime.Now.AddDays(-4),
                    AssignedTo = "陳助理",
                    Notes = "已取得完整地籍資料"
                }
            };

            _caseProgresses.AddRange(sampleProgresses);
        }

        public Task<IEnumerable<CaseProgress>> GetAllAsync()
        {
            return Task.FromResult(_caseProgresses.AsEnumerable());
        }

        public Task<CaseProgress?> GetByIdAsync(int id)
        {
            var caseProgress = _caseProgresses.FirstOrDefault(cp => cp.Id == id);
            return Task.FromResult(caseProgress);
        }

        public Task<IEnumerable<CaseProgress>> GetByCaseIdAsync(int caseId)
        {
            var progresses = _caseProgresses.Where(cp => cp.CaseId == caseId).OrderBy(cp => cp.CreatedDate).AsEnumerable();
            return Task.FromResult(progresses);
        }

        public Task<CaseProgress> CreateAsync(CaseProgress caseProgress)
        {
            caseProgress.Id = _nextId++;
            caseProgress.CreatedDate = DateTime.Now;
            _caseProgresses.Add(caseProgress);
            return Task.FromResult(caseProgress);
        }

        public Task<CaseProgress> UpdateAsync(CaseProgress caseProgress)
        {
            var existingProgress = _caseProgresses.FirstOrDefault(cp => cp.Id == caseProgress.Id);
            if (existingProgress != null)
            {
                existingProgress.Title = caseProgress.Title;
                existingProgress.Description = caseProgress.Description;
                existingProgress.Status = caseProgress.Status;
                existingProgress.CompletedDate = caseProgress.CompletedDate;
                existingProgress.AssignedTo = caseProgress.AssignedTo;
                existingProgress.Notes = caseProgress.Notes;
            }
            return Task.FromResult(existingProgress ?? caseProgress);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var caseProgress = _caseProgresses.FirstOrDefault(cp => cp.Id == id);
            if (caseProgress != null)
            {
                _caseProgresses.Remove(caseProgress);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetWorkflowTemplateAsync(string caseType)
        {
            var workflowTemplates = new Dictionary<string, List<string>>
            {
                ["土地測量"] = new List<string>
                {
                    "案件受理與資料審查",
                    "現場會勘安排",
                    "測量作業執行",
                    "測量成果圖製作",
                    "成果資料整理",
                    "客戶交付與驗收"
                },
                ["建物測量"] = new List<string>
                {
                    "建物現況調查",
                    "測量設備準備",
                    "外業測量作業",
                    "內業圖面製作",
                    "測量成果檢核",
                    "正式成果交付"
                },
                ["地籍調查"] = new List<string>
                {
                    "地籍資料查詢",
                    "界址點位確認",
                    "相關權利人通知",
                    "現場界址測定",
                    "調查結果彙整",
                    "調查報告完成"
                },
                ["土地複丈"] = new List<string>
                {
                    "複丈申請受理",
                    "圖籍資料調查",
                    "界址點位測設",
                    "現場測量作業",
                    "複丈成果圖製作",
                    "複丈結果通知"
                },
                ["界址鑑定"] = new List<string>
                {
                    "鑑定案件分析",
                    "相關資料蒐集",
                    "現場勘查作業",
                    "界址爭議調解",
                    "鑑定報告撰寫",
                    "鑑定結果說明"
                }
            };

            if (workflowTemplates.ContainsKey(caseType))
            {
                return Task.FromResult(workflowTemplates[caseType].AsEnumerable());
            }

            return Task.FromResult(new List<string>().AsEnumerable());
        }

        public Task<bool> InitializeWorkflowAsync(int caseId, string caseType)
        {
            var template = GetWorkflowTemplateAsync(caseType).Result;
            var assignedTo = "系統管理員";

            foreach (var taskName in template)
            {
                var progress = new CaseProgress
                {
                    Id = _nextId++,
                    CaseId = caseId,
                    Title = taskName,
                    Description = $"{caseType}案件的{taskName}階段",
                    Status = CaseStatus.Received,
                    CreatedDate = DateTime.Now,
                    AssignedTo = assignedTo
                };
                _caseProgresses.Add(progress);
            }

            return Task.FromResult(true);
        }

        public Task<Dictionary<CaseStatus, int>> GetProgressStatisticsAsync()
        {
            var statistics = _caseProgresses
                .GroupBy(cp => cp.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            // 確保所有狀態都有值
            foreach (CaseStatus status in Enum.GetValues<CaseStatus>())
            {
                if (!statistics.ContainsKey(status))
                {
                    statistics[status] = 0;
                }
            }

            return Task.FromResult(statistics);
        }
    }
}