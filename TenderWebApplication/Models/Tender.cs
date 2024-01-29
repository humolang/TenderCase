namespace TenderWebApplication.Models
{
    // Data-класс для хранения информации о тендере
    public class Tender
    {
        public string? Name { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string? Url { get; set; }
    }
}
