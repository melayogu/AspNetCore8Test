using AspNetCore8Test.Models.ParkModels;
using AspNetCore8Test.Models.DTOs.ParkDtos;
using System.Threading;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 事件管理服務介面
/// </summary>
public interface IEventService
{
    // 公園事件
    Task<IEnumerable<ParkEventDto>> GetEventsAsync();
    Task<ParkEventDto?> GetEventByIdAsync(int id);
    Task<ParkEventDto> CreateEventAsync(CreateParkEventDto dto);
    Task<bool> UpdateEventAsync(int id, CreateParkEventDto dto);
    Task<bool> DeleteEventAsync(int id);
    Task<IEnumerable<ParkEventDto>> SearchEventsAsync(string searchTerm);
    Task<IEnumerable<ParkEventDto>> GetEventsByTypeAsync(int type);
    Task<IEnumerable<ParkEventDto>> GetUpcomingEventsAsync(int days);
    Task<IEnumerable<ParkEventDto>> GetActiveEventsAsync();
    Task<bool> CompleteEventAsync(int id, string completionNotes);
    Task<bool> CancelEventAsync(int id, string cancellationReason);
    
    // 維護事件
    Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsAsync();
    Task<MaintenanceEventDto?> GetMaintenanceEventByIdAsync(int id);
    Task<MaintenanceEventDto> CreateMaintenanceEventAsync(CreateMaintenanceEventDto dto);
    Task<bool> UpdateMaintenanceEventAsync(int id, CreateMaintenanceEventDto dto);
    Task<bool> DeleteMaintenanceEventAsync(int id);
    Task<IEnumerable<MaintenanceEventDto>> GetTodayMaintenanceEventsAsync();
    Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsByFacilityAsync(int facilityId);
    Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsByStatusAsync(int status);
    Task<bool> AssignMaintenanceEventAsync(int id, string assignTo);
    Task<bool> CompleteMaintenanceEventAsync(int id, string completionNotes);
    
    // 緊急事件
    Task<IEnumerable<EmergencyEventDto>> GetEmergencyEventsAsync();
    Task<EmergencyEventDto?> GetEmergencyEventByIdAsync(int id);
    Task<EmergencyEventDto> CreateEmergencyEventAsync(CreateEmergencyEventDto dto);
    Task<bool> UpdateEmergencyEventAsync(int id, CreateEmergencyEventDto dto);
    Task<bool> DeleteEmergencyEventAsync(int id);
    Task<IEnumerable<EmergencyEventDto>> GetPendingEmergencyEventsAsync();
    Task<IEnumerable<EmergencyEventDto>> GetEmergencyEventsBySeverityAsync(int severity);
    Task<bool> RespondToEmergencyEventAsync(int id, string responder);
    Task<bool> ResolveEmergencyEventAsync(int id, string resolutionNotes);
    
    // 統計
    Task<EventStatsDto> GetEventStatsAsync();
}

/// <summary>
/// 事件管理服務實作
/// </summary>
public class EventService : IEventService
{
    private static readonly List<ParkEvent> _parkEvents = new();
    private static readonly List<MaintenanceEvent> _maintenanceEvents = new();
    private static readonly List<EmergencyEvent> _emergencyEvents = new();
    private static int _parkEventIdCounter = 1;
    private static int _maintenanceEventIdCounter = 1;
    private static int _emergencyEventIdCounter = 1;
    private static readonly object _lock = new();
    
    static EventService()
    {
        InitializeSampleData();
    }
    
    #region 公園事件
    
