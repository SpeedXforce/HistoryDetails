using HistoryDetails.Models;
using HistoryDetails.Services.Commands;
using HistoryDetails.Services.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HistoryDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryQueryService _queryService;
        private readonly IHistoryCommandService _commandService;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(IHistoryQueryService queryService, IHistoryCommandService commandService, ILogger<HistoryController> logger)
        {
            _queryService = queryService;
            _commandService = commandService;
            _logger = logger;
        }

        [HttpGet("ids")]
        public async Task<IActionResult> GetIds()
        {
            _logger.LogInformation("Get History Ids method called");
            var ids = await _queryService.GetHistoryIdsAsync();
            string message = $"Method returned {ids.Count()} records.";
            _logger.LogInformation(message);
            return Ok(ids);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Save([FromBody] CreateHistoryDTO dto)
        {
            _logger.LogInformation("Save history method called with data: {@dto}", dto);
            await _commandService.SaveHistoryAsync(dto);
            _logger.LogInformation("History data saved successfully");
            return Ok("Saved successfully");
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] SearchHistoryDTO dto)
        {
            _logger.LogInformation("Searching history data with criteria:{@dto}", dto);
            var result = await _queryService.SearchHistoryAsync(dto);
            _logger.LogInformation($"Search completed with {result.Count()} results");
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateHistoryDTO dto)
        {
            _logger.LogInformation("Updating history data with id: {Id}", dto.Id);
            await _commandService.UpdateHistoryAsync(dto);
            return Ok();
        }
    }
}
