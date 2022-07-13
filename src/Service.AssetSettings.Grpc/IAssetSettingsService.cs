using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Service.AssetSettings.Domain.Models;
using Service.AssetSettings.Grpc.Models;

namespace Service.AssetSettings.Grpc
{
    [ServiceContract]
    public interface IAssetSettingsService
    {
        [OperationContract]
        Task<IReadOnlyCollection<AssetSetting>> GetAssetSettingsList();

        [OperationContract]
        Task AddAssetSettings(AssetSetting settings);

        [OperationContract]
        Task UpdateAssetSettings(AssetSetting settings);

        [OperationContract]
        Task RemoveAssetSettings(RemoveAssetSettingsRequest request);
    }
}