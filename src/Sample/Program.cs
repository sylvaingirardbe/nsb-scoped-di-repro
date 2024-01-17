using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample.Behaviors;

namespace Sample;

static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        Console.WriteLine("Press any key to shutdown");
        Console.ReadKey();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        #region ContainerConfiguration
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<MyService>();
            })
            .UseNServiceBus(c =>
            {
                var endpointConfiguration = new EndpointConfiguration("Sample.Core");
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                endpointConfiguration.UseTransport<LearningTransport>();
                
                var pipeline = endpointConfiguration.Pipeline;
                pipeline.Register(
                    behavior: new IncomingTerminalBehavior(),
                    description: "Sets the incoming terminal header on the terminalcontext"
                );
                pipeline.Register(
                    behavior: new OutgoingTerminalBehavior(),
                    description: "Sets the outgoing terminal header on the message"
                );
                
                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    #endregion
}