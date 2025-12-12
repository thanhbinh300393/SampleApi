using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Sample.Common.Dependency;

namespace Sample.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConventionalServices(
            this IServiceCollection services, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                // Transient
                if (interfaces.Contains(typeof(ITransientDependency)))
                {
                    var serviceInterfaces = interfaces.Where(i => i != typeof(ITransientDependency));
                    foreach (var serviceType in serviceInterfaces)
                        services.AddTransient(serviceType, type);
                }

                // Scoped
                if (interfaces.Contains(typeof(IScopedDependency)))
                {
                    var serviceInterfaces = interfaces.Where(i => i != typeof(IScopedDependency));
                    foreach (var serviceType in serviceInterfaces)
                        services.AddScoped(serviceType, type);
                }

                // Singleton
                if (interfaces.Contains(typeof(ISingletonDependency)))
                {
                    var serviceInterfaces = interfaces.Where(i => i != typeof(ISingletonDependency));
                    foreach (var serviceType in serviceInterfaces)
                        services.AddSingleton(serviceType, type);
                }
            }

            return services;
        }
    }
}
