using System.Runtime.Serialization;

namespace Service.AssetSettings.Grpc.Models
{
    [DataContract]
    public class OperationResponse
    {
        [DataMember(Order = 1)] public bool IsSuccess { get; set; }

        [DataMember(Order = 2)] public string ErrorText { get; set; }
    }
}