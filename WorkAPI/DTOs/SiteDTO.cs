namespace WorkAPI.DTOs
{
    public class SiteDTO
    {
        public string name { get; set; }
        public string url { get; set; }
        public string query { get; set; }
        public elementFinderDTO resultsContainer { get; set; }
        public string resultName { get; set; }
        public string resultPrice { get; set; }
        public string resultImage { get; set; }
        public string resultLink { get; set; }
    }

    public struct elementFinderDTO
    {
        public string type { get; set; }
        public string data { get; set; }
    }
}