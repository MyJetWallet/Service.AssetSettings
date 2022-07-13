using MyNoSqlServer.Abstractions;
using Service.AssetSettings.Domain.Models;

namespace Service.AssetSettings.MyNoSql
{
    public class AssetSettingsNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-asset-settings-transfers";

        public static string GeneratePartitionKey(string brokerId, string profile) => $"broker:{brokerId}|profile:{profile}";

        public static string GenerateRowKey(string asset, string assetNetwork) =>
            $"{asset}:{assetNetwork}";

        public static AssetSettingsNoSqlEntity Create(AssetSetting settings)
        {
            return new AssetSettingsNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(settings.BrokerId, settings.ProfileId),
                RowKey = GenerateRowKey(settings.AssetId, settings.AssetNetwork),
                AssetSeting = settings,
            };
        }

        public AssetSetting AssetSeting { get; private set; }
    }
}