using Microsoft.EntityFrameworkCore;
using CricketVerse.Models;

namespace CricketVerse.Data
{
    public class CricketVerseContext : DbContext
    {
        public CricketVerseContext(DbContextOptions<CricketVerseContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.Teams)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Team entity
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithMany(p => p.Teams)
                .UsingEntity(j => j.ToTable("TeamPlayers"));

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Captain)
                .WithMany()
                .HasForeignKey(t => t.CaptainId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.ViceCaptain)
                .WithMany()
                .HasForeignKey(t => t.ViceCaptainId)
                .OnDelete(DeleteBehavior.Restrict);

            // SQLite doesn't support decimal precision, so we remove those configurations
        }
    }
}