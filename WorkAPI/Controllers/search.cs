#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScrapySharp.Extensions;
using WorkAPI.DTOs;
using WorkAPI.items;
using WorkAPI.repos;
using elementFinder = WorkAPI.items.elementFinder;

namespace WorkAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class search : ControllerBase
    {
        private static readonly HtmlWeb Web = new HtmlWeb();
        private static readonly HttpClient Client = new HttpClient();
        private static ElementFinderRepository? _elementFinderRepository;

        public search(ElementFinderRepository? elementFinderRepository)
        {
            _elementFinderRepository = elementFinderRepository;
        }

        [HttpGet("{siteName}/{item}")]
        public async Task<ActionResult<SearchDTO>> SearchItem(string siteName, string item)
        {
            if (!manageSites.Sites.ContainsKey(siteName))
            {
                return NotFound();
            }
            
            var site = manageSites.Sites[siteName];
            
            var result = await CheckSite($"{site.url}{site.query}{item}", site);

            return new SearchDTO
            {
                itemSearched = item,
                site = site.name,
                result = result
            };
        }

        private static async Task<SearchResult> CheckSite(string url, site site)
        {
            var html = await GetHTML(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            var container = GetElement(_elementFinderRepository.GetElementById(site.elementFinderId), doc);

            if (container == null) return new SearchResult
            {
                name = "Error, couldn't find results container",
                price = "Infinite",
                image = "",
                link = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
            };
            
            var name = container.SelectSingleNode(site.resultName);
            var price = container.SelectSingleNode(site.resultPrice);
            var image = container.SelectSingleNode(site.resultImage);
            var link = container.SelectSingleNode(site.resultLink);

            return new SearchResult
            {
                name = name.InnerText.Replace("\n", ""),
                price = price.InnerText.Replace("\n", ""),
                image = image.GetAttributeValue("src"),
                link = link.GetAttributeValue("href")
            };
        }
        

        private static HtmlNode? GetElement(elementFinder element, HtmlDocument _doc)
        {
            var result = element.type switch
            {
                "id" => _doc.GetElementbyId(element.data),
                "xpath" => _doc.DocumentNode.SelectSingleNode(element.data),
                _ => null
            };

            return result;
        }

        private static async Task<string> GetHTML(string url)
        {
            Client.DefaultRequestHeaders.Add("Accept","application/json");

            dynamic body = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)body).Add("cmd", "request.get");
            ((IDictionary<String, Object>)body).Add("url", url);

            var content = new StringContent(
                JsonConvert.SerializeObject(body), 
                Encoding.UTF8, 
                "application/json"
                );
            
            var response = await Client.PostAsync("http://192.0.4.80:8191/v1", content);

            if (!response.IsSuccessStatusCode) return "Big oof";
            
            response.EnsureSuccessStatusCode();
            
            var responseBody = await response.Content.ReadAsStringAsync();
                
            dynamic tmp = JsonConvert.DeserializeObject(responseBody);

            return tmp != null ? (string) tmp.solution.response : "Idk mate don't look at me";
        }
    }
}