using Holtz.SmartHealthChecks.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Holtz.SmartHealthChecks.Api.Context
{
    public class HoltzSmartHealthChecksContext : DbContext
    {
        public HoltzSmartHealthChecksContext(DbContextOptions<HoltzSmartHealthChecksContext> options) : base(options)
        { }
        public DbSet<Person> People { get; set; }
    }
}
