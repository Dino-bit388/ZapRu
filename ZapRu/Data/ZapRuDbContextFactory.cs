using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ZapRu.Data
{
    public class ZapRuDbContextFactory : IDesignTimeDbContextFactory<ZapRuDbContext>
    {
        public ZapRuDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ZapRuDbContext>();

            optionsBuilder.UseSqlite("Data Source=app.db");

            return new ZapRuDbContext(optionsBuilder.Options);
        }
    }
}
