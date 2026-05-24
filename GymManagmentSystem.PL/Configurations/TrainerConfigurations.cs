using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem.PL.Configurations
{
    public class TrainerConfigurations:GymUserConfigurations<Trainer>,IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x=>x.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GetDate()");
            base.Configure(builder);
        }
    }
}
