namespace WorkAPI.items
{
    public class site
    {
        public string name { get; set; }
        public string url { get; set; }
        public string query { get; set; }
        public int elementFinderId { get; set; }
        public string resultName { get; set; }
        public string resultPrice { get; set; }
        public string resultImage { get; set; }
        public string resultLink { get; set; }
    }
    public struct elementFinder
    {
        public int id { get; set; }
        public string type { get; set; }
        public string data { get; set; }
    }
}