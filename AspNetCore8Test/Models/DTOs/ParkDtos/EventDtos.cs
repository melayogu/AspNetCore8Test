using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 公園事件 DTO
/// </summary>
public class ParkEventDto
{
    public int Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public bool RequireRegistration { get; set; }
    public string Organizer { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 建立公園事件 DTO
/// </summary>
public class CreateParkEventDto
{
    [Required]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public int EventType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;
    
    public int? MaxParticipants { get; set; }
    
    public bool RequireRegistration { get; set; }
    
    [StringLength(200)]
    public string Organizer { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string ContactInfo { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 維護事件 DTO
/// </summary>
public class MaintenanceEventDto
{
    public int Id { get; set; }
    public string MaintenanceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MaintenanceType { get; set; } = string.Empty;
    public int? FacilityId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public decimal? EstimatedDuration { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? CompletedDate { get; set; }
    public string CompletionNotes { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public bool IsOverdue { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 建立維護事件 DTO
/// </summary>
public class CreateMaintenanceEventDto
{
    [Required]
    [StringLength(200)]
    public string MaintenanceName { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public int MaintenanceType { get; set; }
    
    public int? FacilityId { get; set; }
    
    [Required]
    public DateTime ScheduledDate { get; set; }
    
    public decimal? EstimatedDuration { get; set; }
    
    [Required]
    public int Priority { get; set; }
    
    [StringLength(100)]
    public string AssignedTo { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 緊急事件 DTO
/// </summary>
public class EmergencyEventDto
{
    public int Id { get; set; }
    public string EmergencyType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string ReportedBy { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string RespondedBy { get; set; } = string.Empty;
    public DateTime? ResponseTime { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string ResolutionNotes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ImpactArea { get; set; } = string.Empty;
    public int? EstimatedAffectedPeople { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsResolved { get; set; }
    public int? ResponseTimeMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 建立緊急事件 DTO
/// </summary>
public class CreateEmergencyEventDto
{
    [Required]
    public int EmergencyType { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public int Severity { get; set; }
    
    [Required]
    [StringLength(100)]
    public string ReportedBy { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ContactInfo { get; set; } = string.Empty;
    
    [Required]
    public DateTime OccurredAt { get; set; }
    
    [StringLength(200)]
    public string ImpactArea { get; set; } = string.Empty;
    
    public int? EstimatedAffectedPeople { get; set; }
    
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 事件統計 DTO
/// </summary>
public class EventStatsDto
{
    public int TotalParkEvents { get; set; }
    public int UpcomingEvents { get; set; }
    public int ActiveEvents { get; set; }
    public int TotalMaintenanceEvents { get; set; }
    public int TodayMaintenance { get; set; }
    public int PendingMaintenance { get; set; }
    public int TotalEmergencyEvents { get; set; }
    public int ActiveEmergencies { get; set; }
    public int TodayEmergencies { get; set; }
    
    public EventTypeStats EventTypeStats { get; set; } = new();
    public MaintenanceTypeStats MaintenanceTypeStats { get; set; } = new();
    public EmergencyTypeStats EmergencyTypeStats { get; set; } = new();
}

/// <summary>
/// 事件類型統計
/// </summary>
public class EventTypeStats
{
    public int Festivals { get; set; }
    public int Exhibitions { get; set; }
    public int Sports { get; set; }
    public int Education { get; set; }
    public int Community { get; set; }
    public int Others { get; set; }
}

/// <summary>
/// 維護類型統計
/// </summary>
public class MaintenanceTypeStats
{
    public int RoutineCleaning { get; set; }
    public int FacilityRepair { get; set; }
    public int LandscapeUpkeep { get; set; }
    public int Equipment { get; set; }
    public int Safety { get; set; }
    public int Others { get; set; }
}

/// <summary>
/// 緊急事件類型統計
/// </summary>
public class EmergencyTypeStats
{
    public int Accidents { get; set; }
    public int Security { get; set; }
    public int Weather { get; set; }
    public int Fire { get; set; }
    public int Medical { get; set; }
    public int Others { get; set; }
}