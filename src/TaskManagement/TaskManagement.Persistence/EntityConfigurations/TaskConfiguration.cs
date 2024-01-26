using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Persistence.EntityConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        builder.HasQueryFilter(task => !task.IsDeleted);

        builder.Property(task => task.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(task => task.Description)
            .IsRequired()
            .HasMaxLength(1024);
    }
}
