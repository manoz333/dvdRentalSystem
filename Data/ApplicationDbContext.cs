
using DatabaseCoursework.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace groupCW.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CastMember>()
                .HasKey(o => new { o.DVDNumber, o.ActorNumber });
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }
        public DbSet<DVDCategory> DVDCategories { get; set; }
        public DbSet<DVDCopy> DVDCopies { get; set; }
        public DbSet<DVDTitle> DVDTitles { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCategory> MembershipCategories { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Studio> Studios { get; set; }
      
    }
}