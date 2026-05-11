using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ZapRu.Data
{
    public class ZapRuDbContextFactory : IDesignTimeDbContextFactory<ZapRuDbContext>
    {
        public ZapRuDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ZapRuDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ZapRuDb;Username=postgres;Password=GP1A091104"); 

            return new ZapRuDbContext(optionsBuilder.Options);
        }
    }
}
