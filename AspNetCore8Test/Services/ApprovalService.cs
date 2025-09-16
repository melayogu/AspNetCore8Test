using AspNetCore8Test.Models;
using AspNetCore8Test.Models.DTOs;

namespace AspNetCore8Test.Services
{
    public class ApprovalService
    {
        // 模擬資料庫資料
        private static readonly List<Approval> _approvals = new();
        private static readonly List<ApprovalHistory> _approvalHistories = new();
        private static int _nextId = 1;
        private static int _nextHistoryId = 1;

        static ApprovalService()
        {
            // 初始化一些測試資料
            InitializeTestData();
        }

        /// <summary>
        /// 取得簽核待辦列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<PendingApprovalDto>> GetPendingApprovalsAsync()
        {
            await Task.Delay(100); // 模擬非同步操作

            var pendingApprovals = _approvals
                .Where(a => a.Status == ApprovalStatus.Pending || a.Status == ApprovalStatus.InProgress)
                .Select(a => new PendingApprovalDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    RequestUser = a.RequestUser,
                    ApprovalType = a.ApprovalType,
                    Amount = a.Amount,
                    Priority = a.Priority,
                    CreatedDate = a.CreatedDate,
                    DueDate = a.DueDate
                })
                .OrderByDescending(a => a.CreatedDate)
                .ToList();

            return pendingApprovals;
        }

        /// <summary>
        /// 取得即時流程列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<InProgressApprovalDto>> GetInProgressApprovalsAsync()
        {
            await Task.Delay(100); // 模擬非同步操作

            var inProgressApprovals = _approvals
                .Where(a => a.Status == ApprovalStatus.InProgress)
                .Select(a =>
                {
                    var lastHistory = _approvalHistories
                        .Where(h => h.ApprovalId == a.Id)
                        .OrderByDescending(h => h.ActionDate)
                        .FirstOrDefault();

                    return new InProgressApprovalDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        RequestUser = a.RequestUser,
                        CurrentApprover = a.CurrentApprover,
                        ApprovalType = a.ApprovalType,
                        Status = a.Status,
                        Priority = a.Priority,
                        CreatedDate = a.CreatedDate,
                        LastAction = lastHistory?.Action ?? "提交申請",
                        LastActionDate = lastHistory?.ActionDate ?? a.CreatedDate
                    };
                })
                .OrderByDescending(a => a.LastActionDate)
                .ToList();

