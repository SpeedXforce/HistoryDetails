namespace HistoryAI.Models
{
    public class HistoryDataCorrection
    {
        public int ID { get; set; }
        public string? HISTORY_ID { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? CorrectedValue { get; set; }
        public string? Status_Tag { get; set; }
    }
}