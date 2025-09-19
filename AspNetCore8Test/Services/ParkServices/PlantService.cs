using AspNetCore8Test.Models.ParkModels;
using AspNetCore8Test.Models.DTOs.ParkDtos;
using System.Globalization;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 植栽管理服務介面
/// </summary>
public interface IPlantService
{
    /// <summary>
    /// 取得所有植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetAllPlantsAsync();

    /// <summary>
    /// 根據ID取得植物
    /// </summary>
    Task<PlantDto?> GetPlantByIdAsync(int id);

    /// <summary>
    /// 根據類型取得植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetPlantsByTypeAsync(PlantType type);

    /// <summary>
    /// 根據狀態取得植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetPlantsByStatusAsync(PlantStatus status);

    /// <summary>
    /// 搜尋植物
    /// </summary>
    Task<IEnumerable<PlantDto>> SearchPlantsAsync(string searchTerm);

    /// <summary>
    /// 建立植物
    /// </summary>
    Task<int> CreatePlantAsync(CreatePlantDto createDto);

    /// <summary>
    /// 更新植物
    /// </summary>
    Task<bool> UpdatePlantAsync(int id, UpdatePlantDto updateDto);

    /// <summary>
    /// 刪除植物
    /// </summary>
    Task<bool> DeletePlantAsync(int id);

    /// <summary>
    /// 取得需要澆水的植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetPlantsNeedingWateringAsync();

    /// <summary>
    /// 取得需要施肥的植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetPlantsNeedingFertilizingAsync();

    /// <summary>
    /// 取得需要養護的植物
    /// </summary>
    Task<IEnumerable<PlantDto>> GetPlantsNeedingCareAsync();

    /// <summary>
    /// 取得植物的養護記錄
    /// </summary>
    Task<IEnumerable<PlantCareRecordDto>> GetCareRecordsAsync(int plantId);

    /// <summary>
    /// 建立養護記錄
    /// </summary>
    Task<int> CreateCareRecordAsync(CreatePlantCareRecordDto createDto);

    /// <summary>
    /// 記錄澆水
    /// </summary>
    Task<bool> RecordWateringAsync(int plantId, string caregiverName, string notes = "");

    /// <summary>
    /// 記錄施肥
    /// </summary>
    Task<bool> RecordFertilizingAsync(int plantId, string caregiverName, string notes = "");
}

/// <summary>
/// 植栽管理服務實作
/// </summary>
public class PlantService : IPlantService
{
    // 模擬資料儲存
    private static readonly List<Plant> _plants = new();
    private static readonly List<PlantCareRecord> _careRecords = new();
    private static int _nextPlantId = 1;
    private static int _nextCareRecordId = 1;
    private static readonly object _lock = new();

    static PlantService()
    {
        InitializeSampleData();
    }

    public async Task<IEnumerable<PlantDto>> GetAllPlantsAsync()
    {
        await Task.Delay(10); // 模擬異步操作
        return _plants.Where(p => p.IsActive).Select(MapToDto);
    }

    public async Task<PlantDto?> GetPlantByIdAsync(int id)
    {
        await Task.Delay(10);
        var plant = _plants.FirstOrDefault(p => p.Id == id && p.IsActive);
        return plant != null ? MapToDto(plant) : null;
    }

    public async Task<IEnumerable<PlantDto>> GetPlantsByTypeAsync(PlantType type)
    {
        await Task.Delay(10);
        return _plants.Where(p => p.Type == type && p.IsActive).Select(MapToDto);
    }

    public async Task<IEnumerable<PlantDto>> GetPlantsByStatusAsync(PlantStatus status)
    {
        await Task.Delay(10);
        return _plants.Where(p => p.Status == status && p.IsActive).Select(MapToDto);
    }

