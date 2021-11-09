using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class SiteRepository
    {
        private readonly IConfiguration _configuration;

        public SiteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public site GetSiteById(string name)
        {
            const string sql = "SELECT * FROM websites WHERE name = @name";

            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));
            
            //Do some black magic here
            
            return con.QuerySingle<site>(sql, new {name});
        }
        
        public Dictionary<string, site> GetSites()
        {
            const string sql = "SELECT * FROM websites";

            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));
            
            var sites = con.Query<site>(sql);

            return sites.ToDictionary(x => x.name);
        }

        public int AddSite(site site)
        {
            const string sql = "INSERT INTO websites (name, url, query, \"elementFinderId\", \"resultName\", \"resultPrice\", \"resultImage\", \"resultLink\") VALUES (@name, @url, @query, @elementFinderId, @resultName, @resultPrice, @resultImage, @resultLink);";

            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));

            try
            {
                con.Execute(sql, site);
                return 200;
            }
            catch
            {
                return 409;
            }
        }

        public bool RemoveSite(string name)
        {
            const string sql = "DELETE FROM websites WHERE name = @name;";

            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));

            try
            {
                con.Execute(sql, new {name});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}