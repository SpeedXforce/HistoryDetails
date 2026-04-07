using HistoryDetails.Models;
using HistoryDetails.Repositories.Commands;
using HistoryDetails.Services.Commands;
using Moq;
using Xunit;


namespace HistoryDetails.Tests.Services.Commands
{
    public class HistoryCommandServiceTests
    {
        private readonly Mock<IHistoryCommandRepository> _repo;
        private readonly HistoryCommandService _service;

        public HistoryCommandServiceTests()
        {
            _repo = new Mock<IHistoryCommandRepository>();
            _service = new HistoryCommandService(_repo.Object);
        }

        [Fact]
        public async Task SaveHistoryAsync_HistoryIdIsMissing()
        {
            var dto = new CreateHistoryDTO
            {
                HistoryId = null,
                Status = "{ok}",
                Timestamp = DateTime.Now,
                Value = 6527268.09
            };


           await Assert.ThrowsAsync<ArgumentException>(() => _service.SaveHistoryAsync(dto));
           _repo.Verify(r => r.SaveHistoryAsync(It.IsAny<CreateHistoryDTO>()), Times.Never);

        }

        [Fact]
        public async Task SaveHistoryAsync_StatusIsNull()
        {
            var dto = new CreateHistoryDTO
            {
                HistoryId = "Test2",
                Status = null,
                Timestamp = DateTime.Now,
                Value = 9678.08
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.SaveHistoryAsync(dto));
            _repo.Verify(r => r.SaveHistoryAsync(It.IsAny<CreateHistoryDTO>()), Times.Never);
        }

        [Fact]
        public async Task UpdateHistoryAsync_StatusIsEmptyOrNull()
        {
            var dto = new UpdateHistoryDTO
            {
                Id = 1,
                Status = "",
                Value = 8681.0
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateHistoryAsync(dto));
            _repo.Verify(r => r.UpdateHistoryAsync(It.IsAny<UpdateHistoryDTO>()), Times.Never);
  
        }


    }
}
