using ApprovalSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Persistence;

public class ApprovalDbContext : DbContext
{
    public ApprovalDbContext(
        DbContextOptions<ApprovalDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Request> Requests => Set<Request>();

    public DbSet<ApprovalStep> ApprovalSteps => Set<ApprovalStep>();

    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();

    public DbSet<WorkflowStepDefinition> WorkflowStepDefinitions => Set<WorkflowStepDefinition>();

    public DbSet<WorkflowRule> WorkflowRules => Set<WorkflowRule>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Notification> Notifications { get; set; }


    public DbSet<FileUpload> FileUploads { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApprovalDbContext).Assembly);


        base.OnModelCreating(modelBuilder);
    }
}