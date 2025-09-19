using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 遊客活動創建 DTO
/// </summary>
public class CreateVisitorActivityDto
{
    [Required(ErrorMessage = "活動名稱為必填")]
    [StringLength(100, ErrorMessage = "活動名稱不得超過100個字元")]
    public string ActivityName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "活動描述不得超過500個字元")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "活動類型為必填")]
    public int ActivityType { get; set; }

    [Required(ErrorMessage = "開始時間為必填")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "結束時間為必填")]
    public DateTime EndTime { get; set; }

    [StringLength(200, ErrorMessage = "地點不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "最大參與人數必須在1-1000之間")]
    public int MaxParticipants { get; set; } = 30;

    [Range(0, double.MaxValue, ErrorMessage = "費用不能為負數")]
    public decimal Fee { get; set; } = 0;

    [StringLength(100, ErrorMessage = "導覽員不得超過100個字元")]
    public string Guide { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// 遊客活動查詢 DTO
/// </summary>
public class VisitorActivityDto
{
    public int Id { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public decimal Fee { get; set; }
    public string Guide { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 活動狀態
    /// </summary>
    public string Status 
    { 
        get 
        {
            if (!IsActive) return "已取消";
            if (DateTime.Now < StartTime) return "即將開始";
            if (DateTime.Now >= StartTime && DateTime.Now <= EndTime) return "進行中";
            return "已結束";
        }
    }
    
    /// <summary>
    /// 是否額滿
    /// </summary>
    public bool IsFull => CurrentParticipants >= MaxParticipants;
    
    /// <summary>
    /// 剩餘名額
    /// </summary>
    public int RemainingSlots => Math.Max(0, MaxParticipants - CurrentParticipants);
}

/// <summary>
/// 活動預約創建 DTO
/// </summary>
public class CreateActivityReservationDto
{
    [Required(ErrorMessage = "活動ID為必填")]
    public int ActivityId { get; set; }

    [Required(ErrorMessage = "參與者姓名為必填")]
    [StringLength(50, ErrorMessage = "參與者姓名不得超過50個字元")]
    public string ParticipantName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "聯絡電話不得超過20個字元")]
    public string ContactPhone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [StringLength(100, ErrorMessage = "電子郵件不得超過100個字元")]
    public string Email { get; set; } = string.Empty;

    [Range(1, 50, ErrorMessage = "參與人數必須在1-50之間")]
    public int ParticipantCount { get; set; } = 1;

    [StringLength(500, ErrorMessage = "特殊需求不得超過500個字元")]
    public string SpecialRequirements { get; set; } = string.Empty;

    [Required(ErrorMessage = "預約時間為必填")]
    public DateTime ReservationTime { get; set; }
}

/// <summary>
/// 活動預約查詢 DTO
/// </summary>
public class ActivityReservationDto
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public string ParticipantName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ParticipantCount { get; set; }
    public string SpecialRequirements { get; set; } = string.Empty;
    public DateTime ReservationTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string CancelReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 遊客回饋創建 DTO
/// </summary>
public class CreateVisitorFeedbackDto
{
    [Required(ErrorMessage = "訪客姓名為必填")]
    [StringLength(50, ErrorMessage = "訪客姓名不得超過50個字元")]
    public string VisitorName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "聯絡電話不得超過20個字元")]
    public string ContactPhone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [StringLength(100, ErrorMessage = "電子郵件不得超過100個字元")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "回饋類型為必填")]
    public int FeedbackType { get; set; }

    [Required(ErrorMessage = "評分為必填")]
    [Range(1, 5, ErrorMessage = "評分必須在1-5之間")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "回饋內容為必填")]
    [StringLength(1000, ErrorMessage = "回饋內容不得超過1000個字元")]
    public string Content { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "回饋位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "訪問時間為必填")]
    public DateTime VisitDate { get; set; }

    public int? ActivityId { get; set; }
}

/// <summary>
/// 遊客回饋查詢 DTO
/// </summary>
public class VisitorFeedbackDto
{
    public int Id { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FeedbackType { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public int? ActivityId { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 評分星級顯示
    /// </summary>
    public string RatingStars
    {
        get
        {
            return new string('★', Rating) + new string('☆', 5 - Rating);
        }
    }
}

/// <summary>
/// 遊客服務統計 DTO
/// </summary>
public class VisitorServiceStatsDto
{
    public int TotalActivities { get; set; }
    public int ActiveActivities { get; set; }
    public int TotalReservations { get; set; }
    public int TodayReservations { get; set; }
    public int TotalFeedbacks { get; set; }
    public int UnprocessedFeedbacks { get; set; }
    public decimal AverageRating { get; set; }
    public DateTime? LastActivityDate { get; set; }
    
    public ActivityTypeStats ActivityTypeStats { get; set; } = new();
    public FeedbackTypeStats FeedbackTypeStats { get; set; } = new();
    public RatingDistribution RatingDistribution { get; set; } = new();
}

/// <summary>
/// 活動類型統計
/// </summary>
public class ActivityTypeStats
{
    public int GuidedTours { get; set; }
    public int NatureEducation { get; set; }
    public int Exercise { get; set; }
    public int Photography { get; set; }
    public int Volunteer { get; set; }
    public int Others { get; set; }
}

/// <summary>
/// 回饋類型統計
/// </summary>
public class FeedbackTypeStats
{
    public int Compliments { get; set; }
    public int Suggestions { get; set; }
    public int Complaints { get; set; }
    public int Facilities { get; set; }
    public int Services { get; set; }
    public int Others { get; set; }
}

/// <summary>
/// 評分分布
/// </summary>
public class RatingDistribution
{
    public int OneStar { get; set; }
    public int TwoStars { get; set; }
    public int ThreeStars { get; set; }
    public int FourStars { get; set; }
    public int FiveStars { get; set; }
}