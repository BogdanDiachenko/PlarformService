using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.DataContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Platform> Platforms { get; set; }

}