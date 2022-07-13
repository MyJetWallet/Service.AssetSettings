using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.AssetSettings.Grpc;

namespace Service.AssetSettings.Client
{
    [UsedImplicitly]
    public class AssetSettingsClientFactory: MyGrpcClientFactory
    {
        public AssetSettingsClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IAssetSettingsProfileService GetProfileService() => CreateGrpcService<IAssetSettingsProfileService>();

        public IAssetSettingsService GetSettingsService() => CreateGrpcService<IAssetSettingsService>();
    }
}
