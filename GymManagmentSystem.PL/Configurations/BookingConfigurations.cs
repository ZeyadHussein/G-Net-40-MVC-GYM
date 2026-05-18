using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem.PL.Configurations
{
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            #region Relationships
            builder.HasOne(x => x.Session)
                .WithMany(x => x.SessionMembers)
                .HasForeignKey(x => x.SessionId);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.MemberSessions)
                .HasForeignKey(x => x.MemberId);

            builder.HasIndex(x => new { x.MemberId, x.SessionId });
            #endregion
        }
    }
}
