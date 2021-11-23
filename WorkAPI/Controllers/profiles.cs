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
    public class profiles : ControllerBase
    {
        private static ProfileRepository _profileRepository;

        public profiles(ProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        
        [HttpPost("add")]
        public ActionResult AddProfile([FromBody] ProfileDTO profile)
        {
            _profileRepository.AddProfile(profile.name, profile.sites);

            return Ok();
        }
        
        [HttpGet("{profile}")]
        public string[] GetProfile(string profile)
        {
            return _profileRepository.GetProfile(profile);
        }

        [HttpGet]
        public ActionResult<Dictionary<string, string[]>> GetProfiles()
        {
            return _profileRepository.GetProfiles();
        }
    }
}