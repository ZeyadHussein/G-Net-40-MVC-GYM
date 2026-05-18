using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem.PL.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
           builder.Property(c => c.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasData(

                new Category { Id = 1, CategoryName = "Cardio" },
                new Category { Id = 2, CategoryName = "Yoga" },
                new Category { Id = 3, CategoryName = "Strength" },
                new Category { Id = 4, CategoryName = "Boxing" },
                new Category { Id = 5, CategoryName = "CrossFit" }

               );
        }
    }
}
