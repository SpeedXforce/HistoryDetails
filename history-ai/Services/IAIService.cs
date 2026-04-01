using static HistoryAI.Models.ChatModel;

namespace HistoryAI.Services
{
    public interface IAIService
    {
        Task<ChatResponse> ChatAsync(ChatRequest request);
    }
}
