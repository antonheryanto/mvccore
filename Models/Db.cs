using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MvcCore {
    public class Db : Database<Db>
    {
        public Table<User> Users { get; set; }
    }

    //TODO support multiple Db
    public static class DbServiceExtension {
        public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration config)
        {
            var cn = new MySql.Data.MySqlClient.MySqlConnection(config.GetConnectionString("db"));
            cn.Open();
            return services.AddScoped<Db>(p => Db.Init(cn, 30));
        }
    }
}
