using Microsoft.EntityFrameworkCore;

namespace herokudocker.Models
{
    public class MyContext : DbContext
    {
        public virtual DbSet<Video> Videos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = "ec2-46-137-84-173.eu-west-1.compute.amazonaws.com";
            var port = 5432;
            var databaseName = "d4b3kmir1pe427";
            var userName = "slppjedhxbbwnk";
            var password = "d7642369b5226d43115e5e79caf1b1d86656eae9d72e1577795e8678db055280";

            var connString = $"Server={host}; " + $"Port={port}; " +
                $"User Id={userName};" + $"Password={password};" + $"Database={databaseName};";

            optionsBuilder.UseNpgsql(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
        }
    }
}
