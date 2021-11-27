#nullable enable
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
        
        public site? GetSiteById(string name)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "SELECT * FROM websites WHERE name = @name";

            try
            {
                return con.QuerySingle<site>(sql, new {name});
            }
            catch
            {
                return null;
            }
        }
        
        public Dictionary<string, site> GetSites()
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "SELECT * FROM websites";

            var sites = con.Query<site>(sql);

            return sites.ToDictionary(x => x.name);
        }

        public int AddSite(site site)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "INSERT INTO websites (name, url, query, \"elementFinderId\", \"resultName\", \"resultPrice\", \"resultImage\", \"resultLink\") VALUES (@name, @url, @query, @elementFinderId, @resultName, @resultPrice, @resultImage, @resultLink);";
            
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
            var con = new NpgsqlConnection(connection);
            
            const string sql = "DELETE FROM websites WHERE name = @name;";

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