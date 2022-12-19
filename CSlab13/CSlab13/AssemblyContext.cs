using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CSlab13
{

    public class AssemblyContext : DbContext
    {
        private string _databasePath;
        
        public DbSet<Detail> Details { get; set; } = null!;
        public DbSet<Assembly> Assemblies { get; set; } = null!;
        public DbSet<Part> Parts { get; set; } = null!;

        public AssemblyContext()
        {
            Database.EnsureCreated();
        }
        
        public AssemblyContext(string databasePath)
        {
            _databasePath = databasePath;
            Database.EnsureCreated();
        }

        public AssemblyContext(DbContextOptions<AssemblyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // if (!optionsBuilder.IsConfigured)
            // {
            //     IConfigurationRoot configuration = new ConfigurationBuilder()
            //         .SetBasePath(Directory.GetCurrentDirectory())
            //         .AddJsonFile("appsettings.json")
            //         .Build();
            //     var connectionString = configuration.GetConnectionString("DefaultConnection");
            //     optionsBuilder.UseSqlite(connectionString);
            // }
            
            // optionsBuilder.UseSqlite("Data Source = ../../../Production.db");

            optionsBuilder.UseSqlite($"Filename={_databasePath}");
            SQLitePCL.Batteries.Init();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}