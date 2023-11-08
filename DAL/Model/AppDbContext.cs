using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace SolveChess.DAL.Model;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<Move> Move { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Game>()
            .HasOne(g => g.WhitePlayer)
            .WithMany()
            .HasForeignKey(g => g.WhitePlayerId);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.BlackPlayer)
            .WithMany()
            .HasForeignKey(g => g.BlackPlayerId);

        modelBuilder.Entity<Game>()
            .Property(g => g.EnpassantSquareRank)
            .IsRequired(false);

        modelBuilder.Entity<Game>()
            .Property(g => g.EnpassantSquareFile)
            .IsRequired(false);

        modelBuilder.Entity<Move>()
            .HasKey(m => new { m.GameId, m.Number, m.Side});
    }

}

