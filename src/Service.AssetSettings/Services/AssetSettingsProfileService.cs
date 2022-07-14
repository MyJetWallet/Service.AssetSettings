using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using Service.AssetSettings.Grpc;
using Service.AssetSettings.Grpc.Models;
using Service.AssetSettings.MyNoSql;

namespace Service.AssetSettings.Services
{
    public class AssetSettingsProfileService : IAssetSettingsProfileService
    {
        private readonly IMyNoSqlServerDataWriter<AssetSettingsNoSqlEntity> _assetWriter;
        private readonly IMyNoSqlServerDataWriter<ProfilesNoSqlEntity> _profileWriter;

        public AssetSettingsProfileService(IMyNoSqlServerDataWriter<AssetSettingsNoSqlEntity> assetWriter, IMyNoSqlServerDataWriter<ProfilesNoSqlEntity> profileWriter)
        {
            _assetWriter = assetWriter;
            _profileWriter = profileWriter;
        }

        public async Task<ProfilesResponse> GetAllProfiles()
        {
            var groups = await _profileWriter.GetAsync(ProfilesNoSqlEntity.GeneratePartitionKey(), ProfilesNoSqlEntity.GenerateRowKey());

            if (groups == null || groups.Profiles == null || !groups.Profiles.Any())
            {
                await _profileWriter.InsertOrReplaceAsync(ProfilesNoSqlEntity.Create(new List<string>() { "DEFAULT" }));
                groups = await _profileWriter.GetAsync(ProfilesNoSqlEntity.GeneratePartitionKey(), ProfilesNoSqlEntity.GenerateRowKey());
            }

            return new ProfilesResponse()
            {
                AssetSettingsProfiles = groups?.Profiles ?? new List<string>(),
            };
        }

        public async Task<OperationResponse> CreateProfile(CreateProfileRequest request)
        {
            try
            {
                var groups = await _profileWriter.GetAsync(ProfilesNoSqlEntity.GeneratePartitionKey(),
                    ProfilesNoSqlEntity.GenerateRowKey());
                var withdrawalList = groups?.Profiles ?? new List<string>();
                withdrawalList.Add(request.ProfileId);
                await _profileWriter.InsertOrReplaceAsync(ProfilesNoSqlEntity.Create(withdrawalList.Distinct().ToList()));

                if (!string.IsNullOrWhiteSpace(request.CloneFromProfileId))
                {
                    var assets =
                        (await _assetWriter.GetAsync(
                            AssetSettingsNoSqlEntity.GeneratePartitionKey(request.BrokerId, request.CloneFromProfileId)))
                        .Select(t => t.AssetSeting).ToList();

                    foreach (var asset in assets)
                    {
                        asset.ProfileId = request.ProfileId;
                    }

                    await _assetWriter.BulkInsertOrReplaceAsync(assets.Select(AssetSettingsNoSqlEntity.Create).ToList());
                }

                return new OperationResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new OperationResponse()
                {
                    IsSuccess = false,
                    ErrorText = e.Message
                };
            }
        }

        public async Task<OperationResponse> DeleteProfile(DeleteProfileRequest request)
        {
            try
            {
                var groups = await _profileWriter.GetAsync(ProfilesNoSqlEntity.GeneratePartitionKey(),
                    ProfilesNoSqlEntity.GenerateRowKey());
                var withdrawalList = groups?.Profiles ?? new List<string>();

                var list = withdrawalList.Distinct().ToList();
                list.Remove(request.ProfileId);
                await _profileWriter.InsertOrReplaceAsync(ProfilesNoSqlEntity.Create(list));

                var assets =
                    (await _assetWriter.GetAsync(
                        AssetSettingsNoSqlEntity.GeneratePartitionKey(request.BrokerId, request.ProfileId))).ToList();

                foreach (var asset in assets)
                {
                    await _assetWriter.DeleteAsync(asset.PartitionKey, asset.RowKey);
                }

                return new OperationResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new OperationResponse()
                {
                    IsSuccess = false,
                    ErrorText = e.Message
                };
            }
        }
    }
}