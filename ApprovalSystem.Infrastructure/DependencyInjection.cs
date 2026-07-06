using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Application.Services;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using ApprovalSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApprovalSystem.Application.Common.Settings;
using ApprovalSystem.Infrastructure.Services;




namespace ApprovalSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddDbContext<ApprovalDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(
                    "DefaultConnection"));
        });


        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IApprovalStepRepository, ApprovalStepRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkflowDefinitionRepository, WorkflowDefinitionRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IFileUploadRepository, FileUploadRepository>();
        services.AddScoped<IWorkflowStepDefinitionRepository,
        WorkflowStepDefinitionRepository>();
        services.Configure<SmtpSettings>(
    configuration.GetSection("SMTP"));
services.AddScoped<IDashboardRepository, DashboardRepository>();
services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<
    IWorkflowDefinitionRepository,
    WorkflowDefinitionRepository>();

    services.AddScoped<
        IWorkflowDefinitionService,
        WorkflowDefinitionService>();
        
        return services;
    }
}