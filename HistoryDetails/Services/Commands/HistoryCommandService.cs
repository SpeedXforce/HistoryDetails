using HistoryDetails.Models;
using HistoryDetails.Repositories.Commands;

namespace HistoryDetails.Services.Commands
{
    public class HistoryCommandService: IHistoryCommandService
    {
        private readonly IHistoryCommandRepository _repo;

        public HistoryCommandService(IHistoryCommandRepository repo)
        {
            _repo = repo;
        }
        public Task SaveHistoryAsync(CreateHistoryDTO dto) => _repo.SaveHistoryAsync(dto);
        public Task UpdateHistoryAsync(UpdateHistoryDTO dto) => _repo.UpdateHistoryAsync(dto);
    }
}
