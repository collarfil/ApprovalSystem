using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApprovalSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFileUploadService, FileUploadService>();

        return services;
    }
}