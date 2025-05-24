using IdentityService.Api.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Infrastructure
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    } 
}
