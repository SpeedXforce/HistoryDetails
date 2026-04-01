using HistoryDetails.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HistoryDetails.Repositories.Commands
{
    public class HistoryCommandRepository: IHistoryCommandRepository
    {
        private readonly HistoryDbContext _db;
        public HistoryCommandRepository(HistoryDbContext db)
        {
            _db = db;
        }

        public async Task SaveHistoryAsync(CreateHistoryDTO dto)
        {
            var parameters = new[]
            {
            new SqlParameter("@HistoryId", dto.HistoryId?? (object)DBNull.Value),
            new SqlParameter("@Timestamp", dto.Timestamp ?? (object)DBNull.Value),
            new SqlParameter("@Value", dto.Value ?? (object)DBNull.Value),
            new SqlParameter("@Status", dto.Status ?? (object)DBNull.Value)
        };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[Sp_SaveHistoryData] @HistoryId, @Timestamp, @Status, @Value", parameters);
        }

        public async Task UpdateHistoryAsync(UpdateHistoryDTO dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id",     dto.Id),
                new SqlParameter("@Status", dto.Status ?? (object)DBNull.Value),
                new SqlParameter("@Value",  dto.Value  ?? (object)DBNull.Value)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[Sp_UpdateHistoryData] @Id, @Status, @Value",
                parameters);
        }



    }
}
