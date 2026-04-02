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
        public async Task SaveHistoryAsync(CreateHistoryDTO dto) {

            if (string.IsNullOrEmpty(dto.HistoryId))
            {
                throw new ArgumentException("HistoryId is required");
            }

            if (dto.Value <= 0) {
                throw new ArgumentException("Value should not be less than zero!");
            }

            await _repo.SaveHistoryAsync(dto);

        } 
        public async Task UpdateHistoryAsync(UpdateHistoryDTO dto) {

            await _repo.UpdateHistoryAsync(dto);

        } 
    }
}
