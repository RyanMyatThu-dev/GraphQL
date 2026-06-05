using GraphQL_Test.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_Test.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
    }
}
