using HistoryDetails.Models;
using HistoryDetails.Repositories.Queries;
using HistoryDetails.Services.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HistoryDetails.Tests.Services.Queries
{
    public class HistoryQueryServiceTests
    {
        private readonly Mock<IHistoryQueryRepository> _repo;
        private readonly HistoryQueryService _service;
        private readonly Mock<ILogger<HistoryQueryService>> _logger;

        public HistoryQueryServiceTests()
        {
            _repo = new Mock<IHistoryQueryRepository>();
            _logger = new Mock<ILogger<HistoryQueryService>>();
            _service = new HistoryQueryService(_repo.Object,_logger.Object);
        }

        [Fact]
        public async Task SearchHistoryAsync_MinuteIsNegative()
        {
            var dto = new SearchHistoryDTO
            {
                HistoryId = "test1",
                FromDate = DateTime.UtcNow.AddDays(-1),
                ToDate = DateTime.UtcNow.AddDays(1),
                Minute = -5
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.SearchHistoryAsync(dto));
            _repo.Verify(r=>r.SearchHistoryAsync(It.IsAny<SearchHistoryDTO>()),Times.Never);
        }
    }
}
