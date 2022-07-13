using Autofac;
using Service.AssetSettings.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.AssetSettings.Client
{
    public static class AutofacHelper
    {
        public static void RegisterAssetSettingsClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new AssetSettingsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetProfileService()).As<IAssetSettingsProfileService>().SingleInstance();
            builder.RegisterInstance(factory.GetSettingsService()).As<IAssetSettingsService>().SingleInstance();
        }
    }
}
