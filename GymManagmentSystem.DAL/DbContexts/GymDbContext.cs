using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.dbcontext
{
    public class GymDbContext:IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer($"Server=.;Database=GymManagmentSystemDb;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
             
            #region ApplicationUser
            modelBuilder.Entity<ApplicationUser>(EB =>
            {
                EB.Property(x => x.FirstName)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);

                EB.Property(x => x.LastName)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);
            });
            #endregion
        }
    }
}
