using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class SiteRepository : RepositoryBase
    {
        public SiteRepository(IConfiguration configuration) : base(configuration)
        {
        }
        
        public site GetSiteById(string name)
        {
            const string sql = "SELECT * FROM websites WHERE name = @name";
            
            return Con.QuerySingle<site>(sql, new {name});
        }
        
        public Dictionary<string, site> GetSites()
        {
            const string sql = "SELECT * FROM websites";

            var sites = Con.Query<site>(sql);

            return sites.ToDictionary(x => x.name);
        }

        public int AddSite(site site)
        {
            const string sql = "INSERT INTO websites (name, url, query, \"elementFinderId\", \"resultName\", \"resultPrice\", \"resultImage\", \"resultLink\") VALUES (@name, @url, @query, @elementFinderId, @resultName, @resultPrice, @resultImage, @resultLink);";
            
            try
            {
                Con.Execute(sql, site);
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

            try
            {
                Con.Execute(sql, new {name});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}