    public async Task<IEnumerable<PlantDto>> SearchPlantsAsync(string searchTerm)
    {
        await Task.Delay(10);
        return _plants.Where(p => p.IsActive && 
            (p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             p.ScientificName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             p.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .Select(MapToDto);
    }

    public async Task<int> CreatePlantAsync(CreatePlantDto createDto)
    {
        await Task.Delay(10);
        
        int newId;
        lock (_lock)
        {
            newId = _nextPlantId++;
        }
        
        var plant = new Plant
        {
            Id = newId,
            Name = createDto.Name,
            ScientificName = createDto.ScientificName,
            Type = (PlantType)createDto.Type,
            Status = (PlantStatus)createDto.Status,
            Location = createDto.Location,
            Latitude = createDto.Latitude,
            Longitude = createDto.Longitude,
            PlantingDate = createDto.PlantingDate,
            EstimatedHeight = createDto.EstimatedHeight,
            EstimatedDiameter = createDto.EstimatedDiameter,
            WateringFrequency = createDto.WateringFrequency,
            FertilizingFrequency = createDto.FertilizingFrequency,
            Notes = createDto.Notes,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        lock (_lock)
        {
            _plants.Add(plant);
        }
        return plant.Id;
    }

    public async Task<bool> UpdatePlantAsync(int id, UpdatePlantDto updateDto)
    {
        await Task.Delay(10);
        var plant = _plants.FirstOrDefault(p => p.Id == id && p.IsActive);
        if (plant == null) return false;

        plant.Name = updateDto.Name;
        plant.ScientificName = updateDto.ScientificName;
        plant.Type = (PlantType)updateDto.Type;
        plant.Status = (PlantStatus)updateDto.Status;
        plant.Location = updateDto.Location;
        plant.Latitude = updateDto.Latitude;
        plant.Longitude = updateDto.Longitude;
        plant.EstimatedHeight = updateDto.EstimatedHeight;
        plant.EstimatedDiameter = updateDto.EstimatedDiameter;
        plant.WateringFrequency = updateDto.WateringFrequency;
        plant.FertilizingFrequency = updateDto.FertilizingFrequency;
        plant.LastWateringDate = updateDto.LastWateringDate;
        plant.LastFertilizingDate = updateDto.LastFertilizingDate;
        plant.Notes = updateDto.Notes;
        plant.IsActive = updateDto.IsActive;
        plant.UpdatedAt = DateTime.Now;

        return true;
    }

    public async Task<bool> DeletePlantAsync(int id)
    {
        await Task.Delay(10);
        var plant = _plants.FirstOrDefault(p => p.Id == id);
        if (plant == null) return false;

        plant.IsActive = false;
        plant.UpdatedAt = DateTime.Now;
        return true;
    }

    public async Task<IEnumerable<PlantDto>> GetPlantsNeedingWateringAsync()
    {
        await Task.Delay(10);
        var today = DateTime.Today;
        return _plants.Where(p => p.IsActive && p.Status != PlantStatus.Dead &&
            (p.LastWateringDate == null || p.LastWateringDate.Value.AddDays(p.WateringFrequency) <= today))
            .Select(MapToDto);
    }

    public async Task<IEnumerable<PlantDto>> GetPlantsNeedingFertilizingAsync()
    {
        await Task.Delay(10);
        var today = DateTime.Today;
        return _plants.Where(p => p.IsActive && p.Status != PlantStatus.Dead &&
            (p.LastFertilizingDate == null || p.LastFertilizingDate.Value.AddDays(p.FertilizingFrequency) <= today))
            .Select(MapToDto);
    }

    public async Task<IEnumerable<PlantDto>> GetPlantsNeedingCareAsync()
    {
        await Task.Delay(10);
        var today = DateTime.Today;
        return _plants.Where(p => p.IsActive && 
            (p.Status == PlantStatus.NeedsAttention || p.Status == PlantStatus.Sick ||
             p.LastWateringDate == null || p.LastWateringDate.Value.AddDays(p.WateringFrequency) <= today ||
             p.LastFertilizingDate == null || p.LastFertilizingDate.Value.AddDays(p.FertilizingFrequency) <= today))
            .Select(MapToDto);
    }

    public async Task<IEnumerable<PlantCareRecordDto>> GetCareRecordsAsync(int plantId)
    {
        await Task.Delay(10);
        return _careRecords.Where(c => c.PlantId == plantId).Select(MapToCareRecordDto);
    }

    public async Task<int> CreateCareRecordAsync(CreatePlantCareRecordDto createDto)
    {
        await Task.Delay(10);
        
        int newId;
        lock (_lock)
        {
            newId = _nextCareRecordId++;
        }
        
        var record = new PlantCareRecord
        {
            Id = newId,
            PlantId = createDto.PlantId,
            CareType = (CareType)createDto.CareType,
            CareDate = createDto.CareDate,
            CaregiverName = createDto.CaregiverName,
            Description = createDto.Description,
            MaterialsUsed = createDto.MaterialsUsed,
            Cost = createDto.Cost,
            NextScheduledDate = createDto.NextScheduledDate,
            Notes = createDto.Notes,
            CreatedAt = DateTime.Now
        };

        lock (_lock)
        {
            _careRecords.Add(record);
        }

        // 更新植物的最後養護日期
        var plant = _plants.FirstOrDefault(p => p.Id == createDto.PlantId);
        if (plant != null)
        {
            var careType = (CareType)createDto.CareType;
            if (careType == CareType.Watering)
            {
                plant.LastWateringDate = createDto.CareDate;
            }
            else if (careType == CareType.Fertilizing)
            {
                plant.LastFertilizingDate = createDto.CareDate;
            }
            plant.UpdatedAt = DateTime.Now;
        }

        return record.Id;
    }

    public async Task<bool> RecordWateringAsync(int plantId, string caregiverName, string notes = "")
    {
        await Task.Delay(10);
        var createDto = new CreatePlantCareRecordDto
        {
            PlantId = plantId,
            CareType = (int)CareType.Watering,
            CareDate = DateTime.Now,
            CaregiverName = caregiverName,
            Description = "植物澆水",
            Notes = notes
        };

        await CreateCareRecordAsync(createDto);
        return true;
    }

    public async Task<bool> RecordFertilizingAsync(int plantId, string caregiverName, string notes = "")
    {
        await Task.Delay(10);
        var createDto = new CreatePlantCareRecordDto
        {
            PlantId = plantId,
            CareType = (int)CareType.Fertilizing,
            CareDate = DateTime.Now,
            CaregiverName = caregiverName,
            Description = "植物施肥",
            Notes = notes
        };

        await CreateCareRecordAsync(createDto);
        return true;
    }

    private static PlantDto MapToDto(Plant plant)
    {
        return new PlantDto
        {
            Id = plant.Id,
            Name = plant.Name,
            ScientificName = plant.ScientificName,
            Type = plant.Type.ToString(),
            Status = plant.Status.ToString(),
            Location = plant.Location,
            Latitude = plant.Latitude,
            Longitude = plant.Longitude,
            PlantingDate = plant.PlantingDate,
            EstimatedHeight = plant.EstimatedHeight,
            EstimatedDiameter = plant.EstimatedDiameter,
            WateringFrequency = plant.WateringFrequency,
            FertilizingFrequency = plant.FertilizingFrequency,
            LastWateringDate = plant.LastWateringDate,
            LastFertilizingDate = plant.LastFertilizingDate,
            Notes = plant.Notes,
            IsActive = plant.IsActive,
            CreatedAt = plant.CreatedAt,
            UpdatedAt = plant.UpdatedAt
        };
    }

    private static PlantCareRecordDto MapToCareRecordDto(PlantCareRecord record)
    {
        var plant = _plants.FirstOrDefault(p => p.Id == record.PlantId);
        return new PlantCareRecordDto
        {
            Id = record.Id,
            PlantId = record.PlantId,
            PlantName = plant?.Name ?? "",
            CareType = record.CareType.ToString(),
            CareDate = record.CareDate,
            CaregiverName = record.CaregiverName,
            Description = record.Description,
            MaterialsUsed = record.MaterialsUsed,
            Cost = record.Cost,
            NextScheduledDate = record.NextScheduledDate,
            Notes = record.Notes,
            CreatedAt = record.CreatedAt
        };
    }

    private static void InitializeSampleData()
    {
        var culture = CultureInfo.InvariantCulture;
        
        // 初始化碧湖公園的示範植物數據
        _plants.AddRange(new List<Plant>
        {
            new() {
                Id = _nextPlantId++,
                Name = "台灣欒樹",
                ScientificName = "Koelreuteria elegans",
                Type = PlantType.Tree,
                Status = PlantStatus.Healthy,
                Location = "湖邊步道",
                Latitude = 25.0843M,
                Longitude = 121.5737M,
                PlantingDate = DateTime.Parse("2020-03-15", culture),
                EstimatedHeight = 800,
                EstimatedDiameter = 60,
                WateringFrequency = 7,
                FertilizingFrequency = 60,
                LastWateringDate = DateTime.Parse("2024-09-15", culture),
                LastFertilizingDate = DateTime.Parse("2024-08-01", culture),
                Notes = "公園主要行道樹種，秋季會開黃花",
                IsActive = true,
                CreatedAt = DateTime.Parse("2020-03-15", culture),
                UpdatedAt = DateTime.Parse("2024-09-15", culture)
            },
            new() {
                Id = _nextPlantId++,
                Name = "睡蓮",
                ScientificName = "Nymphaea tetragona",
                Type = PlantType.Aquatic,
                Status = PlantStatus.Healthy,
                Location = "湖心區域",
                Latitude = 25.0845M,
                Longitude = 121.5740M,
                PlantingDate = DateTime.Parse("2021-05-20", culture),
                EstimatedHeight = 15,
                EstimatedDiameter = 30,
                WateringFrequency = 365, // 水生植物不需額外澆水
                FertilizingFrequency = 90,
                LastFertilizingDate = DateTime.Parse("2024-07-15", culture),
                Notes = "湖中的美麗水生植物，夏季開花",
                IsActive = true,
                CreatedAt = DateTime.Parse("2021-05-20", culture),
                UpdatedAt = DateTime.Parse("2024-07-15", culture)
            },
            new() {
                Id = _nextPlantId++,
                Name = "茶花",
                ScientificName = "Camellia sinensis",
                Type = PlantType.Shrub,
                Status = PlantStatus.NeedsAttention,
                Location = "東側花園",
                Latitude = 25.0841M,
                Longitude = 121.5735M,
                PlantingDate = DateTime.Parse("2019-11-10", culture),
                EstimatedHeight = 150,
                EstimatedDiameter = 120,
                WateringFrequency = 3,
                FertilizingFrequency = 30,
                LastWateringDate = DateTime.Parse("2024-09-10", culture),
                LastFertilizingDate = DateTime.Parse("2024-08-20", culture),
                Notes = "需要注意蟲害防治",
                IsActive = true,
                CreatedAt = DateTime.Parse("2019-11-10", culture),
                UpdatedAt = DateTime.Parse("2024-09-10", culture)
            }
        });

        // 初始化示範養護記錄
        _careRecords.AddRange(new List<PlantCareRecord>
        {
            new() {
                Id = _nextCareRecordId++,
                PlantId = 1,
                CareType = CareType.Watering,
                CareDate = DateTime.Parse("2024-09-15", culture),
                CaregiverName = "園藝師小王",
                Description = "定期澆水",
                MaterialsUsed = "澆水器",
                Cost = 0,
                Notes = "植物狀況良好",
                CreatedAt = DateTime.Parse("2024-09-15", culture)
            },
            new() {
                Id = _nextCareRecordId++,
                PlantId = 2,
                CareType = CareType.Fertilizing,
                CareDate = DateTime.Parse("2024-07-15", culture),
                CaregiverName = "園藝師小李",
                Description = "水生植物施肥",
                MaterialsUsed = "水生植物專用肥料",
                Cost = 150,
                NextScheduledDate = DateTime.Parse("2024-10-15", culture),
                Notes = "使用緩釋性肥料",
                CreatedAt = DateTime.Parse("2024-07-15", culture)
            },
            new() {
                Id = _nextCareRecordId++,
                PlantId = 3,
                CareType = CareType.PestControl,
                CareDate = DateTime.Parse("2024-08-20", culture),
                CaregiverName = "園藝師小張",
                Description = "蟲害防治",
                MaterialsUsed = "有機殺蟲劑、噴霧器",
                Cost = 200,
                NextScheduledDate = DateTime.Parse("2024-09-20", culture),
                Notes = "發現少量蚜蟲，已進行處理",
                CreatedAt = DateTime.Parse("2024-08-20", culture)
            }
        });
    }
}