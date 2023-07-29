using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Entities
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<DynamicLinkLibrary> Libraries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = new MySqlConnection("server=139.155.74.96;port=3307;user=root;password=zxcd!@mysql3306.1;database=his_dev");
            var version = ServerVersion.AutoDetect(conn);

            optionsBuilder
                .UseMySql(conn, version)
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}