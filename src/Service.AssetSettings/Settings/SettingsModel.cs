using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.AssetSettings.Settings
{
    public class SettingsModel
    {
        [YamlProperty("AssetSettings.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("AssetSettings.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("AssetSettings.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("AssetSettings.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; internal set; }
    }
}
