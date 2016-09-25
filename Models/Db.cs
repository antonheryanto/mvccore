using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace MvcCore {
    public class Db : Database<Db>
    {
        public Table<User> Users { get; set; }
    }

    public static class DbServiceExtension {
        public static IServiceCollection DapperDb(this IServiceCollection services,  System.Data.IDbConnection cn)
        {
            cn.Open();
            return services.AddSingleton<Db>(p => Db.Init(cn, 30));
        }
    }
}