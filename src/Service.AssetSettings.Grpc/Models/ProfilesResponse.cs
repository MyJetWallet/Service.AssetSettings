using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.AssetSettings.Domain.Models;

namespace Service.AssetSettings.Grpc.Models
{
    [DataContract]
    public class ProfilesResponse
    {
        [DataMember(Order = 1)] public List<string> AssetSettingsProfiles { get; set; }
    }
}