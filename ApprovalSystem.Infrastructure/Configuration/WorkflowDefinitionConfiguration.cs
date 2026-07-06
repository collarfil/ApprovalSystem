using ApprovalSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApprovalSystem.Infrastructure.Configurations
{
public class WorkflowDefinitionConfiguration
    : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(
        EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasMany(x => x.StepDefinitions)
            .WithOne(x => x.WorkflowDefinition)
            .HasForeignKey(x => x.WorkflowDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
}