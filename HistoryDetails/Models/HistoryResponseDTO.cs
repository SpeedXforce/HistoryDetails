namespace HistoryDetails.Models
{
    public class HistoryResponseDTO
    {
        public int Id { get; set; }
        public string? MeterName { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? StatusTag { get; set; }
        public double? Value { get; set; }

    }
}