    public async Task<IEnumerable<ParkEventDto>> GetEventsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _parkEvents
                .OrderByDescending(e => e.StartDate)
                .Select(MapParkEventToDto)
                .ToList();
        }
    }
    
    public async Task<ParkEventDto?> GetEventByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _parkEvents.FirstOrDefault(e => e.Id == id);
            return eventItem != null ? MapParkEventToDto(eventItem) : null;
        }
    }
    
    public async Task<ParkEventDto> CreateEventAsync(CreateParkEventDto dto)
    {
        await Task.Delay(10);
        
        var eventItem = new ParkEvent
        {
            Id = Interlocked.Increment(ref _parkEventIdCounter),
            Name = dto.EventName,
            Description = dto.Description,
            Type = (EventType)dto.EventType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
            MaxParticipants = dto.MaxParticipants,
            RegistrationRequired = dto.RequireRegistration,
            Organizer = dto.Organizer,
            ContactInfo = dto.ContactInfo,
            Notes = dto.Notes,
            Status = EventStatus.Scheduled,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _parkEvents.Add(eventItem);
        }
        
        return MapParkEventToDto(eventItem);
    }
    
    public async Task<bool> UpdateEventAsync(int id, CreateParkEventDto dto)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _parkEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Name = dto.EventName;
            eventItem.Description = dto.Description;
            eventItem.Type = (EventType)dto.EventType;
            eventItem.StartDate = dto.StartDate;
            eventItem.EndDate = dto.EndDate;
            eventItem.Location = dto.Location;
            eventItem.MaxParticipants = dto.MaxParticipants;
            eventItem.RegistrationRequired = dto.RequireRegistration;
            eventItem.Organizer = dto.Organizer;
            eventItem.ContactInfo = dto.ContactInfo;
            eventItem.Notes = dto.Notes;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> DeleteEventAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _parkEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            _parkEvents.Remove(eventItem);
            return true;
        }
    }
    
    public async Task<IEnumerable<ParkEventDto>> SearchEventsAsync(string searchTerm)
    {
        await Task.Delay(10);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetEventsAsync();
        
        lock (_lock)
        {
            return _parkEvents
                .Where(e => 
                    e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Organizer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(e => e.StartDate)
                .Select(MapParkEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<ParkEventDto>> GetEventsByTypeAsync(int type)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _parkEvents
                .Where(e => (int)e.Type == type)
                .OrderByDescending(e => e.StartDate)
                .Select(MapParkEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<ParkEventDto>> GetUpcomingEventsAsync(int days)
    {
        await Task.Delay(10);
        
        var cutoffDate = DateTime.Now.AddDays(days);
        
        lock (_lock)
        {
            return _parkEvents
                .Where(e => e.Status == EventStatus.Scheduled && 
                          e.StartDate >= DateTime.Now && 
                          e.StartDate <= cutoffDate)
                .OrderBy(e => e.StartDate)
                .Select(MapParkEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<ParkEventDto>> GetActiveEventsAsync()
    {
        await Task.Delay(10);
        
        var now = DateTime.Now;
        
        lock (_lock)
        {
            return _parkEvents
                .Where(e => e.Status == EventStatus.InProgress || 
                          (e.Status == EventStatus.Scheduled && e.StartDate <= now && e.EndDate >= now))
                .OrderBy(e => e.StartDate)
                .Select(MapParkEventToDto)
                .ToList();
        }
    }
    
    public async Task<bool> CompleteEventAsync(int id, string completionNotes)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _parkEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Status = EventStatus.Completed;
            eventItem.Notes = string.IsNullOrEmpty(eventItem.Notes) ? 
                completionNotes : $"{eventItem.Notes}\n完成備註: {completionNotes}";
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> CancelEventAsync(int id, string cancellationReason)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _parkEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Status = EventStatus.Cancelled;
            eventItem.Notes = string.IsNullOrEmpty(eventItem.Notes) ? 
                cancellationReason : $"{eventItem.Notes}\n取消原因: {cancellationReason}";
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    #endregion
    
    #region 維護事件
    
    public async Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _maintenanceEvents
                .OrderByDescending(e => e.ScheduledDate)
                .Select(MapMaintenanceEventToDto)
                .ToList();
        }
    }
    
    public async Task<MaintenanceEventDto?> GetMaintenanceEventByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _maintenanceEvents.FirstOrDefault(e => e.Id == id);
            return eventItem != null ? MapMaintenanceEventToDto(eventItem) : null;
        }
    }
    
    public async Task<MaintenanceEventDto> CreateMaintenanceEventAsync(CreateMaintenanceEventDto dto)
    {
        await Task.Delay(10);
        
        var eventItem = new MaintenanceEvent
        {
            Id = Interlocked.Increment(ref _maintenanceEventIdCounter),
            Name = dto.MaintenanceName,
            Description = dto.Description,
            Type = (MaintenanceType)dto.MaintenanceType,
            FacilityId = dto.FacilityId,
            ScheduledDate = dto.ScheduledDate,
            EstimatedDuration = dto.EstimatedDuration,
            Priority = (MaintenancePriority)dto.Priority,
            AssignedTo = dto.AssignedTo,
            Notes = dto.Notes,
            Status = MaintenanceStatus.Scheduled,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _maintenanceEvents.Add(eventItem);
        }
        
        return MapMaintenanceEventToDto(eventItem);
    }
    
    public async Task<bool> UpdateMaintenanceEventAsync(int id, CreateMaintenanceEventDto dto)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _maintenanceEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Name = dto.MaintenanceName;
            eventItem.Description = dto.Description;
            eventItem.Type = (MaintenanceType)dto.MaintenanceType;
            eventItem.FacilityId = dto.FacilityId;
            eventItem.ScheduledDate = dto.ScheduledDate;
            eventItem.EstimatedDuration = dto.EstimatedDuration;
            eventItem.Priority = (MaintenancePriority)dto.Priority;
            eventItem.AssignedTo = dto.AssignedTo;
            eventItem.Notes = dto.Notes;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> DeleteMaintenanceEventAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _maintenanceEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            _maintenanceEvents.Remove(eventItem);
            return true;
        }
    }
    
    public async Task<IEnumerable<MaintenanceEventDto>> GetTodayMaintenanceEventsAsync()
    {
        await Task.Delay(10);
        
        var today = DateTime.Today;
        
        lock (_lock)
        {
            return _maintenanceEvents
                .Where(e => e.ScheduledDate.Date == today)
                .OrderBy(e => e.ScheduledDate)
                .Select(MapMaintenanceEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsByFacilityAsync(int facilityId)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _maintenanceEvents
                .Where(e => e.FacilityId == facilityId)
                .OrderByDescending(e => e.ScheduledDate)
                .Select(MapMaintenanceEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<MaintenanceEventDto>> GetMaintenanceEventsByStatusAsync(int status)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _maintenanceEvents
                .Where(e => (int)e.Status == status)
                .OrderByDescending(e => e.ScheduledDate)
                .Select(MapMaintenanceEventToDto)
                .ToList();
        }
    }
    
    public async Task<bool> AssignMaintenanceEventAsync(int id, string assignTo)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _maintenanceEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.AssignedTo = assignTo;
            eventItem.Status = MaintenanceStatus.Assigned;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> CompleteMaintenanceEventAsync(int id, string completionNotes)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _maintenanceEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Status = MaintenanceStatus.Completed;
            eventItem.CompletedDate = DateTime.Now;
            eventItem.CompletionNotes = completionNotes;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    #endregion
    
    #region 緊急事件
    
    public async Task<IEnumerable<EmergencyEventDto>> GetEmergencyEventsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _emergencyEvents
                .OrderByDescending(e => e.OccurredAt)
                .Select(MapEmergencyEventToDto)
                .ToList();
        }
    }
    
    public async Task<EmergencyEventDto?> GetEmergencyEventByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _emergencyEvents.FirstOrDefault(e => e.Id == id);
            return eventItem != null ? MapEmergencyEventToDto(eventItem) : null;
        }
    }
    
    public async Task<EmergencyEventDto> CreateEmergencyEventAsync(CreateEmergencyEventDto dto)
    {
        await Task.Delay(10);
        
        var eventItem = new EmergencyEvent
        {
            Id = Interlocked.Increment(ref _emergencyEventIdCounter),
            Type = (EmergencyType)dto.EmergencyType,
            Description = dto.Description,
            Location = dto.Location,
            Severity = (EmergencySeverity)dto.Severity,
            ReportedBy = dto.ReportedBy,
            ContactInfo = dto.ContactInfo,
            OccurredAt = dto.OccurredAt,
            Status = EmergencyStatus.Reported,
            ImpactArea = dto.ImpactArea,
            EstimatedAffectedPeople = dto.EstimatedAffectedPeople,
            Notes = dto.Notes,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _emergencyEvents.Add(eventItem);
        }
        
        return MapEmergencyEventToDto(eventItem);
    }
    
    public async Task<bool> UpdateEmergencyEventAsync(int id, CreateEmergencyEventDto dto)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _emergencyEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Type = (EmergencyType)dto.EmergencyType;
            eventItem.Description = dto.Description;
            eventItem.Location = dto.Location;
            eventItem.Severity = (EmergencySeverity)dto.Severity;
            eventItem.ReportedBy = dto.ReportedBy;
            eventItem.ContactInfo = dto.ContactInfo;
            eventItem.OccurredAt = dto.OccurredAt;
            eventItem.ImpactArea = dto.ImpactArea;
            eventItem.EstimatedAffectedPeople = dto.EstimatedAffectedPeople;
            eventItem.Notes = dto.Notes;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> DeleteEmergencyEventAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _emergencyEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            _emergencyEvents.Remove(eventItem);
            return true;
        }
    }
    
    public async Task<IEnumerable<EmergencyEventDto>> GetPendingEmergencyEventsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _emergencyEvents
                .Where(e => e.Status == EmergencyStatus.Reported || e.Status == EmergencyStatus.Responding)
                .OrderByDescending(e => e.Severity)
                .ThenByDescending(e => e.OccurredAt)
                .Select(MapEmergencyEventToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EmergencyEventDto>> GetEmergencyEventsBySeverityAsync(int severity)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _emergencyEvents
                .Where(e => (int)e.Severity == severity)
                .OrderByDescending(e => e.OccurredAt)
                .Select(MapEmergencyEventToDto)
                .ToList();
        }
    }
    
    public async Task<bool> RespondToEmergencyEventAsync(int id, string responder)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _emergencyEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Status = EmergencyStatus.Responding;
            eventItem.RespondedBy = responder;
            eventItem.ResponseTime = DateTime.Now;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    public async Task<bool> ResolveEmergencyEventAsync(int id, string resolutionNotes)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var eventItem = _emergencyEvents.FirstOrDefault(e => e.Id == id);
            if (eventItem == null) return false;
            
            eventItem.Status = EmergencyStatus.Resolved;
            eventItem.ResolvedAt = DateTime.Now;
            eventItem.ResolutionNotes = resolutionNotes;
            eventItem.UpdatedAt = DateTime.Now;
            
            return true;
        }
    }
    
    #endregion
    
    #region 統計
    
    public async Task<EventStatsDto> GetEventStatsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var now = DateTime.Now;
            var today = now.Date;
            
            // 公園事件統計
            var upcomingEvents = _parkEvents.Count(e => e.Status == EventStatus.Scheduled && e.StartDate > now);
            var activeEvents = _parkEvents.Count(e => e.Status == EventStatus.InProgress || 
                (e.Status == EventStatus.Scheduled && e.StartDate <= now && e.EndDate >= now));
            
            // 維護事件統計
            var todayMaintenance = _maintenanceEvents.Count(e => e.ScheduledDate.Date == today);
            var pendingMaintenance = _maintenanceEvents.Count(e => e.Status == MaintenanceStatus.Scheduled || 
                e.Status == MaintenanceStatus.Assigned);
            
            // 緊急事件統計
            var activeEmergencies = _emergencyEvents.Count(e => e.Status == EmergencyStatus.Reported || 
                e.Status == EmergencyStatus.Responding);
            var todayEmergencies = _emergencyEvents.Count(e => e.OccurredAt.Date == today);
            
            return new EventStatsDto
            {
                TotalParkEvents = _parkEvents.Count,
                UpcomingEvents = upcomingEvents,
                ActiveEvents = activeEvents,
                TotalMaintenanceEvents = _maintenanceEvents.Count,
                TodayMaintenance = todayMaintenance,
                PendingMaintenance = pendingMaintenance,
                TotalEmergencyEvents = _emergencyEvents.Count,
                ActiveEmergencies = activeEmergencies,
                TodayEmergencies = todayEmergencies,
                
                EventTypeStats = new EventTypeStats
                {
                    Festivals = _parkEvents.Count(e => e.Type == EventType.Festival),
                    Exhibitions = _parkEvents.Count(e => e.Type == EventType.Exhibition),
                    Sports = _parkEvents.Count(e => e.Type == EventType.Sports),
                    Education = _parkEvents.Count(e => e.Type == EventType.Educational),
                    Community = _parkEvents.Count(e => e.Type == EventType.Community),
                    Others = _parkEvents.Count(e => e.Type == EventType.Other)
                },
                
                MaintenanceTypeStats = new MaintenanceTypeStats
                {
                    RoutineCleaning = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Cleaning),
                    FacilityRepair = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Repair),
                    LandscapeUpkeep = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Landscaping),
                    Equipment = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Equipment),
                    Safety = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Safety),
                    Others = _maintenanceEvents.Count(e => e.Type == MaintenanceType.Other)
                },
                
                EmergencyTypeStats = new EmergencyTypeStats
                {
                    Accidents = _emergencyEvents.Count(e => e.Type == EmergencyType.Accident),
                    Security = _emergencyEvents.Count(e => e.Type == EmergencyType.Security),
                    Weather = _emergencyEvents.Count(e => e.Type == EmergencyType.Weather),
                    Fire = _emergencyEvents.Count(e => e.Type == EmergencyType.Fire),
                    Medical = _emergencyEvents.Count(e => e.Type == EmergencyType.Medical),
                    Others = _emergencyEvents.Count(e => e.Type == EmergencyType.Other)
                }
            };
        }
    }
    
    #endregion
    
    #region 映射方法
    
    private static ParkEventDto MapParkEventToDto(ParkEvent eventItem)
    {
        return new ParkEventDto
        {
            Id = eventItem.Id,
            EventName = eventItem.Name,
            Description = eventItem.Description,
            EventType = eventItem.Type.ToString(),
            StartDate = eventItem.StartDate,
            EndDate = eventItem.EndDate,
            Location = eventItem.Location,
            MaxParticipants = eventItem.MaxParticipants,
            CurrentParticipants = 0,
            RequireRegistration = eventItem.RegistrationRequired,
            Organizer = eventItem.Organizer,
            ContactInfo = eventItem.ContactInfo,
            Notes = eventItem.Notes,
            Status = eventItem.Status.ToString(),
            IsActive = eventItem.Status == EventStatus.Scheduled || eventItem.Status == EventStatus.InProgress,
            CreatedAt = eventItem.CreatedAt
        };
    }
    
    private static MaintenanceEventDto MapMaintenanceEventToDto(MaintenanceEvent eventItem)
    {
        return new MaintenanceEventDto
        {
            Id = eventItem.Id,
            MaintenanceName = eventItem.Name,
            Description = eventItem.Description,
            MaintenanceType = eventItem.Type.ToString(),
            FacilityId = eventItem.FacilityId,
            FacilityName = $"設施 {eventItem.FacilityId}",
            ScheduledDate = eventItem.ScheduledDate,
            EstimatedDuration = eventItem.EstimatedDuration,
            Priority = eventItem.Priority.ToString(),
            AssignedTo = eventItem.AssignedTo,
            Notes = eventItem.Notes,
            Status = eventItem.Status.ToString(),
            CompletedDate = eventItem.CompletedDate,
            CompletionNotes = eventItem.CompletionNotes,
            IsCompleted = eventItem.Status == MaintenanceStatus.Completed,
            IsOverdue = eventItem.Status != MaintenanceStatus.Completed && eventItem.ScheduledDate < DateTime.Now,
            CreatedAt = eventItem.CreatedAt
        };
    }
    
    private static EmergencyEventDto MapEmergencyEventToDto(EmergencyEvent eventItem)
    {
        return new EmergencyEventDto
        {
            Id = eventItem.Id,
            EmergencyType = eventItem.Type.ToString(),
            Description = eventItem.Description,
            Location = eventItem.Location,
            Severity = eventItem.Severity.ToString(),
            ReportedBy = eventItem.ReportedBy,
            ContactInfo = eventItem.ContactInfo,
            OccurredAt = eventItem.OccurredAt,
            RespondedBy = eventItem.RespondedBy,
            ResponseTime = eventItem.ResponseTime,
            ResolvedAt = eventItem.ResolvedAt,
            ResolutionNotes = eventItem.ResolutionNotes,
            Status = eventItem.Status.ToString(),
            ImpactArea = eventItem.ImpactArea,
            EstimatedAffectedPeople = eventItem.EstimatedAffectedPeople,
            Notes = eventItem.Notes,
            IsActive = eventItem.Status == EmergencyStatus.Reported || eventItem.Status == EmergencyStatus.Responding,
            IsResolved = eventItem.Status == EmergencyStatus.Resolved,
            ResponseTimeMinutes = eventItem.ResponseTime.HasValue && eventItem.OccurredAt != default ? 
                (int)(eventItem.ResponseTime.Value - eventItem.OccurredAt).TotalMinutes : null,
            CreatedAt = eventItem.CreatedAt
        };
    }
    
    #endregion
    
    #region 初始化範例數據
    
    private static void InitializeSampleData()
    {
        var now = DateTime.Now;
        
        // 初始化公園事件
        var parkEvents = new[]
        {
            new ParkEvent
            {
                Id = Interlocked.Increment(ref _parkEventIdCounter),
                Name = "碧湖公園賞櫻節",
                Description = "春季賞櫻活動，包含櫻花導覽、攝影比賽和文藝表演",
                Type = EventType.Festival,
                StartDate = now.AddDays(14).Date.AddHours(9),
                EndDate = now.AddDays(16).Date.AddHours(17),
                Location = "湖心亭周邊",
                MaxParticipants = 500,
                RegistrationRequired = true,
                Organizer = "內湖區公所",
                ContactInfo = "02-2792-7171",
                Notes = "請配合防疫措施",
                Status = EventStatus.Scheduled,
                CreatedAt = now.AddDays(-7)
            },
            new ParkEvent
            {
                Id = Interlocked.Increment(ref _parkEventIdCounter),
                Name = "環湖健走活動",
                Description = "每週三晨間健走活動，促進社區居民健康",
                Type = EventType.Sports,
                StartDate = now.AddDays(2).Date.AddHours(7),
                EndDate = now.AddDays(2).Date.AddHours(8),
                Location = "環湖步道",
                MaxParticipants = 100,
                RegistrationRequired = false,
                Organizer = "內湖運動中心",
                ContactInfo = "02-8792-1234",
                Notes = "歡迎自由參加",
                Status = EventStatus.Scheduled,
                CreatedAt = now.AddDays(-3)
            }
        };
        
        _parkEvents.AddRange(parkEvents);
        
        // 初始化維護事件
        var maintenanceEvents = new[]
        {
            new MaintenanceEvent
            {
                Id = Interlocked.Increment(ref _maintenanceEventIdCounter),
                Name = "湖心亭木材維護",
                Description = "檢查並維修湖心亭木結構，補強防腐處理",
                Type = MaintenanceType.Repair,
                FacilityId = 1,
                ScheduledDate = now.AddDays(1).Date.AddHours(8),
                EstimatedDuration = 4,
                Priority = MaintenancePriority.High,
                AssignedTo = "王維修師傅",
                Status = MaintenanceStatus.Assigned,
                Notes = "需要專業木工工具",
                CreatedAt = now.AddDays(-2)
            },
            new MaintenanceEvent
            {
                Id = Interlocked.Increment(ref _maintenanceEventIdCounter),
                Name = "步道清潔維護",
                Description = "定期清掃環湖步道，清理落葉和垃圾",
                Type = MaintenanceType.Cleaning,
                FacilityId = 2,
                ScheduledDate = now.Date.AddHours(6),
                EstimatedDuration = 2,
                Priority = MaintenancePriority.Medium,
                AssignedTo = "清潔小組",
                Status = MaintenanceStatus.InProgress,
                Notes = "天候良好，可進行清潔",
                CreatedAt = now.AddDays(-1)
            }
        };
        
        _maintenanceEvents.AddRange(maintenanceEvents);
        
        // 初始化緊急事件
        var emergencyEvents = new[]
        {
            new EmergencyEvent
            {
                Id = Interlocked.Increment(ref _emergencyEventIdCounter),
                Type = EmergencyType.Accident,
                Description = "遊客在湖邊步道滑倒受傷",
                Location = "東岸步道",
                Severity = EmergencySeverity.Medium,
                ReportedBy = "李小姐",
                ContactInfo = "0912345678",
                OccurredAt = now.AddHours(-2),
                Status = EmergencyStatus.Resolved,
                RespondedBy = "公園管理員",
                ResponseTime = now.AddHours(-1.5),
                ResolvedAt = now.AddHours(-1),
                ResolutionNotes = "協助送醫，已聯繫家屬",
                ImpactArea = "東岸步道約50公尺",
                EstimatedAffectedPeople = 1,
                Notes = "建議加強該路段防滑措施",
                CreatedAt = now.AddHours(-2)
            }
        };
        
        _emergencyEvents.AddRange(emergencyEvents);
    }
    
    #endregion
}