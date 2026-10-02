using Library.Application.Common.Interfaces;
using Library.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb")
            ?? throw new InvalidOperationException("Connection string 'LibraryDb' not found.");

        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(connectionString));

        // services.AddScoped<IBookStore, EfBookStore>();

         services.AddScoped<ILibraryDbContext>(provider =>
            provider.GetRequiredService<LibraryDbContext>());//asks it for the already-registered DbContext.

    ///Registers the services that check [Authorize] attributes and roles.
        services.AddAuthorization();

        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LibraryDbContext>();
        return services;
    }
}