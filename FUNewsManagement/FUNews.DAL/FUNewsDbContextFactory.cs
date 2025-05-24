using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL
{
    public class FUNewsDbContextFactory : IDesignTimeDbContextFactory<FUNewsDbContext>
    {
        public FUNewsDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") // <-- hoặc appsettings.Development.json nếu bạn dùng file này
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FUNewsDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new FUNewsDbContext(optionsBuilder.Options);
        }
    }
}
