using HistoryDetails.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HistoryDetails.Repositories.Queries
{
    public class HistoryQueryRepository: IHistoryQueryRepository
    {
        private readonly HistoryDbContext _db;

        public HistoryQueryRepository(HistoryDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<string>> GetHistoryIdsAsync()
        {
            var result = await _db.HistoryIds
                .FromSqlRaw("EXEC dbo.Sp_GetHistoryId")
                .ToListAsync();

            return result
                .Select(h => h.History_ID);
        }

        

        public async Task<IEnumerable<HistoryResponseDTO>> SearchHistoryAsync(SearchHistoryDTO dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@HistoryId",dto.HistoryId?? (object)DBNull.Value),
                new SqlParameter("@FromDate",dto.FromDate?? (object)DBNull.Value),
                new SqlParameter("@ToDate",dto.ToDate?? (object)DBNull.Value),
                new SqlParameter("@Minute",dto.Minute ?? (object)DBNull.Value)
            };

            var raw = await _db.HistoryDataCorrections
                .FromSqlRaw("EXEC [dbo].[Sp_SearchHistoryData] @HistoryId, @FromDate, @ToDate, @Minute", parameters)
                .ToListAsync();

            return raw.Select(h => new HistoryResponseDTO
            {
                Id = h.ID,
                MeterName = h.HISTORY_ID,
                Timestamp = h.Timestamp,
                StatusTag = h.Status_Tag,
                Value = h.CorrectedValue
            });
            
        }

        

    }
}
