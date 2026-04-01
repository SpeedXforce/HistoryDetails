using HistoryDetails.Models;

namespace HistoryDetails.Repositories.Queries
{
    public interface IHistoryQueryRepository
    {
        Task<IEnumerable<string>> GetHistoryIdsAsync();
        
        Task<IEnumerable<HistoryResponseDTO>> SearchHistoryAsync(SearchHistoryDTO searchHistoryDTO);
         
    }
}
