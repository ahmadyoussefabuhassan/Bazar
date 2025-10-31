using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IAIService _aiService;

        // Constructor - هون بنستقبل الخدمة من الـ Dependency Injection
        public ChatController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatResponse>> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new ChatResponse
                    {
                        Reply = "الرسالة لا يمكن أن تكون فارغة"
                    });
                }

                var response = await _aiService.SendMessageAsync(request.Message);

                return Ok(new ChatResponse
                {
                    Reply = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ChatResponse
                {
                    Reply = $"حدث خطأ: {ex.Message}"
                });
            }

        }
    }
    public class ChatRequest
    {
        public string? Message { get; set; }
    }

    public class ChatResponse
    {
        public string? Reply { get; set; }
    }
}
