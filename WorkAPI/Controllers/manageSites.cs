using System;
using System.Collections.Generic;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using WorkAPI.DTOs;
using WorkAPI.items;

namespace WorkAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class manageSites : ControllerBase
    {
        public static Dictionary<string, site> Sites = new();

        [HttpGet]
        public SiteListDTO GetSites()
        {
            return new SiteListDTO()
            {
                Sites = Sites
            };
        }

        [HttpPost]
        public ActionResult AddSite([FromBody] SiteDTO site)
        {
            if (Sites.ContainsKey(site.name))
            {
                return Conflict();
            }
            
            Sites.Add(site.name, new site
            {
                name = site.name,
                url = site.url
            });

            return Ok();
        }
    }
}