using Bazar.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bazar.Application.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                // إعداد الطلب
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                    new { role = "user", content = message }
                },
                    max_tokens = 500
                };

                // إضافة المفتاح
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                // إرسال الطلب
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api.openai.com/v1/chat/completions",
                    requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<OpenAIResponse>(jsonResponse);
                    return result.choices[0].message.content;
                }

                return "عذراً، حدث خطأ في الاتصال";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ: {ex.Message}";
            }
        }
    }

    // الكلاسات المساعدة
    public class OpenAIResponse
    {
        public Choice[]? choices { get; set; }
    }

    public class Choice
    {
        public Message? message { get; set; }
    }

    public class Message
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
}
