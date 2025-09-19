using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.ParkServices;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Controllers.ParkControllers;

/// <summary>
/// 遊客活動 API 控制器
/// </summary>
[ApiController]
[Route("api/visitor-activities")]
public class VisitorActivitiesController : ControllerBase
{
    private readonly IVisitorService _visitorService;

    public VisitorActivitiesController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    /// <summary>
    /// 取得所有活動
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitorActivityDto>>> GetActivities()
    {
        var activities = await _visitorService.GetActivitiesAsync();
        return Ok(activities);
    }

    /// <summary>
    /// 根據ID取得活動
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitorActivityDto>> GetActivity(int id)
    {
        var activity = await _visitorService.GetActivityByIdAsync(id);
        if (activity == null)
        {
            return NotFound($"找不到ID為 {id} 的活動");
        }
        return Ok(activity);
    }

    /// <summary>
    /// 創建新活動
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<VisitorActivityDto>> CreateActivity([FromBody] CreateVisitorActivityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var activity = await _visitorService.CreateActivityAsync(dto);
        return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
    }

    /// <summary>
    /// 更新活動
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(int id, [FromBody] CreateVisitorActivityDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _visitorService.UpdateActivityAsync(id, dto);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的活動");
        }

        return NoContent();
    }

    /// <summary>
    /// 刪除活動
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var success = await _visitorService.DeleteActivityAsync(id);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的活動");
        }

        return NoContent();
    }

    /// <summary>
    /// 取消活動
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelActivity(int id, [FromBody] CancelActivityDto dto)
    {
        var success = await _visitorService.CancelActivityAsync(id, dto.Reason);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的活動");
        }

        return NoContent();
    }

    /// <summary>
    /// 搜尋活動
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<VisitorActivityDto>>> SearchActivities([FromQuery] string? searchTerm)
    {
        var activities = await _visitorService.SearchActivitiesAsync(searchTerm ?? "");
        return Ok(activities);
    }

    /// <summary>
    /// 取得即將開始的活動
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<VisitorActivityDto>>> GetUpcomingActivities()
    {
        var activities = await _visitorService.GetUpcomingActivitiesAsync();
        return Ok(activities);
    }

    /// <summary>
    /// 取得正在進行的活動
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<VisitorActivityDto>>> GetActiveActivities()
    {
        var activities = await _visitorService.GetActiveActivitiesAsync();
        return Ok(activities);
    }
}

/// <summary>
/// 活動預約 API 控制器
/// </summary>
[ApiController]
[Route("api/activity-reservations")]
public class ActivityReservationsController : ControllerBase
{
    private readonly IVisitorService _visitorService;

    public ActivityReservationsController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    /// <summary>
    /// 取得所有預約
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityReservationDto>>> GetReservations()
    {
        var reservations = await _visitorService.GetReservationsAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// 根據活動ID取得預約
    /// </summary>
    [HttpGet("activity/{activityId}")]
    public async Task<ActionResult<IEnumerable<ActivityReservationDto>>> GetReservationsByActivity(int activityId)
    {
        var reservations = await _visitorService.GetReservationsByActivityAsync(activityId);
        return Ok(reservations);
    }

    /// <summary>
    /// 根據ID取得預約
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityReservationDto>> GetReservation(int id)
    {
        var reservation = await _visitorService.GetReservationByIdAsync(id);
        if (reservation == null)
        {
            return NotFound($"找不到ID為 {id} 的預約");
        }
        return Ok(reservation);
    }

    /// <summary>
    /// 創建新預約
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ActivityReservationDto>> CreateReservation([FromBody] CreateActivityReservationDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var reservation = await _visitorService.CreateReservationAsync(dto);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 確認預約
    /// </summary>
    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> ConfirmReservation(int id)
    {
        var success = await _visitorService.ConfirmReservationAsync(id);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的預約");
        }

        return NoContent();
    }

    /// <summary>
    /// 取消預約
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelReservation(int id, [FromBody] CancelReservationDto dto)
    {
        var success = await _visitorService.CancelReservationAsync(id, dto.Reason);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的預約");
        }

        return NoContent();
    }
}

/// <summary>
/// 遊客回饋 API 控制器
/// </summary>
[ApiController]
[Route("api/visitor-feedback")]
public class VisitorFeedbackController : ControllerBase
{
    private readonly IVisitorService _visitorService;

    public VisitorFeedbackController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    /// <summary>
    /// 取得所有回饋
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitorFeedbackDto>>> GetFeedbacks()
    {
        var feedbacks = await _visitorService.GetFeedbacksAsync();
        return Ok(feedbacks);
    }

    /// <summary>
    /// 根據ID取得回饋
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitorFeedbackDto>> GetFeedback(int id)
    {
        var feedback = await _visitorService.GetFeedbackByIdAsync(id);
        if (feedback == null)
        {
            return NotFound($"找不到ID為 {id} 的回饋");
        }
        return Ok(feedback);
    }

    /// <summary>
    /// 創建新回饋
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<VisitorFeedbackDto>> CreateFeedback([FromBody] CreateVisitorFeedbackDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var feedback = await _visitorService.CreateFeedbackAsync(dto);
        return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
    }

    /// <summary>
    /// 處理回饋
    /// </summary>
    [HttpPut("{id}/process")]
    public async Task<IActionResult> ProcessFeedback(int id, [FromBody] ProcessFeedbackDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _visitorService.ProcessFeedbackAsync(id, dto.ProcessedBy, dto.Response);
        if (!success)
        {
            return NotFound($"找不到ID為 {id} 的回饋");
        }

        return NoContent();
    }

    /// <summary>
    /// 取得未處理的回饋
    /// </summary>
    [HttpGet("unprocessed")]
    public async Task<ActionResult<IEnumerable<VisitorFeedbackDto>>> GetUnprocessedFeedbacks()
    {
        var feedbacks = await _visitorService.GetUnprocessedFeedbacksAsync();
        return Ok(feedbacks);
    }

    /// <summary>
    /// 根據評分取得回饋
    /// </summary>
    [HttpGet("rating/{rating}")]
    public async Task<ActionResult<IEnumerable<VisitorFeedbackDto>>> GetFeedbacksByRating(int rating)
    {
        var feedbacks = await _visitorService.GetFeedbacksByRatingAsync(rating);
        return Ok(feedbacks);
    }

    /// <summary>
    /// 取得遊客服務統計
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<VisitorServiceStatsDto>> GetStats()
    {
        var stats = await _visitorService.GetStatsAsync();
        return Ok(stats);
    }
}

/// <summary>
/// 取消活動 DTO
/// </summary>
public class CancelActivityDto
{
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// 取消預約 DTO
/// </summary>
public class CancelReservationDto
{
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// 處理回饋 DTO
/// </summary>
public class ProcessFeedbackDto
{
    public string ProcessedBy { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
}