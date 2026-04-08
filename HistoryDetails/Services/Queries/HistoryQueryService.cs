using HistoryDetails.Models;
using HistoryDetails.Repositories.Queries;

namespace HistoryDetails.Services.Queries
{
    public class HistoryQueryService: IHistoryQueryService
    {
        private readonly IHistoryQueryRepository _repo;
        private readonly ILogger<HistoryQueryService> _logger;

        public HistoryQueryService(IHistoryQueryRepository repo, ILogger<HistoryQueryService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<IEnumerable<string>> GetHistoryIdsAsync()
        {
            _logger.LogInformation("Getting history Ids");
           var response =  _repo.GetHistoryIdsAsync();
            _logger.LogInformation("Get history Ids completed with {Count} records", response.Result.Count());
            return response;
        }

        public Task<IEnumerable<HistoryResponseDTO>> SearchHistoryAsync(SearchHistoryDTO dto) 
        {
            if(dto.Minute < 0)
            {
                _logger.LogError("Minute value is negative:{Minute}", dto.Minute);
                throw new ArgumentException("Minute can't be a negative value");
            }

           _logger.LogInformation("Searching history with parameters: HistoryId={HistoryId}, FromDate={FromDate}, ToDate={ToDate}, Minute={Minute}", 
                dto.HistoryId, dto.FromDate, dto.ToDate, dto.Minute);
            var res =  _repo.SearchHistoryAsync(dto); 
            _logger.LogInformation("Search history completed for HistoryId={HistoryId}", dto.HistoryId);
            return res;
        }

    }
}
