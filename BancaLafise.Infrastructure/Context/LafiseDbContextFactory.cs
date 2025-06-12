using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace BancaLafise.Infrastructure.Context
{
    public class LafiseDbContextFactory : IDesignTimeDbContextFactory<LafiseDbContext>
    {
        public LafiseDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LafiseDbContext>();
            optionsBuilder.UseSqlite("Data Source=LAFISE.db");

            return new LafiseDbContext(optionsBuilder.Options);
        }
    }
}
