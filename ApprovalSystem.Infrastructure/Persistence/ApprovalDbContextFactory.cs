using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ApprovalSystem.Infrastructure.Persistence;

public class ApprovalDbContextFactory 
    : IDesignTimeDbContextFactory<ApprovalDbContext>
{
    public ApprovalDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "../ApprovalSystem.Api"))
            .AddJsonFile(
                "appsettings.json",
                optional: false)
            .Build();


        var optionsBuilder =
            new DbContextOptionsBuilder<ApprovalDbContext>();


        optionsBuilder.UseNpgsql(
            configuration.GetConnectionString(
                "DefaultConnection"));


        return new ApprovalDbContext(
            optionsBuilder.Options);
    }
}