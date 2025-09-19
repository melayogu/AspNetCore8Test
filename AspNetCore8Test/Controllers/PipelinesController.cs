using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PipelinesController : ControllerBase
    {
        private readonly IPipelineService _pipelineService;

        public PipelinesController(IPipelineService pipelineService)
        {
            _pipelineService = pipelineService;
        }

        // GET: api/pipelines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PipelineDto>>> GetPipelines([FromQuery] string? search = null, [FromQuery] string? status = null)
        {
            try
            {
                var pipelines = await _pipelineService.GetAllPipelinesAsync();
                
                // 搜尋篩選
                if (!string.IsNullOrEmpty(search))
                {
                    pipelines = pipelines.Where(p => 
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        p.Location.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (p.Description != null && p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    );
                }

                // 狀態篩選
                if (!string.IsNullOrEmpty(status))
                {
                    pipelines = pipelines.Where(p => p.Status == status);
                }

                return Ok(pipelines.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入管線列表時發生錯誤", error = ex.Message });
            }
        }

        // GET: api/pipelines/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPipelineStats()
        {
            try
            {
                var pipelines = await _pipelineService.GetAllPipelinesAsync();
                var alerts = await _pipelineService.GetPipelineAlertsAsync(true);

                var stats = new
                {
                    totalPipelines = pipelines.Count(),
                    activePipelines = pipelines.Count(p => p.Status == "Active"),
                    maintenancePipelines = pipelines.Count(p => p.Status == "Maintenance"),
                    activeAlerts = alerts.Count()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入統計數據時發生錯誤", error = ex.Message });
            }
        }

        // GET: api/pipelines/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PipelineDto>> GetPipeline(int id)
        {
            try
            {
                var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
                if (pipeline == null)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                return Ok(pipeline);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入管線資料時發生錯誤", error = ex.Message });
            }
        }

        // POST: api/pipelines
        [HttpPost]
        public async Task<ActionResult<PipelineDto>> CreatePipeline([FromBody] CreatePipelineDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var pipeline = await _pipelineService.CreatePipelineAsync(dto);
                return CreatedAtAction(nameof(GetPipeline), new { id = pipeline.Id }, pipeline);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "創建管線時發生錯誤", error = ex.Message });
            }
        }

        // PUT: api/pipelines/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePipeline(int id, [FromBody] UpdatePipelineDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _pipelineService.UpdatePipelineAsync(id, dto);
                if (!success)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "更新管線時發生錯誤", error = ex.Message });
            }
        }

        // DELETE: api/pipelines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePipeline(int id)
        {
            try
            {
                var success = await _pipelineService.DeletePipelineAsync(id);
                if (!success)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "刪除管線時發生錯誤", error = ex.Message });
            }
        }

        // GET: api/pipelines/{id}/monitoring
        [HttpGet("{id}/monitoring")]
        public async Task<ActionResult<IEnumerable<PipelineMonitoringDto>>> GetPipelineMonitoring(int id, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
                if (pipeline == null)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                var monitoringData = await _pipelineService.GetPipelineMonitoringDataAsync(id, fromDate, toDate);
                return Ok(monitoringData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入監控數據時發生錯誤", error = ex.Message });
            }
        }

        // GET: api/pipelines/{id}/alerts
        [HttpGet("{id}/alerts")]
        public async Task<ActionResult<IEnumerable<PipelineAlertDto>>> GetPipelineAlerts(int id, [FromQuery] bool activeOnly = false)
        {
            try
            {
                var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
                if (pipeline == null)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                var alerts = await _pipelineService.GetPipelineAlertsAsync(activeOnly);
                var pipelineAlerts = alerts.Where(a => a.PipelineId == id);

                return Ok(pipelineAlerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入警報數據時發生錯誤", error = ex.Message });
            }
        }

        // POST: api/pipelines/{id}/monitoring
        [HttpPost("{id}/monitoring")]
        public async Task<ActionResult<PipelineMonitoringDto>> AddMonitoringData(int id, [FromBody] CreatePipelineMonitoringDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
                if (pipeline == null)
                {
                    return NotFound(new { message = $"找不到ID為 {id} 的管線" });
                }

                var monitoringData = await _pipelineService.AddMonitoringDataAsync(id, dto.Pressure, dto.FlowRate, dto.Temperature, dto.Humidity);
                return CreatedAtAction(nameof(GetPipelineMonitoring), new { id = id }, monitoringData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "新增監控數據時發生錯誤", error = ex.Message });
            }
        }
    }

    // DTO for creating monitoring data
    public class CreatePipelineMonitoringDto
    {
        public decimal Pressure { get; set; }
        public decimal FlowRate { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}