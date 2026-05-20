using Microsoft.EntityFrameworkCore;
using Practice_DB_EntityFramework.Models;

namespace Practice_DB_EntityFramework.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.CharacterID);

                entity.Property(e => e.CharacterID)
                    .UseIdentityColumn(1, 1); 

                entity.Property(e => e.CharacterName)
                    .HasMaxLength(30)
                    .IsRequired(false);

                entity.Property(e => e.CharacterLevel)
                    .IsRequired(false);

                entity.Property(e => e.IsAlive)
                    .IsRequired(false);
            });

            
            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    CharacterID = 1,
                    CharacterName = "aqua",
                    CharacterLevel = 12,
                    IsAlive = true
                },
                new Character
                {
                    CharacterID = 2,
                    CharacterName = "1337",
                    CharacterLevel = 999,
                    IsAlive = true
                },
                new Character
                {
                    CharacterID = 3,
                    CharacterName = "Max",
                    CharacterLevel = 8,
                    IsAlive = false
                },
                new Character
                {
                    CharacterID = 4,
                    CharacterName = "nosk",
                    CharacterLevel = 15,
                    IsAlive = true
                }
            );
        }
    }
}