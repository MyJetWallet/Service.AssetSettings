using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.AssetSettings.Client;
using Service.AssetSettings.Domain.Models;
using Service.AssetSettings.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new AssetSettingsClientFactory("http://localhost:5001");
            var client = factory.GetProfileService();
            var service = factory.GetSettingsService();
            var x = await client.GetAllProfiles();
            await service.UpdateAssetSettings(new AssetSetting()
            {
                AssetId = "TRX",
                AssetNetwork = "fireblocks-tron-test",
                BrokerId = "hetwallet",
                MinDeposit = 0.001m,
                ProfileId = "DEFAULT"
            });
            var all = await service.GetAssetSettingsList();
            //var resp = await  client.SayHelloAsync(new HelloRequest(){Name = "Alex"});
            //Console.WriteLine(resp?.Message);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
