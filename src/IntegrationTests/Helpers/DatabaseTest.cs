using System.Data.Entity;

namespace WebApiMovieRest.IntegrationTests.Helpers
{
    public abstract class DatabaseTest<TContext> where TContext : DbContext, new()
    {
        static DatabaseTest()
        {
            // We can't rely on the context's database initializer to create the database
            // as all the tests are run within a transaction and you can't create a database within a transaction. 
            // So we need to make sure the database exists before any database tests run.

            // Unfortunatly xunit doesn't have to ability to execute a piece of code before all the tests are run
            // so we use a static constructor instead which will only run once before the very first database test
            // is executed. 

           new TContext().DropAndCreateDatabase();
        }
    }
}