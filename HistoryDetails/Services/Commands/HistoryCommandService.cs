using HistoryDetails.Models;
using HistoryDetails.Repositories.Commands;

namespace HistoryDetails.Services.Commands
{
    public class HistoryCommandService: IHistoryCommandService
    {
        private readonly IHistoryCommandRepository _repo;
        private readonly ILogger<HistoryCommandService> _logger;

        public HistoryCommandService(IHistoryCommandRepository repo, ILogger<HistoryCommandService> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task SaveHistoryAsync(CreateHistoryDTO dto) {

            

            if (string.IsNullOrEmpty(dto.Status))
            {
                _logger.LogError("Status is null or empty");
                throw new ArgumentException("Status should be either ok or missing");
            }

            if (dto.Value <= 0) {
                _logger.LogError("Value should not be less than zero");
                throw new ArgumentException("Value should not be less than zero!");
            }

            _logger.LogInformation("Saving history with Id: {HistoryId}", dto.HistoryId);
            await _repo.SaveHistoryAsync(dto);
            _logger.LogInformation("History with Id: {HistoryId} saved successfully", dto.HistoryId);

        } 
        public async Task UpdateHistoryAsync(UpdateHistoryDTO dto) {

            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                _logger.LogError("Status is null or empty");    
                throw new ArgumentException("Status should be either ok or missing");
            }
            _logger.LogInformation("Updating history with Id: {Id}", dto.Id);
            await _repo.UpdateHistoryAsync(dto);
            _logger.LogInformation("History with Id: {Id} updated successfully", dto.Id);

        } 
    }
}
