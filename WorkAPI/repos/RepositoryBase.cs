using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WorkAPI.repos
{
    public abstract class RepositoryBase
    {
        protected static string connection;

        protected RepositoryBase(IConfiguration configuration)
        {
            //Con = new NpgsqlConnection(configuration.GetConnectionString("stokerrConnection"));
            connection = configuration.GetConnectionString("stokerrConnection");
        }
    }
}