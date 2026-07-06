using ApprovalSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApprovalSystem.Infrastructure.Configuration
{
public class WorkflowRuleConfiguration
    : IEntityTypeConfiguration<WorkflowRule>
{
    public void Configure(
        EntityTypeBuilder<WorkflowRule> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FieldName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Operator)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(100);
    }
}
}