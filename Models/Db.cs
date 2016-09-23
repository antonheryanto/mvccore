using Dapper;

namespace MvcCore {
    public class Db : Database<Db>
    {
        public Table<User> Users { get; set; }
    }
}