using HistoryDetails.Models;
using HistoryDetails.Repositories.Queries;

namespace HistoryDetails.Services.Queries
{
    public class HistoryQueryService: IHistoryQueryService
    {
        private readonly IHistoryQueryRepository _repo;

        public HistoryQueryService(IHistoryQueryRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<string>> GetHistoryIdsAsync() => _repo.GetHistoryIdsAsync();

        public Task<IEnumerable<HistoryResponseDTO>> SearchHistoryAsync(SearchHistoryDTO dto) 
        {
            if(dto.Minute < 0)
            {
                throw new ArgumentException("Minute can't be a negative value");
            }

           return _repo.SearchHistoryAsync(dto); 
        }

    }
}
