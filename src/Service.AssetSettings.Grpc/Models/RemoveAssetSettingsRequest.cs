using System.Runtime.Serialization;

namespace Service.AssetSettings.Grpc.Models
{
    [DataContract]
    public class RemoveAssetSettingsRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string AssetId { get; set; }
        [DataMember(Order = 4)] public string ProfileId { get; set; }
        [DataMember(Order = 5)] public string AssetNetwork { get; set; }

    }
}