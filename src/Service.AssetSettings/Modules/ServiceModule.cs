using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.AssetSettings.MyNoSql;
using System;

namespace Service.AssetSettings.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

        }
    }

    public class MyNoSqlModule : Module
    {
        private readonly Func<string> _myNoSqlServerWriterUrl;

        public MyNoSqlModule(Func<string> myNoSqlServerWriterUrl)
        {
            _myNoSqlServerWriterUrl = myNoSqlServerWriterUrl;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMyNoSqlWriter<AssetSettingsNoSqlEntity>(_myNoSqlServerWriterUrl, AssetSettingsNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<ProfilesNoSqlEntity>(_myNoSqlServerWriterUrl, ProfilesNoSqlEntity.TableName);

        }
    }
}