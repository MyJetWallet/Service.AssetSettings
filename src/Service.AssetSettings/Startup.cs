using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using MyJetWallet.Sdk.GrpcMetrics;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using Prometheus;
using ProtoBuf.Grpc.Server;
using Service.AssetSettings.Grpc;
using Service.AssetSettings.Modules;
using Service.AssetSettings.Services;
using SimpleTrading.BaseMetrics;
using SimpleTrading.ServiceStatusReporterConnector;

namespace Service.AssetSettings
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureJetWallet<ApplicationLifetimeManager>(Program.Settings.ZipkinUrl);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureJetWallet(env, endpoints =>
            {
                endpoints.MapGrpcSchema<AssetSettingsProfileService, IAssetSettingsProfileService>();
                endpoints.MapGrpcSchema<AssetSettingsService, IAssetSettingsService>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureJetWallet();
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule(new MyNoSqlModule(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl)));
        }
    }
}
