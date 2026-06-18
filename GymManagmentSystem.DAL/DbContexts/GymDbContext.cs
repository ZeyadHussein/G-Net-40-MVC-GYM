using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagmentSystem.DAL.dbcontext
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(
            DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        #region DbSets

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Session> Sessions { get; set; }

        #endregion

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            // VERY IMPORTANT
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(x => x.FirstName)
                      .HasColumnType("varchar")
                      .HasMaxLength(50);

                entity.Property(x => x.LastName)
                      .HasColumnType("varchar")
                      .HasMaxLength(50);
            });
        }
    }
}