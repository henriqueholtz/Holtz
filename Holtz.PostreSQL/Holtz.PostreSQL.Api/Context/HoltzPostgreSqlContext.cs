using Holtz.PostreSQL.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Holtz.PostreSQL.Api.Context
{
    public class HoltzPostgreSqlContext : DbContext
    {
        public HoltzPostgreSqlContext(DbContextOptions<HoltzPostgreSqlContext> options) : base(options)
        { }
        public DbSet<Person> People { get; set; }
    }
}
