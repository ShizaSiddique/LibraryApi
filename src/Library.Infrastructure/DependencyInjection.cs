using Library.Application.Common.Interfaces;
using Library.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IBookStore, InMemoryBookStore>();

        return services;
    }
}