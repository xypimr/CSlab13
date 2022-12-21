using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CSlab13
{

    public class ApplicationContext : DbContext
    {
        private string _databasePath;
        
        public DbSet<Auditorium> Auditoriums { get; set; } = null!;
        public DbSet<Building> Buildings { get; set; } = null!;
        public DbSet<AuditoriumGroup> AuditoriumsGroups { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        
        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
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