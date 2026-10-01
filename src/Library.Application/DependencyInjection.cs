using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class DependencyInjection
{ public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        //scans the Application project and registers every handler automatically.
        //  You never register handlers one by one; adding a new handler file is enough.
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}