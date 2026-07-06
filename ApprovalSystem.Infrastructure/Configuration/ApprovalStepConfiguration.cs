using ApprovalSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApprovalSystem.Infrastructure.Configuration
{
public class ApprovalStepConfiguration
    : IEntityTypeConfiguration<ApprovalStep>
{
    public void Configure(
        EntityTypeBuilder<ApprovalStep> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment)
            .HasMaxLength(500);

        builder.HasOne(x => x.Request)
            .WithMany(x => x.ApprovalSteps)
            .HasForeignKey(x => x.RequestId);

        builder.HasOne(x => x.AssignedToUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId);
    }
}
}