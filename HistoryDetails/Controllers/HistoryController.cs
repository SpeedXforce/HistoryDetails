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


        public HistoryController(IHistoryQueryService queryService, IHistoryCommandService commandService)
        {
            _queryService = queryService;
            _commandService = commandService;

        }

        [HttpGet("ids")]
        public async Task<IActionResult> GetIds()
        {
            var ids = await _queryService.GetHistoryIdsAsync();
            return Ok(ids);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Save([FromBody] CreateHistoryDTO dto)
        {
            await _commandService.SaveHistoryAsync(dto);
            return Ok("Saved successfully");
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] SearchHistoryDTO dto)
        {
            var result = await _queryService.SearchHistoryAsync(dto); 
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateHistoryDTO dto)
        {
            await _commandService.UpdateHistoryAsync(dto);
            return Ok();
        }
    }
}
