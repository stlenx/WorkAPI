#nullable enable
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

        public string[]? GetProfile(string name)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "SELECT sites FROM profiles WHERE name = @name";

            try
            {
                return con.QuerySingle<string[]>(sql, new {name});
            }
            catch
            {
                return null;
            }
        }

        public void AddProfile(string name, string[] sites)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "INSERT INTO profiles (name, sites) VALUES (@name, @sites);";

            con.Execute(sql, new {name, sites});
        }

        public Dictionary<string, string[]> GetProfiles()
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "SELECT * FROM profiles";

            var profiles = con.Query<ProfileDTO>(sql);

            return profiles.ToDictionary(x => x.name, x => x.sites);
        }
    }
}