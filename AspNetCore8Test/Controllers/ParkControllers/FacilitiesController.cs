using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;
using AspNetCore8Test.Models.ParkModels;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 設施管理控制器
/// </summary>
[ApiController]
[Route("api/park/[controller]")]
public class FacilitiesController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilitiesController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    /// <summary>
    /// 取得所有設施
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> GetAllFacilities()
    {
        var facilities = await _facilityService.GetAllFacilitiesAsync();
        return Ok(facilities);
    }

    /// <summary>
    /// 根據ID取得設施
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FacilityDto>> GetFacility(int id)
    {
        var facility = await _facilityService.GetFacilityByIdAsync(id);
        if (facility == null)
        {
            return NotFound($"找不到ID為 {id} 的設施");
        }
        return Ok(facility);
    }

    /// <summary>
    /// 根據類型取得設施
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> GetFacilitiesByType(FacilityType type)
    {
        var facilities = await _facilityService.GetFacilitiesByTypeAsync(type);
        return Ok(facilities);
    }

    /// <summary>
    /// 根據狀態取得設施
    /// </summary>
    [HttpGet("by-status/{status}")]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> GetFacilitiesByStatus(FacilityStatus status)
    {
        var facilities = await _facilityService.GetFacilitiesByStatusAsync(status);
        return Ok(facilities);
    }

    /// <summary>
    /// 搜尋設施
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> SearchFacilities([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("搜尋關鍵字不能為空");
        }

        var facilities = await _facilityService.SearchFacilitiesAsync(searchTerm);
        return Ok(facilities);
    }

    /// <summary>
    /// 取得需要維護的設施
    /// </summary>
    [HttpGet("needs-maintenance")]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> GetFacilitiesNeedingMaintenance()
    {
        var facilities = await _facilityService.GetFacilitiesNeedingMaintenanceAsync();
        return Ok(facilities);
    }

    /// <summary>
    /// 建立設施
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<int>> CreateFacility([FromBody] CreateFacilityDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var facilityId = await _facilityService.CreateFacilityAsync(createDto);
        return CreatedAtAction(nameof(GetFacility), new { id = facilityId }, facilityId);
    }

    /// <summary>
    /// 更新設施
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFacility(int id, [FromBody] UpdateFacilityDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _facilityService.UpdateFacilityAsync(id, updateDto);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的設施");
        }

        return NoContent();
    }

    /// <summary>
    /// 刪除設施
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFacility(int id)
    {
        var success = await _facilityService.DeleteFacilityAsync(id);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的設施");
        }

        return NoContent();
    }

    /// <summary>
    /// 取得設施的維護記錄
    /// </summary>
    [HttpGet("{id}/maintenance-records")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecords(int id)
    {
        var records = await _facilityService.GetMaintenanceRecordsAsync(id);
        return Ok(records);
    }

    /// <summary>
    /// 建立維護記錄
    /// </summary>
    [HttpPost("maintenance-records")]
    public async Task<ActionResult<int>> CreateMaintenanceRecord([FromBody] CreateMaintenanceRecordDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recordId = await _facilityService.CreateMaintenanceRecordAsync(createDto);
        return CreatedAtAction(nameof(GetMaintenanceRecords), new { id = createDto.FacilityId }, recordId);
    }

    /// <summary>
    /// 完成維護記錄
    /// </summary>
    [HttpPatch("maintenance-records/{recordId}/complete")]
    public async Task<ActionResult> CompleteMaintenance(int recordId, [FromBody] string? notes = null)
    {
        var success = await _facilityService.CompleteMaintenanceAsync(recordId, notes ?? "");
        if (!success)
        {
            return NotFound($"找不到ID為 {recordId} 的維護記錄");
        }

        return NoContent();
    }

    /// <summary>
    /// 取得待處理的維護記錄
    /// </summary>
    [HttpGet("maintenance-records/pending")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetPendingMaintenanceRecords()
    {
        var records = await _facilityService.GetPendingMaintenanceRecordsAsync();
        return Ok(records);
    }
}