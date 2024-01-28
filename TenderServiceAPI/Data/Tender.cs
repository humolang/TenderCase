namespace TenderServiceAPI.Data
{
    public class Tender
    {
        public string? Name { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string? Url { get; set; }
    }
}
