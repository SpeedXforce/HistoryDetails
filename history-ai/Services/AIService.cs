using HistoryAI.Models;
using HistoryAI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static HistoryAI.Models.ChatModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HistoryAI.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly HistoryDbContext _db;

        public AIService(HttpClient httpClient, IConfiguration config, HistoryDbContext dbContext)
        {
            _httpClient = httpClient;
            _config = config;
            _db = dbContext;
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


                var dbData = await _db.HistoryIds.FromSqlRaw("EXEC dbo.Sp_GetHistoryId")
                .ToListAsync();

                var content = string.Join("\n", dbData.Select(x=>x.History_ID));

                var systemPrompt = @"
                    You are a helpful assistant for a History Management System.
                    The system stores history records with:
                    - History ID  → meter identifier (e.g Volume or Energy)
                    - Timestamp   → date and time of reading
                    - Status Tag  → status of the reading
                    - Value       → numerical readings
                    Keep responses short, clear and friendly.
                    You can ask the user what they'd like to see,
                    surf internet and get data if u don't know something and
                    if they asks HistoryId - ask them to give an input 
                    and that input should be in this format '/Bankstown/GM203_SR_Volume'
                    It means that /(historyName)/(Volume) and you can get the content from here - 
                ";

                systemPrompt = systemPrompt + $"{content}";


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