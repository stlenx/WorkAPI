using System.Collections.Generic;
using WorkAPI.items;

namespace WorkAPI.DTOs
{
    public class SiteListDTO
    {
        public Dictionary<string, SiteDTO> Sites { get; set; }
    }
}