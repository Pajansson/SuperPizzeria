using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SuperPizzeria.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=tcp:superpizzeriaserver.database.windows.net,1433;Initial Catalog=SuperPizzeriaDb;Persist Security Info=False;User ID=Patrik_Jansson;Password=Quizer1212;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var dbContext = new ApplicationDbContext(builder.Options);

            return dbContext;
        }
    }
}
