using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem.PL.Configurations
{
    public class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(T =>
            {
                T.HasCheckConstraint("SessionCapacityConstraint", "Capacity Between 1 and 25");
                T.HasCheckConstraint("SessionEndDateAfterStartDate", "EndDate > StartDate");


                builder.HasOne(x => x.Trainer)
                    .WithMany(x => x.TrainerSessions)
                    .HasForeignKey(x => x.TrainerId);

                builder.HasOne(x => x.Category)
                    .WithMany(x => x.Sessions )
                    .HasForeignKey(x => x.CategoryId);


            });
        }
    }
}
