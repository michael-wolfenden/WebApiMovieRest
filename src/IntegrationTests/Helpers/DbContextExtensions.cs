using System.Data.Entity;
using System.Data.SqlClient;

namespace WebApiMovieRest.IntegrationTests.Helpers
{
    public static class DbContextExtensions
    {
        public static readonly string DropDatabaseSql = @"
if (select DB_ID('{0}')) is not null
begin
    alter database [{0}] set offline with rollback immediate;
    alter database [{0}] set online;
    drop database [{0}];
end".Trim();

        public static void DropAndCreateDatabase(this DbContext dbContext)
        {
            var databaseExists = dbContext.Database.Exists();
            if (databaseExists)
            {
                var databaseName = dbContext.Database.Connection.Database;
                var connectionString = dbContext.Database.Connection.ConnectionString;
                var sql = string.Format(DropDatabaseSql, databaseName);

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            dbContext.Database.Initialize(true);
        }
    }
}