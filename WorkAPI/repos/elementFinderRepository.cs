using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WorkAPI.DTOs;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class ElementFinderRepository
    {
        private readonly IConfiguration _configuration;

        public ElementFinderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public elementFinder GetElementById(int id)
        {
            const string sql = "SELECT * FROM \"elementFinders\" WHERE id = @id";
            
            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));
            
            return con.QuerySingle<elementFinder>(sql, new {id});
        }

        public int AddElementFinder(elementFinder element)
        {
            const string sql = "INSERT INTO \"elementFinders\" (type, data) VALUES (@type, @data) RETURNING id;";
            
            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));
            
            return con.QuerySingle<int>(sql, new elementFinderDTO
            {
                type = element.type,
                data = element.data
            });
        }

        public bool RemoveElementFinder(int id)
        {
            const string sql = "DELETE FROM \"elementFinders\" WHERE id = @id;";

            using var con  = new NpgsqlConnection(_configuration.GetConnectionString("stokerrConnection"));

            try
            {
                con.Execute(sql, new {id});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}