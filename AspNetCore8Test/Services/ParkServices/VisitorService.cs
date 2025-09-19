using AspNetCore8Test.Models.ParkModels;
using AspNetCore8Test.Models.DTOs.ParkDtos;
using System.Threading;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 遊客服務介面
/// </summary>
public interface IVisitorService
{
    // 活動管理
    Task<IEnumerable<VisitorActivityDto>> GetActivitiesAsync();
    Task<VisitorActivityDto?> GetActivityByIdAsync(int id);
    Task<VisitorActivityDto> CreateActivityAsync(CreateVisitorActivityDto dto);
    Task<bool> UpdateActivityAsync(int id, CreateVisitorActivityDto dto);
    Task<bool> DeleteActivityAsync(int id);
    Task<bool> CancelActivityAsync(int id, string reason);
    
    // 預約管理
    Task<IEnumerable<ActivityReservationDto>> GetReservationsAsync();
    Task<IEnumerable<ActivityReservationDto>> GetReservationsByActivityAsync(int activityId);
    Task<ActivityReservationDto?> GetReservationByIdAsync(int id);
    Task<ActivityReservationDto> CreateReservationAsync(CreateActivityReservationDto dto);
    Task<bool> ConfirmReservationAsync(int id);
    Task<bool> CancelReservationAsync(int id, string reason);
    
    // 回饋管理
    Task<IEnumerable<VisitorFeedbackDto>> GetFeedbacksAsync();
    Task<VisitorFeedbackDto?> GetFeedbackByIdAsync(int id);
    Task<VisitorFeedbackDto> CreateFeedbackAsync(CreateVisitorFeedbackDto dto);
    Task<bool> ProcessFeedbackAsync(int id, string processedBy, string response);
    
    // 統計和查詢
    Task<VisitorServiceStatsDto> GetStatsAsync();
    Task<IEnumerable<VisitorActivityDto>> SearchActivitiesAsync(string searchTerm);
    Task<IEnumerable<VisitorActivityDto>> GetUpcomingActivitiesAsync();
    Task<IEnumerable<VisitorActivityDto>> GetActiveActivitiesAsync();
    Task<IEnumerable<VisitorFeedbackDto>> GetUnprocessedFeedbacksAsync();
    Task<IEnumerable<VisitorFeedbackDto>> GetFeedbacksByRatingAsync(int rating);
}

/// <summary>
/// 遊客服務實作
/// </summary>
public class VisitorService : IVisitorService
{
    private static readonly List<VisitorActivity> _activities = new();
    private static readonly List<ActivityReservation> _reservations = new();
    private static readonly List<VisitorFeedback> _feedbacks = new();
    private static int _activityIdCounter = 1;
    private static int _reservationIdCounter = 1;
    private static int _feedbackIdCounter = 1;
    private static readonly object _lock = new();
    
    static VisitorService()
    {
        InitializeSampleData();
    }
    
