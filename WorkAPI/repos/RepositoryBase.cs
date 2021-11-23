using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WorkAPI.repos
{
    public abstract class RepositoryBase
    {
        protected readonly NpgsqlConnection Con;

        protected RepositoryBase(IConfiguration configuration)
        {
            Con = new NpgsqlConnection(configuration.GetConnectionString("stokerrConnection"));
        }
    }
}