using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Configurations;

public class TaskTagAssociationConfiguration: IEntityTypeConfiguration<TaskTagAssociation>
{
    public void Configure(EntityTypeBuilder<TaskTagAssociation> builder)
    {
        builder.HasKey(association => new {association.TaskId, association.TagId });

        builder.HasOne(association => association.Task)
            .WithMany(task => task.TaskTagAssociations)
            .HasForeignKey(association => association.TaskId);

        builder.HasOne(association => association.Tag)
            .WithMany(tag => tag.TaskTagAssociations)
            .HasForeignKey(association => association.TagId);
    }
}