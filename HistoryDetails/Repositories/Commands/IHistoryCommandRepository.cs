using HistoryDetails.Models;

namespace HistoryDetails.Repositories.Commands
{
    public interface IHistoryCommandRepository
    {
        Task SaveHistoryAsync(CreateHistoryDTO createHistoryDTO);
        Task UpdateHistoryAsync(UpdateHistoryDTO updateHistoryDTO);
    }
}
