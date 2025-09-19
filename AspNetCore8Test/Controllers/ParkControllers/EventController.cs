using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 公園事件管理控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ParkEventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public ParkEventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// 取得所有公園事件
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkEventDto>>> GetEvents()
    {
        var events = await _eventService.GetEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 根據ID取得公園事件
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ParkEventDto>> GetEvent(int id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        
        if (eventItem == null)
            return NotFound($"找不到ID為 {id} 的事件");
        
        return Ok(eventItem);
    }

    /// <summary>
    /// 建立新的公園事件
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ParkEventDto>> CreateEvent(CreateParkEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var eventItem = await _eventService.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 更新公園事件
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, CreateParkEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _eventService.UpdateEventAsync(id, dto);
            
            if (!success)
                return NotFound($"找不到ID為 {id} 的事件");
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 刪除公園事件
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var success = await _eventService.DeleteEventAsync(id);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的事件");
        
        return NoContent();
    }

    /// <summary>
    /// 搜尋公園事件
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ParkEventDto>>> SearchEvents([FromQuery] string searchTerm)
    {
        var events = await _eventService.SearchEventsAsync(searchTerm);
        return Ok(events);
    }

    /// <summary>
    /// 根據類型篩選事件
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<IEnumerable<ParkEventDto>>> GetEventsByType(int type)
    {
        var events = await _eventService.GetEventsByTypeAsync(type);
        return Ok(events);
    }

    /// <summary>
    /// 取得即將發生的事件
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<ParkEventDto>>> GetUpcomingEvents([FromQuery] int days = 7)
    {
        var events = await _eventService.GetUpcomingEventsAsync(days);
        return Ok(events);
    }

    /// <summary>
    /// 取得目前進行中的事件
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ParkEventDto>>> GetActiveEvents()
    {
        var events = await _eventService.GetActiveEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 取得事件統計
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<EventStatsDto>> GetEventStats()
    {
        var stats = await _eventService.GetEventStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// 完成事件
    /// </summary>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteEvent(int id, [FromBody] string completionNotes)
    {
        var success = await _eventService.CompleteEventAsync(id, completionNotes);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的事件");
        
        return NoContent();
    }

    /// <summary>
    /// 取消事件
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelEvent(int id, [FromBody] string cancellationReason)
    {
        var success = await _eventService.CancelEventAsync(id, cancellationReason);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的事件");
        
        return NoContent();
    }
}

/// <summary>
/// 維護事件管理控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MaintenanceEventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public MaintenanceEventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// 取得所有維護事件
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetMaintenanceEvents()
    {
        var events = await _eventService.GetMaintenanceEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 根據ID取得維護事件
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceEventDto>> GetMaintenanceEvent(int id)
    {
        var eventItem = await _eventService.GetMaintenanceEventByIdAsync(id);
        
        if (eventItem == null)
            return NotFound($"找不到ID為 {id} 的維護事件");
        
        return Ok(eventItem);
    }

    /// <summary>
    /// 建立新的維護事件
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MaintenanceEventDto>> CreateMaintenanceEvent(CreateMaintenanceEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var eventItem = await _eventService.CreateMaintenanceEventAsync(dto);
            return CreatedAtAction(nameof(GetMaintenanceEvent), new { id = eventItem.Id }, eventItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 更新維護事件
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceEvent(int id, CreateMaintenanceEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _eventService.UpdateMaintenanceEventAsync(id, dto);
            
            if (!success)
                return NotFound($"找不到ID為 {id} 的維護事件");
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 刪除維護事件
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceEvent(int id)
    {
        var success = await _eventService.DeleteMaintenanceEventAsync(id);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的維護事件");
        
        return NoContent();
    }

    /// <summary>
    /// 取得今日維護事件
    /// </summary>
    [HttpGet("today")]
    public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetTodayMaintenanceEvents()
    {
        var events = await _eventService.GetTodayMaintenanceEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 根據設施篩選維護事件
    /// </summary>
    [HttpGet("by-facility/{facilityId}")]
    public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetMaintenanceEventsByFacility(int facilityId)
    {
        var events = await _eventService.GetMaintenanceEventsByFacilityAsync(facilityId);
        return Ok(events);
    }

    /// <summary>
    /// 根據狀態篩選維護事件
    /// </summary>
    [HttpGet("by-status/{status}")]
    public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetMaintenanceEventsByStatus(int status)
    {
        var events = await _eventService.GetMaintenanceEventsByStatusAsync(status);
        return Ok(events);
    }

    /// <summary>
    /// 指派維護事件
    /// </summary>
    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignMaintenanceEvent(int id, [FromBody] string assignTo)
    {
        var success = await _eventService.AssignMaintenanceEventAsync(id, assignTo);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的維護事件");
        
        return NoContent();
    }

    /// <summary>
    /// 完成維護事件
    /// </summary>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteMaintenanceEvent(int id, [FromBody] string completionNotes)
    {
        var success = await _eventService.CompleteMaintenanceEventAsync(id, completionNotes);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的維護事件");
        
        return NoContent();
    }
}

/// <summary>
/// 緊急事件管理控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmergencyEventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EmergencyEventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// 取得所有緊急事件
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmergencyEventDto>>> GetEmergencyEvents()
    {
        var events = await _eventService.GetEmergencyEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 根據ID取得緊急事件
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmergencyEventDto>> GetEmergencyEvent(int id)
    {
        var eventItem = await _eventService.GetEmergencyEventByIdAsync(id);
        
        if (eventItem == null)
            return NotFound($"找不到ID為 {id} 的緊急事件");
        
        return Ok(eventItem);
    }

    /// <summary>
    /// 建立新的緊急事件
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmergencyEventDto>> CreateEmergencyEvent(CreateEmergencyEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var eventItem = await _eventService.CreateEmergencyEventAsync(dto);
            return CreatedAtAction(nameof(GetEmergencyEvent), new { id = eventItem.Id }, eventItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 更新緊急事件
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmergencyEvent(int id, CreateEmergencyEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _eventService.UpdateEmergencyEventAsync(id, dto);
            
            if (!success)
                return NotFound($"找不到ID為 {id} 的緊急事件");
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 刪除緊急事件
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmergencyEvent(int id)
    {
        var success = await _eventService.DeleteEmergencyEventAsync(id);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的緊急事件");
        
        return NoContent();
    }

    /// <summary>
    /// 取得待處理的緊急事件
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<EmergencyEventDto>>> GetPendingEmergencyEvents()
    {
        var events = await _eventService.GetPendingEmergencyEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// 根據嚴重程度篩選緊急事件
    /// </summary>
    [HttpGet("by-severity/{severity}")]
    public async Task<ActionResult<IEnumerable<EmergencyEventDto>>> GetEmergencyEventsBySeverity(int severity)
    {
        var events = await _eventService.GetEmergencyEventsBySeverityAsync(severity);
        return Ok(events);
    }

    /// <summary>
    /// 回應緊急事件
    /// </summary>
    [HttpPut("{id}/respond")]
    public async Task<IActionResult> RespondToEmergencyEvent(int id, [FromBody] string responder)
    {
        var success = await _eventService.RespondToEmergencyEventAsync(id, responder);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的緊急事件");
        
        return NoContent();
    }

    /// <summary>
    /// 解決緊急事件
    /// </summary>
    [HttpPut("{id}/resolve")]
    public async Task<IActionResult> ResolveEmergencyEvent(int id, [FromBody] string resolutionNotes)
    {
        var success = await _eventService.ResolveEmergencyEventAsync(id, resolutionNotes);
        
        if (!success)
            return NotFound($"找不到ID為 {id} 的緊急事件");
        
        return NoContent();
    }
}