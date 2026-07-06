using ApprovalSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApprovalSystem.Infrastructure.Configuration
{
public class WorkflowStepDefinitionConfiguration
    : IEntityTypeConfiguration<WorkflowStepDefinition>
{
    public void Configure(
        EntityTypeBuilder<WorkflowStepDefinition> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StepName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}
}