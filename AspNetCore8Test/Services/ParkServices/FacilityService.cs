using AspNetCore8Test.Models.ParkModels;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 設施管理服務介面
/// </summary>
public interface IFacilityService
{
    /// <summary>
    /// 取得所有設施
    /// </summary>
    Task<IEnumerable<FacilityDto>> GetAllFacilitiesAsync();

    /// <summary>
    /// 根據ID取得設施
    /// </summary>
    Task<FacilityDto?> GetFacilityByIdAsync(int id);

    /// <summary>
    /// 根據類型取得設施
    /// </summary>
    Task<IEnumerable<FacilityDto>> GetFacilitiesByTypeAsync(FacilityType type);

    /// <summary>
    /// 根據狀態取得設施
    /// </summary>
    Task<IEnumerable<FacilityDto>> GetFacilitiesByStatusAsync(FacilityStatus status);

    /// <summary>
    /// 搜尋設施
    /// </summary>
    Task<IEnumerable<FacilityDto>> SearchFacilitiesAsync(string searchTerm);

    /// <summary>
    /// 建立設施
    /// </summary>
    Task<int> CreateFacilityAsync(CreateFacilityDto createDto);

    /// <summary>
    /// 更新設施
    /// </summary>
    Task<bool> UpdateFacilityAsync(int id, UpdateFacilityDto updateDto);

    /// <summary>
    /// 刪除設施
    /// </summary>
    Task<bool> DeleteFacilityAsync(int id);

    /// <summary>
    /// 取得需要維護的設施
    /// </summary>
    Task<IEnumerable<FacilityDto>> GetFacilitiesNeedingMaintenanceAsync();

    /// <summary>
    /// 取得設施的維護記錄
    /// </summary>
    Task<IEnumerable<MaintenanceRecordDto>> GetMaintenanceRecordsAsync(int facilityId);

    /// <summary>
    /// 建立維護記錄
    /// </summary>
    Task<int> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto createDto);

    /// <summary>
    /// 完成維護記錄
    /// </summary>
    Task<bool> CompleteMaintenanceAsync(int recordId, string notes = "");

    /// <summary>
    /// 取得待處理的維護記錄
    /// </summary>
    Task<IEnumerable<MaintenanceRecordDto>> GetPendingMaintenanceRecordsAsync();
}

/// <summary>
/// 設施管理服務實作
/// </summary>
public class FacilityService : IFacilityService
{
    // 模擬資料儲存
    private static readonly List<Facility> _facilities = new();
    private static readonly List<MaintenanceRecord> _maintenanceRecords = new();
    private static int _nextFacilityId = 1;
    private static int _nextMaintenanceId = 1;
    private static readonly object _lock = new();

    static FacilityService()
    {
        InitializeSampleData();
    }

    public async Task<IEnumerable<FacilityDto>> GetAllFacilitiesAsync()
    {
        await Task.Delay(10); // 模擬異步操作
        return _facilities.Where(f => f.IsActive).Select(MapToDto);
    }

    public async Task<FacilityDto?> GetFacilityByIdAsync(int id)
    {
        await Task.Delay(10);
        var facility = _facilities.FirstOrDefault(f => f.Id == id && f.IsActive);
        return facility != null ? MapToDto(facility) : null;
    }

    public async Task<IEnumerable<FacilityDto>> GetFacilitiesByTypeAsync(FacilityType type)
    {
        await Task.Delay(10);
        return _facilities.Where(f => f.Type == type && f.IsActive).Select(MapToDto);
    }

    public async Task<IEnumerable<FacilityDto>> GetFacilitiesByStatusAsync(FacilityStatus status)
    {
        await Task.Delay(10);
        return _facilities.Where(f => f.Status == status && f.IsActive).Select(MapToDto);
    }

