using System.IO;
using DataBaseManager.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DataBaseManager
{
    public class VkDbContext: DbContext
    {
        public DbSet<PostItemTable> PostItemTable { get; set; }
        public DbSet<PostLinksTable> PostLinksTable { get; set; }
        public DbSet<PostImagesTable> PostImagesTable { get; set; }

        public VkDbContext(DbContextOptions<VkDbContext> options) : base(options)
        {
        }

        public static VkDbContext Connect()
        {
            var optionsBuilder = new DbContextOptionsBuilder<VkDbContext>();
            var conStr = new SqliteConnectionStringBuilder();
            conStr.DataSource = Path.Combine(Consts.FileDirectory, "VKParserDB.sqlite3");
            optionsBuilder.UseSqlite(conStr.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging();

            return new VkDbContext(optionsBuilder.Options);
        }
    }
}