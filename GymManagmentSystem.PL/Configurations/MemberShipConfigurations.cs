using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem.PL.Configurations
{
    public class MemberShipConfigurations : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
           builder.HasKey(m=>m.Id);

            builder.Property(x=>x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m=>m.Plan)
                .WithMany(p=>p.PlanMembers)
                .HasForeignKey(m=>m.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m=>m.Member)
                .WithMany(m=>m.MemberPlans)
                .HasForeignKey(m=>m.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
