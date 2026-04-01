using HistoryDetails.Models;

namespace HistoryDetails.Services.Queries
{
    public interface IHistoryQueryService
    {
        Task<IEnumerable<string>> GetHistoryIdsAsync();
        
        Task<IEnumerable<HistoryResponseDTO>> SearchHistoryAsync(SearchHistoryDTO dto);
        
    }
}
