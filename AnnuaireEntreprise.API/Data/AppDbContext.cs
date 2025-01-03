using AnnuaireEntreprise.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Site> Sites { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Salarie> Salaries { get; set; } = null!;
}
