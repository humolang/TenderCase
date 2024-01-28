namespace TenderServiceAPI.Data
{
    public class Tender
    {
        public string? Name { get; set; }
        public long DateBegin { get; set; }
        public long DateEnd { get; set; }
        public string? Url { get; set; }
    }
}
