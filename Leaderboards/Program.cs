using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LeaderboardDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MainDB"), new MySqlServerVersion(new Version(8, 0, 26))));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/leaderboard", async ([FromServices] LeaderboardDbContext _context) =>
    await _context
        .Records
        .Take(5)
        .ToListAsync());

app.MapPost("/leaderboard", async ([FromServices] LeaderboardDbContext _context, LeaderboardRecord leaderboardRecord) =>
{
    await _context.Records.AddAsync(leaderboardRecord);
    await _context.SaveChangesAsync();
});

app.Run();

public class LeaderboardRecord
{
    public Guid Id;

    public string Name { get; set; } = null!;

    public int Points { get; set; }

    public int MeleeKilled { get; set; }
    
    public int RangedKilled { get; set; }
}

public class LeaderboardDbContext : DbContext
{
    public DbSet<LeaderboardRecord> Records => Set<LeaderboardRecord>();

    public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaderboardRecord>();
    }
}