using HistoryDetails.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoryDetails.Repositories
{
    public class HistoryDbContext: DbContext
    {
        public HistoryDbContext(DbContextOptions<HistoryDbContext> options): base(options) { }

        public DbSet<HistoryDataCorrection> HistoryDataCorrections { get; set; }
        public DbSet<HistoryIds> HistoryIds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoryDataCorrection>()
                .HasNoKey()
                .ToTable("trtHistoryDataCorrections");

            modelBuilder.Entity<HistoryIds>()
                .HasNoKey()
                .ToTable("trtHistoryID");
        }
    }
}
