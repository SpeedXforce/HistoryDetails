namespace HistoryDetails.Models
{
    public class SearchHistoryDTO
    {
        public string? HistoryId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Minute { get; set; }

    }
}
