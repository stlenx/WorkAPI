using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WorkAPI.DTOs;

namespace WorkAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class listSites : ControllerBase
    {
        public List<string> TestingSites = new()
        {
            "PCComponentes", "Amazon", "Newegg"
        };

        [HttpGet]
        public SiteListDTO GetSites()
        {
            return new SiteListDTO()
            {
                Sites = TestingSites
            };
        }
    }
}