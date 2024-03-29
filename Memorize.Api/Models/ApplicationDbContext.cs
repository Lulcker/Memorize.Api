using Memorize.Api.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memorize.Api.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CollectionCard> CollectionCards { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
