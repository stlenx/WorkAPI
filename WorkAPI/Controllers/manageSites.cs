using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using WorkAPI.DTOs;
using WorkAPI.items;
using WorkAPI.repos;

namespace WorkAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class manageSites : ControllerBase
    {
        public static Dictionary<string, site> Sites = new();
        private readonly SiteRepository _siteRepository;
        private readonly ElementFinderRepository _elementFinderRepository;
        public manageSites(SiteRepository siteRepository, ElementFinderRepository elementFinderRepository)
        {
            _siteRepository = siteRepository;
            _elementFinderRepository = elementFinderRepository;

            if (!Sites.Any())
            {
                ReloadSites();
            }
        }

        private void ReloadSites()
        {
            Sites = _siteRepository.GetSites();
        }

        [HttpGet]
        public SiteListDTO GetSites()
        {
            var dict = new Dictionary<string, SiteDTO>();

            foreach (var (name, site) in Sites)
            {
                var element = _elementFinderRepository.GetElementById(site.elementFinderId);
                
                dict.Add(name, new SiteDTO
                {
                    name = site.name,
                    url = site.url,
                    query = site.query,
                    resultsContainer = new elementFinderDTO
                    {
                        type = element.type,
                        data = element.data
                    },
                    resultName = site.resultName,
                    resultPrice = site.resultPrice,
                    resultImage = site.resultImage,
                    resultLink = site.resultLink
                });
            }

            return new SiteListDTO
            {
                Sites = dict
            };
        }

        [HttpGet("{siteName}")]
        public ActionResult RemoveSite(string siteName)
        {
            if (!Sites.ContainsKey(siteName))
            {
                return NotFound();
            }
            
            var result = _siteRepository.RemoveSite(siteName);

            if (!result) return NotFound();

            _elementFinderRepository.RemoveElementFinder(Sites[siteName].elementFinderId);
            
            Sites.Remove(siteName);
            return Ok();
        }
        
        [HttpPost]
        public ActionResult<int> AddSite([FromBody] SiteDTO site)
        {
            if (Sites.ContainsKey(site.name))
            {
                return Conflict();
            }
            
            var elementId = _elementFinderRepository.AddElementFinder(new elementFinder
            {
                type = site.resultsContainer.type,
                data = site.resultsContainer.data
            });

            var newSite = new site
            {
                name = site.name,
                url = site.url,
                query = site.query,
                elementFinderId = elementId,
                resultName = site.resultName,
                resultPrice = site.resultPrice,
                resultImage = site.resultImage,
                resultLink = site.resultLink
            };

            var result = _siteRepository.AddSite(newSite);
            
            Sites.Add(site.name, newSite);

            return StatusCode(result);
        }

        private ActionResult DoesWebsiteExist(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                var response = request.GetResponse() as HttpWebResponse;
                response.Close();
                
                Console.WriteLine(response.StatusCode);
                
                return StatusCode((int) response.StatusCode);
            }
            catch
            {
                return StatusCode(418);
            }
        }
    }
}