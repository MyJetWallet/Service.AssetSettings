using System.Collections.Generic;
using MyNoSqlServer.Abstractions;

namespace Service.AssetSettings.MyNoSql
{
    public class ProfilesNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-asset-settings-profiles";

        public static string GeneratePartitionKey() => "Profiles";

        public static string GenerateRowKey() => "Profiles";

        public static ProfilesNoSqlEntity Create(List<string> profiles)
        {
            return new ProfilesNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
                Profiles = profiles,
            };
        }

        public List<string> Profiles { get; set; }

    }
}