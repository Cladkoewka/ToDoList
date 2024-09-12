using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Configurations;

public class TaskTagAssociationConfiguration: IEntityTypeConfiguration<TaskTagAssociation>
{
    public void Configure(EntityTypeBuilder<TaskTagAssociation> builder)
    {
        builder.HasKey(tt => new { tt.ToDoTaskId, tt.TagId });

        builder.HasOne(tt => tt.ToDoTask)
            .WithMany(t => t.TaskTagAssociations)
            .HasForeignKey(tt => tt.ToDoTaskId);

        builder.HasOne(tt => tt.Tag)
            .WithMany(t => t.TaskTagAssociations)
            .HasForeignKey(tt => tt.TagId);
    }
}