    public async Task<IEnumerable<VisitorActivityDto>> GetActivitiesAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _activities
                .OrderBy(a => a.StartDateTime)
                .Select(MapActivityToDto)
                .ToList();
        }
    }
    
    public async Task<VisitorActivityDto?> GetActivityByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var activity = _activities.FirstOrDefault(a => a.Id == id);
            return activity != null ? MapActivityToDto(activity) : null;
        }
    }
    
    public async Task<VisitorActivityDto> CreateActivityAsync(CreateVisitorActivityDto dto)
    {
        await Task.Delay(10);
        
        var activity = new VisitorActivity
        {
            Id = Interlocked.Increment(ref _activityIdCounter),
            Name = dto.ActivityName,
            Description = dto.Description,
            Type = (ActivityType)dto.ActivityType,
            StartDateTime = dto.StartTime,
            EndDateTime = dto.EndTime,
            Location = dto.Location,
            MaxParticipants = dto.MaxParticipants,
            Fee = dto.Fee,
            ContactPerson = dto.Guide,
            Notes = dto.Notes,
            Status = dto.IsActive ? ActivityStatus.Scheduled : ActivityStatus.Cancelled,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _activities.Add(activity);
        }
        
        return MapActivityToDto(activity);
    }
    
    public async Task<bool> UpdateActivityAsync(int id, CreateVisitorActivityDto dto)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var activity = _activities.FirstOrDefault(a => a.Id == id);
            if (activity == null) return false;
            
            activity.Name = dto.ActivityName;
            activity.Description = dto.Description;
            activity.Type = (ActivityType)dto.ActivityType;
            activity.StartDateTime = dto.StartTime;
            activity.EndDateTime = dto.EndTime;
            activity.Location = dto.Location;
            activity.MaxParticipants = dto.MaxParticipants;
            activity.Fee = dto.Fee;
            activity.ContactPerson = dto.Guide;
            activity.Notes = dto.Notes;
            activity.Status = dto.IsActive ? ActivityStatus.Scheduled : ActivityStatus.Cancelled;
            activity.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> DeleteActivityAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var activity = _activities.FirstOrDefault(a => a.Id == id);
            if (activity == null) return false;
            
            _activities.Remove(activity);
            return true;
        }
    }
    
    public async Task<bool> CancelActivityAsync(int id, string reason)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var activity = _activities.FirstOrDefault(a => a.Id == id);
            if (activity == null) return false;
            
            activity.Status = ActivityStatus.Cancelled;
            activity.Notes = $"取消原因: {reason}";
            activity.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<IEnumerable<ActivityReservationDto>> GetReservationsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _reservations
                .OrderByDescending(r => r.CreatedAt)
                .Select(MapReservationToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<ActivityReservationDto>> GetReservationsByActivityAsync(int activityId)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _reservations
                .Where(r => r.ActivityId == activityId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(MapReservationToDto)
                .ToList();
        }
    }
    
    public async Task<ActivityReservationDto?> GetReservationByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            return reservation != null ? MapReservationToDto(reservation) : null;
        }
    }
    
    public async Task<ActivityReservationDto> CreateReservationAsync(CreateActivityReservationDto dto)
    {
        await Task.Delay(10);
        
        // 檢查活動是否存在且有名額
        var activity = _activities.FirstOrDefault(a => a.Id == dto.ActivityId);
        if (activity == null)
            throw new InvalidOperationException("找不到指定的活動");
            
        var currentParticipants = _reservations
            .Where(r => r.ActivityId == dto.ActivityId && r.Status != ReservationStatus.Cancelled)
            .Sum(r => r.ParticipantCount);
            
        if (currentParticipants + dto.ParticipantCount > activity.MaxParticipants)
            throw new InvalidOperationException("活動名額不足");
        
        var reservation = new ActivityReservation
        {
            Id = Interlocked.Increment(ref _reservationIdCounter),
            ActivityId = dto.ActivityId,
            VisitorName = dto.ParticipantName,
            VisitorPhone = dto.ContactPhone,
            VisitorEmail = dto.Email,
            ParticipantCount = dto.ParticipantCount,
            SpecialRequirements = dto.SpecialRequirements,
            ReservationDate = dto.ReservationTime,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _reservations.Add(reservation);
        }
        
        return MapReservationToDto(reservation);
    }
    
    public async Task<bool> ConfirmReservationAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return false;
            
            reservation.Status = ReservationStatus.Confirmed;
            reservation.ConfirmationDate = DateTime.Now;
            reservation.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> CancelReservationAsync(int id, string reason)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return false;
            
            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancellationDate = DateTime.Now;
            reservation.CancellationReason = reason;
            reservation.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<IEnumerable<VisitorFeedbackDto>> GetFeedbacksAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _feedbacks
                .OrderByDescending(f => f.CreatedAt)
                .Select(MapFeedbackToDto)
                .ToList();
        }
    }
    
    public async Task<VisitorFeedbackDto?> GetFeedbackByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var feedback = _feedbacks.FirstOrDefault(f => f.Id == id);
            return feedback != null ? MapFeedbackToDto(feedback) : null;
        }
    }
    
    public async Task<VisitorFeedbackDto> CreateFeedbackAsync(CreateVisitorFeedbackDto dto)
    {
        await Task.Delay(10);
        
        var feedback = new VisitorFeedback
        {
            Id = Interlocked.Increment(ref _feedbackIdCounter),
            VisitorName = dto.VisitorName,
            VisitorPhone = dto.ContactPhone,
            VisitorEmail = dto.Email,
            Type = (FeedbackType)dto.FeedbackType,
            Rating = dto.Rating,
            Content = dto.Content,
            Subject = dto.Content.Length > 50 ? dto.Content.Substring(0, 50) + "..." : dto.Content,
            CreatedAt = dto.VisitDate
        };
        
        lock (_lock)
        {
            _feedbacks.Add(feedback);
        }
        
        return MapFeedbackToDto(feedback);
    }
    
    public async Task<bool> ProcessFeedbackAsync(int id, string processedBy, string response)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var feedback = _feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback == null) return false;
            
            feedback.Status = FeedbackStatus.Resolved;
            feedback.HandledAt = DateTime.Now;
            feedback.HandledBy = processedBy;
            feedback.Response = response;
            feedback.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<VisitorServiceStatsDto> GetStatsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var activeActivities = _activities.Count(a => a.Status == ActivityStatus.Scheduled && a.StartDateTime > DateTime.Now);
            var todayReservations = _reservations.Count(r => r.CreatedAt.Date == DateTime.Today);
            var unprocessedFeedbacks = _feedbacks.Count(f => f.Status == FeedbackStatus.Pending);
            var averageRating = _feedbacks.Any(f => f.Rating.HasValue) ? 
                _feedbacks.Where(f => f.Rating.HasValue).Average(f => f.Rating!.Value) : 0;
            
            return new VisitorServiceStatsDto
            {
                TotalActivities = _activities.Count,
                ActiveActivities = activeActivities,
                TotalReservations = _reservations.Count,
                TodayReservations = todayReservations,
                TotalFeedbacks = _feedbacks.Count,
                UnprocessedFeedbacks = unprocessedFeedbacks,
                AverageRating = (decimal)averageRating,
                LastActivityDate = _activities.Any() ? _activities.Max(a => a.StartDateTime) : null,
                
                ActivityTypeStats = new ActivityTypeStats
                {
                    GuidedTours = _activities.Count(a => a.Type == ActivityType.GuidedTour),
                    NatureEducation = _activities.Count(a => a.Type == ActivityType.Educational),
                    Exercise = _activities.Count(a => a.Type == ActivityType.Sports),
                    Photography = _activities.Count(a => a.Type == ActivityType.NatureWatching),
                    Volunteer = _activities.Count(a => a.Type == ActivityType.Volunteer),
                    Others = _activities.Count(a => a.Type == ActivityType.Other)
                },
                
                FeedbackTypeStats = new FeedbackTypeStats
                {
                    Compliments = _feedbacks.Count(f => f.Type == FeedbackType.Compliment),
                    Suggestions = _feedbacks.Count(f => f.Type == FeedbackType.Suggestion),
                    Complaints = _feedbacks.Count(f => f.Type == FeedbackType.Complaint),
                    Facilities = _feedbacks.Count(f => f.Category == FeedbackCategory.Facilities),
                    Services = _feedbacks.Count(f => f.Category == FeedbackCategory.Service),
                    Others = _feedbacks.Count(f => f.Type == FeedbackType.Other)
                },
                
                RatingDistribution = new RatingDistribution
                {
                    OneStar = _feedbacks.Count(f => f.Rating == 1),
                    TwoStars = _feedbacks.Count(f => f.Rating == 2),
                    ThreeStars = _feedbacks.Count(f => f.Rating == 3),
                    FourStars = _feedbacks.Count(f => f.Rating == 4),
                    FiveStars = _feedbacks.Count(f => f.Rating == 5)
                }
            };
        }
    }
    
    public async Task<IEnumerable<VisitorActivityDto>> SearchActivitiesAsync(string searchTerm)
    {
        await Task.Delay(10);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActivitiesAsync();
        
        lock (_lock)
        {
            return _activities
                .Where(a => 
                    a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.ContactPerson.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.StartDateTime)
                .Select(MapActivityToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<VisitorActivityDto>> GetUpcomingActivitiesAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _activities
                .Where(a => a.Status == ActivityStatus.Scheduled && a.StartDateTime > DateTime.Now)
                .OrderBy(a => a.StartDateTime)
                .Select(MapActivityToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<VisitorActivityDto>> GetActiveActivitiesAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var now = DateTime.Now;
            return _activities
                .Where(a => a.Status == ActivityStatus.InProgress || 
                          (a.Status == ActivityStatus.Scheduled && a.StartDateTime <= now && a.EndDateTime >= now))
                .OrderBy(a => a.StartDateTime)
                .Select(MapActivityToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<VisitorFeedbackDto>> GetUnprocessedFeedbacksAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _feedbacks
                .Where(f => f.Status == FeedbackStatus.Pending)
                .OrderByDescending(f => f.CreatedAt)
                .Select(MapFeedbackToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<VisitorFeedbackDto>> GetFeedbacksByRatingAsync(int rating)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _feedbacks
                .Where(f => f.Rating == rating)
                .OrderByDescending(f => f.CreatedAt)
                .Select(MapFeedbackToDto)
                .ToList();
        }
    }
    
    private static VisitorActivityDto MapActivityToDto(VisitorActivity activity)
    {
        var currentParticipants = _reservations
            .Where(r => r.ActivityId == activity.Id && r.Status != ReservationStatus.Cancelled)
            .Sum(r => r.ParticipantCount);
        
        return new VisitorActivityDto
        {
            Id = activity.Id,
            ActivityName = activity.Name,
            Description = activity.Description,
            ActivityType = activity.Type.ToString(),
            StartTime = activity.StartDateTime,
            EndTime = activity.EndDateTime,
            Location = activity.Location,
            MaxParticipants = activity.MaxParticipants,
            CurrentParticipants = currentParticipants,
            Fee = activity.Fee,
            Guide = activity.ContactPerson,
            Notes = activity.Notes,
            IsActive = activity.Status != ActivityStatus.Cancelled,
            CreatedAt = activity.CreatedAt
        };
    }
    
    private static ActivityReservationDto MapReservationToDto(ActivityReservation reservation)
    {
        var activity = _activities.FirstOrDefault(a => a.Id == reservation.ActivityId);
        
        return new ActivityReservationDto
        {
            Id = reservation.Id,
            ActivityId = reservation.ActivityId,
            ActivityName = activity?.Name ?? "",
            ParticipantName = reservation.VisitorName,
            ContactPhone = reservation.VisitorPhone,
            Email = reservation.VisitorEmail,
            ParticipantCount = reservation.ParticipantCount,
            SpecialRequirements = reservation.SpecialRequirements,
            ReservationTime = reservation.ReservationDate,
            Status = reservation.Status.ToString(),
            IsConfirmed = reservation.Status == ReservationStatus.Confirmed,
            IsCancelled = reservation.Status == ReservationStatus.Cancelled,
            ConfirmedAt = reservation.ConfirmationDate,
            CancelledAt = reservation.CancellationDate,
            CancelReason = reservation.CancellationReason,
            CreatedAt = reservation.CreatedAt
        };
    }
    
    private static VisitorFeedbackDto MapFeedbackToDto(VisitorFeedback feedback)
    {
        return new VisitorFeedbackDto
        {
            Id = feedback.Id,
            VisitorName = feedback.VisitorName,
            ContactPhone = feedback.VisitorPhone,
            Email = feedback.VisitorEmail,
            FeedbackType = feedback.Type.ToString(),
            Rating = feedback.Rating ?? 0,
            Content = feedback.Content,
            Location = feedback.Subject, // 使用主題作為位置
            VisitDate = feedback.CreatedAt,
            ActivityId = null,
            ActivityName = "",
            IsProcessed = feedback.Status != FeedbackStatus.Pending,
            ProcessedAt = feedback.HandledAt,
            ProcessedBy = feedback.HandledBy,
            Response = feedback.Response,
            CreatedAt = feedback.CreatedAt
        };
    }
    
    private static void InitializeSampleData()
    {
        // 初始化活動數據
        var now = DateTime.Now;
        var activities = new[]
        {
            new VisitorActivity
            {
                Id = Interlocked.Increment(ref _activityIdCounter),
                Name = "湖心亭生態導覽",
                Description = "專業生態導覽員帶領遊客認識碧湖公園的生態環境與湖心亭的歷史文化",
                Type = ActivityType.GuidedTour,
                StartDateTime = now.AddDays(1).Date.AddHours(9),
                EndDateTime = now.AddDays(1).Date.AddHours(11),
                Location = "湖心亭",
                MaxParticipants = 20,
                Fee = 0,
                ContactPerson = "王志明導覽員",
                Notes = "請穿著舒適鞋子",
                Status = ActivityStatus.Scheduled,
                CreatedAt = now.AddDays(-5)
            },
            new VisitorActivity
            {
                Id = Interlocked.Increment(ref _activityIdCounter),
                Name = "晨間太極拳班",
                Description = "在優美的湖景陪伴下練習太極拳，適合各年齡層參與",
                Type = ActivityType.Sports,
                StartDateTime = now.AddDays(2).Date.AddHours(7),
                EndDateTime = now.AddDays(2).Date.AddHours(8),
                Location = "東岸草地",
                MaxParticipants = 30,
                Fee = 0,
                ContactPerson = "李師傅",
                Notes = "請自備瑜伽墊",
                Status = ActivityStatus.Scheduled,
                CreatedAt = now.AddDays(-3)
            },
            new VisitorActivity
            {
                Id = Interlocked.Increment(ref _activityIdCounter),
                Name = "自然攝影工作坊",
                Description = "學習自然攝影技巧，捕捉公園四季之美",
                Type = ActivityType.NatureWatching,
                StartDateTime = now.AddDays(3).Date.AddHours(14),
                EndDateTime = now.AddDays(3).Date.AddHours(17),
                Location = "公園各景點",
                MaxParticipants = 15,
                Fee = 200,
                ContactPerson = "陳攝影師",
                Notes = "請攜帶相機或手機",
                Status = ActivityStatus.Scheduled,
                CreatedAt = now.AddDays(-2)
            }
        };
        
        _activities.AddRange(activities);
        
        // 初始化預約數據
        var reservations = new[]
        {
            new ActivityReservation
            {
                Id = Interlocked.Increment(ref _reservationIdCounter),
                ActivityId = 1,
                VisitorName = "張小美",
                VisitorPhone = "0912345678",
                VisitorEmail = "zhang@example.com",
                ParticipantCount = 2,
                SpecialRequirements = "需要輪椅無障礙設施",
                ReservationDate = now.AddDays(1).Date.AddHours(9),
                Status = ReservationStatus.Confirmed,
                ConfirmationDate = now.AddHours(-2),
                CreatedAt = now.AddDays(-1)
            },
            new ActivityReservation
            {
                Id = Interlocked.Increment(ref _reservationIdCounter),
                ActivityId = 2,
                VisitorName = "劉大明",
                VisitorPhone = "0987654321",
                VisitorEmail = "liu@example.com",
                ParticipantCount = 1,
                SpecialRequirements = "",
                ReservationDate = now.AddDays(2).Date.AddHours(7),
                Status = ReservationStatus.Pending,
                CreatedAt = now.AddHours(-5)
            }
        };
        
        _reservations.AddRange(reservations);
        
        // 初始化回饋數據
        var feedbacks = new[]
        {
            new VisitorFeedback
            {
                Id = Interlocked.Increment(ref _feedbackIdCounter),
                VisitorName = "林小姐",
                VisitorPhone = "0911111111",
                VisitorEmail = "lin@example.com",
                Type = FeedbackType.Compliment,
                Category = FeedbackCategory.Facilities,
                Rating = 5,
                Content = "公園環境維護得很好，湖心亭的風景非常美麗，是休閒散步的好地方！",
                Subject = "讚美公園環境",
                CreatedAt = now.AddDays(-1)
            },
            new VisitorFeedback
            {
                Id = Interlocked.Increment(ref _feedbackIdCounter),
                VisitorName = "陳先生",
                VisitorPhone = "0922222222",
                VisitorEmail = "chen@example.com",
                Type = FeedbackType.Suggestion,
                Category = FeedbackCategory.Facilities,
                Rating = 4,
                Content = "建議在步道增設更多休息椅，方便年長者休息",
                Subject = "步道設施建議",
                CreatedAt = now.AddDays(-2)
            },
            new VisitorFeedback
            {
                Id = Interlocked.Increment(ref _feedbackIdCounter),
                VisitorName = "王太太",
                VisitorPhone = "0933333333",
                VisitorEmail = "wang@example.com",
                Type = FeedbackType.Complaint,
                Category = FeedbackCategory.Cleanliness,
                Rating = 3,
                Content = "廁所設施需要改善，清潔度有待加強",
                Subject = "廁所設施問題",
                CreatedAt = now.AddDays(-3)
            }
        };
        
        _feedbacks.AddRange(feedbacks);
    }
}