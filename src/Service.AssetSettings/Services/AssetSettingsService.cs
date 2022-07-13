using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;
using Service.AssetSettings.Domain.Models;
using Service.AssetSettings.Grpc;
using Service.AssetSettings.Grpc.Models;
using Service.AssetSettings.MyNoSql;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.AssetSettings.Services
{
    public class AssetSettingsService : IAssetSettingsService
    {
        private readonly ILogger<AssetSettingsService> _logger;
        private readonly IMyNoSqlServerDataWriter<AssetSettingsNoSqlEntity> _writer;

        public AssetSettingsService(ILogger<AssetSettingsService> logger,
            IMyNoSqlServerDataWriter<AssetSettingsNoSqlEntity> writer)
        {
            _logger = logger;
            _writer = writer;
        }

        public async Task<IReadOnlyCollection<AssetSetting>> GetAssetSettingsList()
        {
            var entities = await _writer.GetAsync();
            return entities.Select(e => e.AssetSeting).ToList();
        }

        public async Task AddAssetSettings(AssetSetting settings)
        {
            using var action = MyTelemetry.StartActivity("Add Asset Fees Settings");
            settings.AddToActivityAsJsonTag("settings");
            try
            {
                _logger.LogInformation("Add Asset Fees Setting: {jsonText}",
                    JsonConvert.SerializeObject(settings));

                ValidateSettings(settings);

                var entity = AssetSettingsNoSqlEntity.Create(settings);

                var existingItem = await _writer.GetAsync(entity.PartitionKey, entity.RowKey);
                if (existingItem != null) throw new Exception("Cannot add Asset Fees Settings. Already exist");

                await _writer.InsertAsync(entity);

                _logger.LogInformation("Added Asset Fees Setting: {jsonText}",
                    JsonConvert.SerializeObject(settings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot add ExternalMarketSettings: {requestJson}",
                    JsonConvert.SerializeObject(settings));
                ex.FailActivity();
                throw;
            }
        }

        public async Task UpdateAssetSettings(AssetSetting settings)
        {
            using var action = MyTelemetry.StartActivity("Update Asset Fees Settings");
            settings.AddToActivityAsJsonTag("settings");
            try
            {
                _logger.LogInformation("Update Asset Fees Setting: {jsonText}",
                    JsonConvert.SerializeObject(settings));

                ValidateSettings(settings);

                var entity = AssetSettingsNoSqlEntity.Create(settings);

                await _writer.InsertOrReplaceAsync(entity);

                _logger.LogInformation("Updated Asset Fees Setting: {jsonText}",
                    JsonConvert.SerializeObject(settings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot update ExternalMarketSettings: {requestJson}",
                    JsonConvert.SerializeObject(settings));
                ex.FailActivity();
                throw;
            }
        }

        public async Task RemoveAssetSettings(RemoveAssetSettingsRequest request)
        {
            using var action = MyTelemetry.StartActivity("Remove Asset Fees Settings");
            request.AddToActivityAsJsonTag("request");
            try
            {
                _logger.LogInformation("Remove Asset Fees Setting: {jsonText}",
                    JsonConvert.SerializeObject(request));

                var entity = await _writer.DeleteAsync(AssetSettingsNoSqlEntity.GeneratePartitionKey(request.BrokerId, request.ProfileId),
                    AssetSettingsNoSqlEntity.GenerateRowKey(request.AssetId, request.AssetNetwork));

                if (entity != null)
                    _logger.LogInformation("Removed Asset Fees Settings: {jsonText}",
                        JsonConvert.SerializeObject(entity));
                else
                    _logger.LogInformation("Unable to remove Asset Fees Setting, do not exist: {jsonText}",
                        JsonConvert.SerializeObject(request));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot remove ExternalMarketSettings: {requestJson}",
                    JsonConvert.SerializeObject(request));
                ex.FailActivity();
                throw;
            }
        }

        private static void ValidateSettings(AssetSetting settings)
        {
            if (string.IsNullOrEmpty(settings.BrokerId)) throw new Exception("Cannot add settings with empty broker");
            if (string.IsNullOrEmpty(settings.ProfileId)) throw new Exception("Cannot add settings with empty profile");
            if (string.IsNullOrEmpty(settings.AssetId)) throw new Exception("Cannot add settings with empty asset");
            if (string.IsNullOrEmpty(settings.AssetNetwork)) throw new Exception("Cannot add settings with empty network");

            if (settings.MinDeposit < 0) throw new Exception("Cannot add settings with negative min deposit size");
        }
    }
}