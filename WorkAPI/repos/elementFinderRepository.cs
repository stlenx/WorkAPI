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
            const string sql = "SELECT * FROM \"elementFinders\" WHERE id = @id";
            
            return Con.QuerySingle<elementFinder>(sql, new {id});
        }
        
        public int AddElementFinder(elementFinder element)
        {
            const string sql = "INSERT INTO \"elementFinders\" (type, data) VALUES (@type, @data) RETURNING id;";

            return Con.QuerySingle<int>(sql, new elementFinderDTO
            {
                type = element.type,
                data = element.data
            });
        }
        
        public bool RemoveElementFinder(int id)
        {
            const string sql = "DELETE FROM \"elementFinders\" WHERE id = @id;";
            
            try
            {
                Con.Execute(sql, new {id});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}