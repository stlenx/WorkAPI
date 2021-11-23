using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WorkAPI.DTOs;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class ProfileRepository : RepositoryBase
    {
        public ProfileRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public string[] GetProfile(string name)
        {
            const string sql = "SELECT sites FROM profiles WHERE name = @name";

            return Con.QuerySingle<string[]>(sql, new {name});
        }

        public void AddProfile(string name, string[] sites)
        {
            const string sql = "INSERT INTO profiles (name, sites) VALUES (@name, @sites);";

            Con.Execute(sql, new {name, sites});
        }

        public Dictionary<string, string[]> GetProfiles()
        {
            const string sql = "SELECT * FROM profiles";

            var profiles = Con.Query<ProfileDTO>(sql);

            return profiles.ToDictionary(x => x.name, x => x.sites);
        }
    }
}