using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LeaderboardDbContext>(options =>
    options.UseMySql($"{builder.Configuration.GetConnectionString("MainDB")};database=leaderboard", new MySqlServerVersion(new Version(8, 0, 26))));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/leaderboard", async ([FromServices] LeaderboardDbContext _context) =>
    await _context
        .Records
        .OrderByDescending(r => r.Points)
        .Take(5)
        .ToListAsync());

app.MapPost("/leaderboard", async ([FromServices] LeaderboardDbContext _context, LeaderboardRecord leaderboardRecord) =>
{
    await _context.Records.AddAsync(leaderboardRecord);
    await _context.SaveChangesAsync();
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LeaderboardDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");

        throw;
    }
}

app.Run();

public class LeaderboardRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        modelBuilder.Entity<LeaderboardRecord>()
            .HasKey(r => r.Id);
    }
}