            return inProgressApprovals;
        }

        /// <summary>
        /// 取得簽核歷史列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<ApprovalHistoryDto> Items, int TotalCount)> GetApprovalHistoryAsync(int page, int pageSize)
        {
            await Task.Delay(100); // 模擬非同步操作

            var completedApprovals = _approvals
                .Where(a => a.Status == ApprovalStatus.Approved || 
                           a.Status == ApprovalStatus.Rejected || 
                           a.Status == ApprovalStatus.Withdrawn)
                .OrderByDescending(a => a.CompletedDate ?? a.CreatedDate);

            var totalCount = completedApprovals.Count();
            
            var items = completedApprovals
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a =>
                {
                    var finalAction = _approvalHistories
                        .Where(h => h.ApprovalId == a.Id)
                        .OrderByDescending(h => h.ActionDate)
                        .FirstOrDefault();

                    return new ApprovalHistoryDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        RequestUser = a.RequestUser,
                        ApprovalType = a.ApprovalType,
                        Status = a.Status,
                        CreatedDate = a.CreatedDate,
                        CompletedDate = a.CompletedDate,
                        FinalApprover = finalAction?.ApproverName ?? "",
                        Remarks = a.Remarks
                    };
                })
                .ToList();

            return (items, totalCount);
        }

        /// <summary>
        /// 處理簽核動作
        /// </summary>
        /// <param name="actionDto"></param>
        /// <returns></returns>
        public async Task<(bool Success, string Message)> ProcessApprovalAsync(ApprovalActionDto actionDto)
        {
            await Task.Delay(100); // 模擬非同步操作

            var approval = _approvals.FirstOrDefault(a => a.Id == actionDto.ApprovalId);
            if (approval == null)
            {
                return (false, "找不到簽核項目");
            }

            if (approval.Status != ApprovalStatus.Pending && approval.Status != ApprovalStatus.InProgress)
            {
                return (false, "此簽核項目已處理完成");
            }

            // 記錄簽核歷史
            var history = new ApprovalHistory
            {
                Id = GetNextHistoryId(),
                ApprovalId = approval.Id,
                ApproverName = "目前使用者", // 實際應用中應從身份驗證取得
                Action = actionDto.Action,
                Comments = actionDto.Comments,
                ActionDate = DateTime.Now,
                IpAddress = "127.0.0.1" // 實際應用中應從請求取得
            };

            _approvalHistories.Add(history);

            // 更新簽核狀態
            switch (actionDto.Action.ToLower())
            {
                case "approve":
                    approval.Status = ApprovalStatus.Approved;
                    approval.CompletedDate = DateTime.Now;
                    break;
                case "reject":
                    approval.Status = ApprovalStatus.Rejected;
                    approval.CompletedDate = DateTime.Now;
                    break;
                case "forward":
                    approval.Status = ApprovalStatus.InProgress;
                    approval.CurrentApprover = actionDto.NextApprover ?? approval.CurrentApprover;
                    break;
                default:
                    return (false, "不支援的簽核動作");
            }

            return (true, "簽核處理成功");
        }

        /// <summary>
        /// 取得簽核詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object?> GetApprovalDetailAsync(int id)
        {
            await Task.Delay(100); // 模擬非同步操作

            var approval = _approvals.FirstOrDefault(a => a.Id == id);
            if (approval == null)
            {
                return null;
            }

            var histories = _approvalHistories
                .Where(h => h.ApprovalId == id)
                .OrderBy(h => h.ActionDate)
                .Select(h => new
                {
                    h.ApproverName,
                    h.Action,
                    h.Comments,
                    h.ActionDate
                })
                .ToList();

            return new
            {
                approval.Id,
                approval.Title,
                approval.Description,
                approval.RequestUser,
                approval.CurrentApprover,
                approval.Status,
                approval.ApprovalType,
                approval.Amount,
                approval.Priority,
                approval.CreatedDate,
                approval.DueDate,
                approval.CompletedDate,
                approval.Remarks,
                Histories = histories
            };
        }

        /// <summary>
        /// 取得下一個歷史記錄ID
        /// </summary>
        /// <returns></returns>
        private static int GetNextHistoryId()
        {
            return _nextHistoryId++;
        }

        /// <summary>
        /// 取得下一個簽核ID
        /// </summary>
        /// <returns></returns>
        private static int GetNextApprovalId()
        {
            return _nextId++;
        }

        /// <summary>
        /// 初始化測試資料
        /// </summary>
        private static void InitializeTestData()
        {
            var testApprovals = new[]
            {
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "設備採購申請",
                    Description = "購買新的開發用電腦設備",
                    RequestUser = "張小明",
                    CurrentApprover = "李主管",
                    Status = ApprovalStatus.Pending,
                    ApprovalType = "設備採購",
                    Amount = 50000,
                    Priority = "High",
                    CreatedDate = DateTime.Now.AddDays(-2),
                    DueDate = DateTime.Now.AddDays(3)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "年假申請",
                    Description = "申請 2024/10/01 - 2024/10/05 年假",
                    RequestUser = "王小華",
                    CurrentApprover = "陳經理",
                    Status = ApprovalStatus.InProgress,
                    ApprovalType = "請假申請",
                    Priority = "Normal",
                    CreatedDate = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now.AddDays(2)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "加班費申請",
                    Description = "八月份加班費申請",
                    RequestUser = "林小美",
                    CurrentApprover = "人事部",
                    Status = ApprovalStatus.Approved,
                    ApprovalType = "費用申請",
                    Amount = 8000,
                    Priority = "Normal",
                    CreatedDate = DateTime.Now.AddDays(-7),
                    CompletedDate = DateTime.Now.AddDays(-1)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "差旅費申請",
                    Description = "台北出差交通住宿費用",
                    RequestUser = "劉小強",
                    CurrentApprover = "財務部",
                    Status = ApprovalStatus.Rejected,
                    ApprovalType = "費用申請",
                    Amount = 12000,
                    Priority = "Low",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    CompletedDate = DateTime.Now.AddDays(-2),
                    Remarks = "缺少相關憑證"
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "軟體授權採購",
                    Description = "購買Visual Studio Professional授權5套",
                    RequestUser = "陳大勇",
                    CurrentApprover = "技術部主管",
                    Status = ApprovalStatus.Approved,
                    ApprovalType = "軟體採購",
                    Amount = 35000,
                    Priority = "High",
                    CreatedDate = DateTime.Now.AddDays(-10),
                    CompletedDate = DateTime.Now.AddDays(-3)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "教育訓練申請",
                    Description = "參加Azure雲端架構師認證課程",
                    RequestUser = "李大華",
                    CurrentApprover = "人力資源部",
                    Status = ApprovalStatus.Approved,
                    ApprovalType = "訓練申請",
                    Amount = 15000,
                    Priority = "Normal",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    CompletedDate = DateTime.Now.AddDays(-4)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "辦公用品採購",
                    Description = "採購影印紙、文具用品等辦公耗材",
                    RequestUser = "總務小王",
                    CurrentApprover = "行政主管",
                    Status = ApprovalStatus.Rejected,
                    ApprovalType = "用品採購",
                    Amount = 3500,
                    Priority = "Low",
                    CreatedDate = DateTime.Now.AddDays(-6),
                    CompletedDate = DateTime.Now.AddDays(-3),
                    Remarks = "預算不足，請下個月再申請"
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "專案獎金申請",
                    Description = "ERP系統開發專案完成獎金申請",
                    RequestUser = "專案經理張三",
                    CurrentApprover = "總經理",
                    Status = ApprovalStatus.InProgress,
                    ApprovalType = "獎金申請",
                    Amount = 100000,
                    Priority = "High",
                    CreatedDate = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(1)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "病假申請",
                    Description = "因感冒需請病假3天",
                    RequestUser = "研發部小李",
                    CurrentApprover = "研發主管",
                    Status = ApprovalStatus.Approved,
                    ApprovalType = "請假申請",
                    Priority = "Normal",
                    CreatedDate = DateTime.Now.AddDays(-12),
                    CompletedDate = DateTime.Now.AddDays(-11)
                },
                new Approval
                {
                    Id = GetNextApprovalId(),
                    Title = "設備維修申請",
                    Description = "辦公室空調系統維修",
                    RequestUser = "設施管理員",
                    CurrentApprover = "總務部",
                    Status = ApprovalStatus.Approved,
                    ApprovalType = "維修申請",
                    Amount = 8500,
                    Priority = "High",
                    CreatedDate = DateTime.Now.AddDays(-15),
                    CompletedDate = DateTime.Now.AddDays(-13)
                }
            };

            _approvals.AddRange(testApprovals);

            // 新增一些歷史記錄
            var testHistories = new[]
            {
                // ID 2 (年假申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 2,
                    ApproverName = "李主管",
                    Action = "Forward",
                    Comments = "轉給經理審核",
                    ActionDate = DateTime.Now.AddHours(-6)
                },
                
                // ID 3 (加班費申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 3,
                    ApproverName = "直屬主管",
                    Action = "Approve",
                    Comments = "同意申請，加班確實辛苦",
                    ActionDate = DateTime.Now.AddDays(-2)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 3,
                    ApproverName = "人事部主管",
                    Action = "Approve",
                    Comments = "核准發放",
                    ActionDate = DateTime.Now.AddDays(-1)
                },
                
                // ID 4 (差旅費申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 4,
                    ApproverName = "直屬主管",
                    Action = "Approve",
                    Comments = "出差必要，同意申請",
                    ActionDate = DateTime.Now.AddDays(-4)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 4,
                    ApproverName = "財務主管",
                    Action = "Reject",
                    Comments = "請補齊相關憑證後重新申請，收據不清楚",
                    ActionDate = DateTime.Now.AddDays(-2)
                },
                
                // ID 5 (軟體授權採購) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 5,
                    ApproverName = "技術部主管",
                    Action = "Approve",
                    Comments = "開發需要，同意採購",
                    ActionDate = DateTime.Now.AddDays(-8)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 5,
                    ApproverName = "採購部主管",
                    Action = "Approve",
                    Comments = "已比較價格，此廠商最優惠",
                    ActionDate = DateTime.Now.AddDays(-6)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 5,
                    ApproverName = "財務總監",
                    Action = "Approve",
                    Comments = "預算充足，核准採購",
                    ActionDate = DateTime.Now.AddDays(-3)
                },
                
                // ID 6 (教育訓練申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 6,
                    ApproverName = "直屬主管",
                    Action = "Approve",
                    Comments = "有助於技能提升，支持參加",
                    ActionDate = DateTime.Now.AddDays(-7)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 6,
                    ApproverName = "人力資源部主管",
                    Action = "Approve",
                    Comments = "符合年度教育訓練計畫，核准",
                    ActionDate = DateTime.Now.AddDays(-4)
                },
                
                // ID 7 (辦公用品採購) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 7,
                    ApproverName = "行政主管",
                    Action = "Reject",
                    Comments = "本月預算已用完，請下個月再申請",
                    ActionDate = DateTime.Now.AddDays(-3)
                },
                
                // ID 8 (專案獎金申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 8,
                    ApproverName = "專案部門主管",
                    Action = "Approve",
                    Comments = "專案完成度優秀，值得獎勵",
                    ActionDate = DateTime.Now.AddDays(-2)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 8,
                    ApproverName = "人力資源總監",
                    Action = "Forward",
                    Comments = "轉送總經理最終核定",
                    ActionDate = DateTime.Now.AddDays(-1)
                },
                
                // ID 9 (病假申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 9,
                    ApproverName = "研發主管",
                    Action = "Approve",
                    Comments = "已提供醫生診斷書，核准請假",
                    ActionDate = DateTime.Now.AddDays(-11)
                },
                
                // ID 10 (設備維修申請) 的歷史記錄
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 10,
                    ApproverName = "設施主管",
                    Action = "Approve",
                    Comments = "空調確實需要維修，影響辦公環境",
                    ActionDate = DateTime.Now.AddDays(-14)
                },
                new ApprovalHistory
                {
                    Id = GetNextHistoryId(),
                    ApprovalId = 10,
                    ApproverName = "總務部經理",
                    Action = "Approve",
                    Comments = "已聯繫廠商，明日進行維修",
                    ActionDate = DateTime.Now.AddDays(-13)
                }
            };

            _approvalHistories.AddRange(testHistories);
        }
    }
}