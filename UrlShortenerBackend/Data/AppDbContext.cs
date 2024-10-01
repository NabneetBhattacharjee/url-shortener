using Microsoft.EntityFrameworkCore;
using UrlShortenerBackend.Models;

namespace UrlShortenerBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}