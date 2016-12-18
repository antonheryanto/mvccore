using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MvcCore {
    public class Db : Database<Db>
    {
        public Table<User> Users { get; set; }
    }

	public class Db2 : Database<Db2>
	{
		public Table<User> Users { get; set; }
	}

    public static class DbService
	{
		public static System.Data.Common.DbConnection DbConn(IConfiguration config, string name = "db")
		{
			var cs = config.GetConnectionString(name);
			var cn = new MySql.Data.MySqlClient.MySqlConnection(cs);
			cn.Open();
			return cn;
		}

		public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration config)
		{
			return services.AddScoped(p => Db.Init(DbConn(config), 30))
				           .AddScoped(p => Db2.Init(DbConn(config, nameof(Db2)), 30));
		}
	}
}
