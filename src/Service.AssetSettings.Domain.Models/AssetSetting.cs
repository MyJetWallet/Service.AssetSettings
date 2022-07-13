using System;
using System.Runtime.Serialization;

namespace Service.AssetSettings.Domain.Models
{
    [DataContract]
    public class AssetSetting
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string AssetId { get; set; }
        [DataMember(Order = 3)] public string ProfileId { get; set; }
        [DataMember(Order = 4)] public string AssetNetwork { get; set; }
        [DataMember(Order = 5)] public decimal MinDeposit { get; set; }
    }
}
