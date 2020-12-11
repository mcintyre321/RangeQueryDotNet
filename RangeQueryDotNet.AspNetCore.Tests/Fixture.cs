using System;
using System.Net.Http;
using RangeQueryDotNet.Example;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace RangeQueryDotNet.AspNetCore.Tests
{
    class Fixture:IDisposable
    {
        public Fixture()
        {
            var hostBuilder = Program
                .CreateHostBuilder(new[] {"--environment", "Test"})
                .ConfigureWebHost(o =>
                {
                    o.UseTestServer();
                    o.ConfigureTestServices((s) =>
                    {
                         
                    });
                });
            
            this.Host = hostBuilder.Start();
            this.HttpClient = Host.GetTestClient();

        }

        public HttpClient HttpClient { get; set; }

        public IHost Host { get; set; }

        public void Dispose()
        {
            Host.Dispose();
            HttpClient.Dispose();
        }
    }
}