using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace SolveChess.DAL.Model;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<GameModel> Game { get; set; }
    public DbSet<MoveModel> Move { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameModel>()
            .Property(g => g.EnpassantSquareRank)
            .IsRequired(false);

        modelBuilder.Entity<GameModel>()
            .Property(g => g.EnpassantSquareFile)
            .IsRequired(false);

        modelBuilder.Entity<MoveModel>()
            .HasKey(m => new { m.GameId, m.Number, m.Side});
    }

}

