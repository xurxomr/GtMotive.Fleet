using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.InfrastructureTests.Infrastructure
{
    public sealed class GenericInfrastructureTestServerFixture : IDisposable
    {
        private readonly IHost _host;

        public GenericInfrastructureTestServerFixture()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseEnvironment("IntegrationTest")
                        .UseTestServer()
                        .ConfigureAppConfiguration((context, builder) => builder.AddEnvironmentVariables())
                        .UseStartup<Startup>();
                })
                .Build();

            _host.Start();
            Server = _host.GetTestServer();
        }

        public TestServer Server { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            _host?.Dispose();
        }
    }
}
