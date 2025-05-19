using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CommonLibrary.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration, Action<ConsulClientConfiguration> consulClientConfiguration = null)
        {
            var address = configuration["ConsulConfig:Address"];
            var consulClient = new ConsulClientConfiguration { Address = new Uri(address) };
            if (consulClientConfiguration is not null) consulClientConfiguration(consulClient);
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulClient));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, Action<AgentServiceRegistration> config, int defaultPort = 5000)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            // Get server IP
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.FirstOrDefault() ?? $"http://localhost:{defaultPort}";

            // Register service with consul
            var uri = new Uri(address);

            var currentAssembly = Assembly.GetExecutingAssembly().FullName.ToLower();
            var registration = new AgentServiceRegistration()
            {
                ID = $"{currentAssembly}-" + Guid.NewGuid(),
                Name = currentAssembly,
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = [currentAssembly],
                Check = new AgentServiceCheck
                {
                    HTTP = $"{uri.Scheme}://host.docker.internal:{uri.Port}/health",
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
                }
            };

            config(registration);

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }

    }
}
