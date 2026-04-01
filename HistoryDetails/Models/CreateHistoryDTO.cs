namespace HistoryDetails.Models
{
    public class CreateHistoryDTO
    {
        public string? HistoryId { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? Value { get; set; }
        public string? Status { get; set; }

    }
}
