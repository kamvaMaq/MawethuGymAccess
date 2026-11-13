using Microsoft.EntityFrameworkCore;
using MawethuGym.Web.Models; // <-- Required to reference your separate models

namespace MawethuGym.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
    }
}