using Microsoft.EntityFrameworkCore;

namespace MawethuGym.Web.Data
{
    public class Member
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PlanType { get; set; } = "1 Month";
        public DateTime ExpiryDate { get; set; }
        public bool BiometricConsentGiven { get; set; }
    }

    public class CheckIn
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Method { get; set; } = "Face";
        public bool IsOverride { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
    }
}
