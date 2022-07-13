using System.ServiceModel;
using System.Threading.Tasks;
using Service.AssetSettings.Grpc.Models;

namespace Service.AssetSettings.Grpc
{
    [ServiceContract]
    public interface IAssetSettingsProfileService
    {
        [OperationContract] public Task<ProfilesResponse> GetAllProfiles();

        [OperationContract] public Task<OperationResponse> CreateProfile(CreateProfileRequest request);

        [OperationContract] public Task<OperationResponse> DeleteProfile(DeleteProfileRequest request);
    }
}