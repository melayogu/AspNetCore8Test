using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;
using AspNetCore8Test.Models.ParkModels;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 植栽管理控制器
/// </summary>
[ApiController]
[Route("api/park/[controller]")]
public class PlantsController : ControllerBase
{
    private readonly IPlantService _plantService;

    public PlantsController(IPlantService plantService)
    {
        _plantService = plantService;
    }

    /// <summary>
    /// 取得所有植物
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetAllPlants()
    {
        var plants = await _plantService.GetAllPlantsAsync();
        return Ok(plants);
    }

    /// <summary>
    /// 根據ID取得植物
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PlantDto>> GetPlant(int id)
    {
        var plant = await _plantService.GetPlantByIdAsync(id);
        if (plant == null)
        {
            return NotFound($"找不到ID為 {id} 的植物");
        }
        return Ok(plant);
    }

    /// <summary>
    /// 根據類型取得植物
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetPlantsByType(PlantType type)
    {
        var plants = await _plantService.GetPlantsByTypeAsync(type);
        return Ok(plants);
    }

    /// <summary>
    /// 根據狀態取得植物
    /// </summary>
    [HttpGet("by-status/{status}")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetPlantsByStatus(PlantStatus status)
    {
        var plants = await _plantService.GetPlantsByStatusAsync(status);
        return Ok(plants);
    }

    /// <summary>
    /// 搜尋植物
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> SearchPlants([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("搜尋關鍵字不能為空");
        }

        var plants = await _plantService.SearchPlantsAsync(searchTerm);
        return Ok(plants);
    }

    /// <summary>
    /// 取得需要澆水的植物
    /// </summary>
    [HttpGet("needs-watering")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetPlantsNeedingWatering()
    {
        var plants = await _plantService.GetPlantsNeedingWateringAsync();
        return Ok(plants);
    }

    /// <summary>
    /// 取得需要施肥的植物
    /// </summary>
    [HttpGet("needs-fertilizing")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetPlantsNeedingFertilizing()
    {
        var plants = await _plantService.GetPlantsNeedingFertilizingAsync();
        return Ok(plants);
    }

    /// <summary>
    /// 取得需要養護的植物
    /// </summary>
    [HttpGet("needs-care")]
    public async Task<ActionResult<IEnumerable<PlantDto>>> GetPlantsNeedingCare()
    {
        var plants = await _plantService.GetPlantsNeedingCareAsync();
        return Ok(plants);
    }

    /// <summary>
    /// 建立植物
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<int>> CreatePlant([FromBody] CreatePlantDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var plantId = await _plantService.CreatePlantAsync(createDto);
        return CreatedAtAction(nameof(GetPlant), new { id = plantId }, plantId);
    }

    /// <summary>
    /// 更新植物
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePlant(int id, [FromBody] UpdatePlantDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _plantService.UpdatePlantAsync(id, updateDto);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的植物");
        }

        return NoContent();
    }

    /// <summary>
    /// 刪除植物
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlant(int id)
    {
        var success = await _plantService.DeletePlantAsync(id);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的植物");
        }

        return NoContent();
    }

    /// <summary>
    /// 取得植物的養護記錄
    /// </summary>
    [HttpGet("{id}/care-records")]
    public async Task<ActionResult<IEnumerable<PlantCareRecordDto>>> GetCareRecords(int id)
    {
        var records = await _plantService.GetCareRecordsAsync(id);
        return Ok(records);
    }

    /// <summary>
    /// 建立養護記錄
    /// </summary>
    [HttpPost("care-records")]
    public async Task<ActionResult<int>> CreateCareRecord([FromBody] CreatePlantCareRecordDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recordId = await _plantService.CreateCareRecordAsync(createDto);
        return CreatedAtAction(nameof(GetCareRecords), new { id = createDto.PlantId }, recordId);
    }

    /// <summary>
    /// 記錄澆水
    /// </summary>
    [HttpPost("{id}/watering")]
    public async Task<ActionResult> RecordWatering(int id, [FromBody] WateringRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.CaregiverName))
        {
            return BadRequest("養護人員姓名不能為空");
        }

        var success = await _plantService.RecordWateringAsync(id, request.CaregiverName, request.Notes ?? "");
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的植物");
        }

        return NoContent();
    }

    /// <summary>
    /// 記錄施肥
    /// </summary>
    [HttpPost("{id}/fertilizing")]
    public async Task<ActionResult> RecordFertilizing(int id, [FromBody] FertilizingRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.CaregiverName))
        {
            return BadRequest("養護人員姓名不能為空");
        }

        var success = await _plantService.RecordFertilizingAsync(id, request.CaregiverName, request.Notes ?? "");
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的植物");
        }

        return NoContent();
    }
}

/// <summary>
/// 澆水請求 DTO
/// </summary>
public class WateringRequestDto
{
    public string CaregiverName { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

/// <summary>
/// 施肥請求 DTO
/// </summary>
public class FertilizingRequestDto
{
    public string CaregiverName { get; set; } = string.Empty;
    public string? Notes { get; set; }
}