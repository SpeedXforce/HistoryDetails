using HistoryAI.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static HistoryAI.Models.ChatModel;

namespace HistoryAI.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ChatResponse> ChatAsync(ChatRequest request)
        {
            try
            {
                var apiKey = _config["AzureOpenAISettings:ApiKey"];
                var endpoint = _config["AzureOpenAISettings:Endpoint"];
                var deploymentName = _config["AzureOpenAISettings:DeploymentName"];
                var apiVersion = _config["AzureOpenAISettings:ApiVersion"];

                // Build URL
                var apiUrl = $"{endpoint}/openai/deployments/{deploymentName}/chat/completions?api-version={apiVersion}";
                Console.WriteLine($"Calling: {apiUrl}");

                // System prompt
                var systemPrompt = @"
                    You are a helpful assistant for a History Management System.
                    The system stores history records with:
                    - History ID  → meter identifier (e.g H001)
                    - Timestamp   → date and time of reading
                    - Status Tag  → status of the reading
                    - Value       → numerical reading
                    Keep responses short, clear and friendly.
                ";

                // Build messages
                var messages = new List<AIMessage>
                {
                    new AIMessage
                    {
                        Role    = "system",
                        Content = systemPrompt
                    }
                };

                foreach (var h in request.History)
                {
                    messages.Add(new AIMessage
                    {
                        Role = h.Role,
                        Content = h.Content
                    });
                }

                messages.Add(new AIMessage
                {
                    Role = "user",
                    Content = request.UserMessage
                });

                // Build request — NO model in body for Azure!
                var aiRequest = new
                {
                    messages = messages,
                    max_tokens = 1024
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(aiRequest, jsonOptions),
                    Encoding.UTF8,
                    "application/json"
                );

                // Azure OpenAI uses api-key header
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                // Call API
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);

                var responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseJson}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {responseJson}");
                    return new ChatResponse
                    {
                        Message = "Sorry I could not process that right now.",
                        IsSuccess = false
                    };
                }

                // Parse response
                var aiResponse = JsonSerializer.Deserialize<AIResponse>(
                    responseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var aiMessage = aiResponse?
                    .Choices?[0]?
                    .Message?
                    .Content ?? string.Empty;

                Console.WriteLine($"AI Message: '{aiMessage}'");

                if (string.IsNullOrEmpty(aiMessage))
                {
                    return new ChatResponse
                    {
                        Message = "Received your message but could not generate response.",
                        IsSuccess = false
                    };
                }

                return new ChatResponse
                {
                    Message = aiMessage,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ChatResponse
                {
                    Message = "Something went wrong. Please try again.",
                    IsSuccess = false
                };
            }
        }
    }
}