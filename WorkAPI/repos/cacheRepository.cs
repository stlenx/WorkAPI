using System;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Npgsql;
using WorkAPI.DTOs;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class CacheRepository : RepositoryBase
    {
        public CacheRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public int AddResult(string query, string site, SearchResult search)
        {
            const string sql =
                "INSERT INTO \"cachedResults\" (query, site, \"resultName\", \"resultPrice\", \"resultImage\", \"resultLink\", \"timeStamp\") VALUES (@query, @site, @name, @price, @image, @link, @time);";

            Con.Execute(sql, new
            {
                query,
                site,
                search.name,
                search.price,
                search.image,
                search.link,
                time = DateTime.Now
            });

            return 200;
        }

        public SearchResult? GetResult(string query, string site, out DateTime timeStamp)
        {
            const string sql = "SELECT * FROM \"cachedResults\" WHERE query = @query AND site = @site";

            try
            {
                var result = Con.QuerySingle(sql, new {query, site});
                timeStamp = result.timeStamp;
                return new SearchResult
                {
                    name = result.resultName,
                    price = result.resultPrice,
                    image = result.resultImage,
                    link = result.resultLink
                };
            }
            catch
            {
                timeStamp = DateTime.MinValue;
                return null;
            }
        }

        public bool RemoveResult(string query, string site)
        {
            const string sql = "DELETE FROM \"cachedResults\" WHERE query = @query AND site = @site;";

            try
            {
                Con.Execute(sql, new {query, site});
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateResult(string query, string site, SearchResult newResult)
        {
            RemoveResult(query, site);
            
            AddResult(query, site, newResult);
        }
    }
}