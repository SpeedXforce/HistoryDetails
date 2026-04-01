using HistoryDetails.Models;

namespace HistoryDetails.Services.Commands
{
    public interface IHistoryCommandService
    {
        Task SaveHistoryAsync(CreateHistoryDTO createHistoryDTO);
        Task UpdateHistoryAsync(UpdateHistoryDTO dto);
    }
}