    public async Task<IEnumerable<FacilityDto>> SearchFacilitiesAsync(string searchTerm)
    {
        await Task.Delay(10);
        return _facilities.Where(f => f.IsActive && 
            (f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             f.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             f.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .Select(MapToDto);
    }

    public async Task<int> CreateFacilityAsync(CreateFacilityDto createDto)
    {
        await Task.Delay(10);
        
        int newId;
        lock (_lock)
        {
            newId = _nextFacilityId++;
        }
        
        var facility = new Facility
        {
            Id = newId,
            Name = createDto.Name,
            Description = createDto.Description,
            Type = (FacilityType)createDto.Type,
            Status = (FacilityStatus)createDto.Status,
            Location = createDto.Location,
            Latitude = createDto.Latitude,
            Longitude = createDto.Longitude,
            InstallationDate = createDto.InstallationDate,
            LastMaintenanceDate = createDto.LastMaintenanceDate,
            NextMaintenanceDate = createDto.NextMaintenanceDate,
            MaintenanceCost = createDto.MaintenanceCost,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        lock (_lock)
        {
            _facilities.Add(facility);
        }
        return facility.Id;
    }

    public async Task<bool> UpdateFacilityAsync(int id, UpdateFacilityDto updateDto)
    {
        await Task.Delay(10);
        var facility = _facilities.FirstOrDefault(f => f.Id == id && f.IsActive);
        if (facility == null) return false;

        facility.Name = updateDto.Name;
        facility.Description = updateDto.Description;
        facility.Type = (FacilityType)updateDto.Type;
        facility.Status = (FacilityStatus)updateDto.Status;
        facility.Location = updateDto.Location;
        facility.Latitude = updateDto.Latitude;
        facility.Longitude = updateDto.Longitude;
        facility.LastMaintenanceDate = updateDto.LastMaintenanceDate;
        facility.NextMaintenanceDate = updateDto.NextMaintenanceDate;
        facility.MaintenanceCost = updateDto.MaintenanceCost;
        facility.IsActive = updateDto.IsActive;
        facility.UpdatedAt = DateTime.Now;

        return true;
    }

    public async Task<bool> DeleteFacilityAsync(int id)
    {
        await Task.Delay(10);
        var facility = _facilities.FirstOrDefault(f => f.Id == id);
        if (facility == null) return false;

        facility.IsActive = false;
        facility.UpdatedAt = DateTime.Now;
        return true;
    }

    public async Task<IEnumerable<FacilityDto>> GetFacilitiesNeedingMaintenanceAsync()
    {
        await Task.Delay(10);
        var today = DateTime.Today;
        return _facilities.Where(f => f.IsActive &&
            (f.Status == FacilityStatus.NeedsMaintenance ||
             (f.NextMaintenanceDate.HasValue && f.NextMaintenanceDate.Value <= today.AddDays(7))))
            .Select(MapToDto);
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetMaintenanceRecordsAsync(int facilityId)
    {
        await Task.Delay(10);
        return _maintenanceRecords.Where(m => m.FacilityId == facilityId).Select(MapToMaintenanceDto);
    }

    public async Task<int> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto createDto)
    {
        await Task.Delay(10);
        
        int newId;
        lock (_lock)
        {
            newId = _nextMaintenanceId++;
        }
        
        var record = new MaintenanceRecord
        {
            Id = newId,
            FacilityId = createDto.FacilityId,
            MaintenanceType = createDto.MaintenanceType,
            Description = createDto.Description,
            Status = (MaintenanceStatus)createDto.Status,
            ScheduledDate = createDto.ScheduledDate,
            AssignedTo = createDto.AssignedTo,
            Cost = createDto.Cost,
            Notes = createDto.Notes,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        lock (_lock)
        {
            _maintenanceRecords.Add(record);
        }
        return record.Id;
    }

    public async Task<bool> CompleteMaintenanceAsync(int recordId, string notes = "")
    {
        await Task.Delay(10);
        var record = _maintenanceRecords.FirstOrDefault(r => r.Id == recordId);
        if (record == null) return false;

        record.Status = MaintenanceStatus.Completed;
        record.CompletedDate = DateTime.Now;
        if (!string.IsNullOrEmpty(notes))
        {
            record.Notes = notes;
        }
        record.UpdatedAt = DateTime.Now;

        // 更新設施的最後維護日期
        var facility = _facilities.FirstOrDefault(f => f.Id == record.FacilityId);
        if (facility != null)
        {
            facility.LastMaintenanceDate = DateTime.Now;
            facility.Status = FacilityStatus.Normal;
            facility.UpdatedAt = DateTime.Now;
        }

        return true;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetPendingMaintenanceRecordsAsync()
    {
        await Task.Delay(10);
        return _maintenanceRecords.Where(m => m.Status == MaintenanceStatus.Scheduled || m.Status == MaintenanceStatus.InProgress)
            .Select(MapToMaintenanceDto);
    }

    private static FacilityDto MapToDto(Facility facility)
    {
        return new FacilityDto
        {
            Id = facility.Id,
            Name = facility.Name,
            Description = facility.Description,
            Type = facility.Type.ToString(),
            Status = facility.Status.ToString(),
            Location = facility.Location,
            Latitude = facility.Latitude,
            Longitude = facility.Longitude,
            InstallationDate = facility.InstallationDate,
            LastMaintenanceDate = facility.LastMaintenanceDate,
            NextMaintenanceDate = facility.NextMaintenanceDate,
            MaintenanceCost = facility.MaintenanceCost,
            IsActive = facility.IsActive,
            CreatedAt = facility.CreatedAt,
            UpdatedAt = facility.UpdatedAt
        };
    }

    private static MaintenanceRecordDto MapToMaintenanceDto(MaintenanceRecord record)
    {
        var facility = _facilities.FirstOrDefault(f => f.Id == record.FacilityId);
        return new MaintenanceRecordDto
        {
            Id = record.Id,
            FacilityId = record.FacilityId,
            FacilityName = facility?.Name ?? "",
            MaintenanceType = record.MaintenanceType,
            Description = record.Description,
            Status = record.Status.ToString(),
            ScheduledDate = record.ScheduledDate,
            CompletedDate = record.CompletedDate,
            AssignedTo = record.AssignedTo,
            Cost = record.Cost,
            Notes = record.Notes,
            CreatedAt = record.CreatedAt
        };
    }

    private static void InitializeSampleData()
    {
        // 初始化碧湖公園的示範設施數據
        _facilities.AddRange(new List<Facility>
        {
            new() {
                Id = _nextFacilityId++,
                Name = "兒童遊樂區",
                Description = "適合3-12歲兒童的安全遊樂設施",
                Type = FacilityType.Playground,
                Status = FacilityStatus.Normal,
                Location = "公園東側",
                Latitude = 25.0842M,
                Longitude = 121.5736M,
                InstallationDate = DateTime.Parse("2020-03-15"),
                LastMaintenanceDate = DateTime.Parse("2024-08-15"),
                NextMaintenanceDate = DateTime.Parse("2024-11-15"),
                MaintenanceCost = 15000,
                IsActive = true,
                CreatedAt = DateTime.Parse("2020-03-15"),
                UpdatedAt = DateTime.Parse("2024-08-15")
            },
            new() {
                Id = _nextFacilityId++,
                Name = "湖心亭",
                Description = "位於湖中央的觀景涼亭",
                Type = FacilityType.Landscape,
                Status = FacilityStatus.Normal,
                Location = "湖心島",
                Latitude = 25.0845M,
                Longitude = 121.5740M,
                InstallationDate = DateTime.Parse("2018-06-20"),
                LastMaintenanceDate = DateTime.Parse("2024-07-10"),
                NextMaintenanceDate = DateTime.Parse("2024-10-10"),
                MaintenanceCost = 8000,
                IsActive = true,
                CreatedAt = DateTime.Parse("2018-06-20"),
                UpdatedAt = DateTime.Parse("2024-07-10")
            },
            new() {
                Id = _nextFacilityId++,
                Name = "環湖步道照明",
                Description = "環湖步道的LED照明系統",
                Type = FacilityType.Lighting,
                Status = FacilityStatus.NeedsMaintenance,
                Location = "環湖步道",
                Latitude = 25.0843M,
                Longitude = 121.5738M,
                InstallationDate = DateTime.Parse("2019-09-10"),
                LastMaintenanceDate = DateTime.Parse("2024-05-20"),
                NextMaintenanceDate = DateTime.Parse("2024-09-20"),
                MaintenanceCost = 25000,
                IsActive = true,
                CreatedAt = DateTime.Parse("2019-09-10"),
                UpdatedAt = DateTime.Parse("2024-05-20")
            }
        });

        // 初始化示範維護記錄
        _maintenanceRecords.AddRange(new List<MaintenanceRecord>
        {
            new() {
                Id = _nextMaintenanceId++,
                FacilityId = 1,
                MaintenanceType = "定期保養",
                Description = "遊樂設施安全檢查與清潔",
                Status = MaintenanceStatus.Completed,
                ScheduledDate = DateTime.Parse("2024-08-15"),
                CompletedDate = DateTime.Parse("2024-08-15"),
                AssignedTo = "維護團隊A",
                Cost = 5000,
                Notes = "所有設施運作正常",
                CreatedAt = DateTime.Parse("2024-08-01"),
                UpdatedAt = DateTime.Parse("2024-08-15")
            },
            new() {
                Id = _nextMaintenanceId++,
                FacilityId = 3,
                MaintenanceType = "故障維修",
                Description = "LED燈具更換",
                Status = MaintenanceStatus.Scheduled,
                ScheduledDate = DateTime.Parse("2024-09-25"),
                AssignedTo = "電工團隊",
                Cost = 12000,
                Notes = "需更換20盞LED燈具",
                CreatedAt = DateTime.Parse("2024-09-15"),
                UpdatedAt = DateTime.Parse("2024-09-15")
            }
        });
    }
}