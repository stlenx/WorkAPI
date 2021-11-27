using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WorkAPI.DTOs;
using WorkAPI.items;

namespace WorkAPI.repos
{
    public class ElementFinderRepository : RepositoryBase
    {
        public ElementFinderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public elementFinder GetElementById(int id)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "SELECT * FROM \"elementFinders\" WHERE id = @id";
            
            return con.QuerySingle<elementFinder>(sql, new {id});
        }
        
        public int AddElementFinder(elementFinder element)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "INSERT INTO \"elementFinders\" (type, data) VALUES (@type, @data) RETURNING id;";

            return con.QuerySingle<int>(sql, new elementFinderDTO
            {
                type = element.type,
                data = element.data
            });
        }
        
        public bool RemoveElementFinder(int id)
        {
            var con = new NpgsqlConnection(connection);
            
            const string sql = "DELETE FROM \"elementFinders\" WHERE id = @id;";
            
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