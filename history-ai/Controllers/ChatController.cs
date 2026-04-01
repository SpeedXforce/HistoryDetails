using HistoryAI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HistoryAI.Models.ChatModel;

namespace HistoryAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAIService _aiService;

        public ChatController(IAIService aiservice)
        {
            _aiService = aiservice;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            var response = await _aiService.ChatAsync(request);
            return Ok(response);
        }
    }
}
