using System;

namespace WorkAPI.DTOs
{
    public class SearchDTO
    {
        public string itemSearched { get; set; }
        public string site { get; set; }
        public bool cached { get; set; }
        public DateTime date { get; set; }
        public SearchResult result { get; set; }
    }

    public struct SearchResult
    {
        public string name { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string link { get; set; }
    }
}