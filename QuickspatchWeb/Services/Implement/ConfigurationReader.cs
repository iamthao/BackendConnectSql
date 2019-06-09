using System.Configuration;
using QuickspatchWeb.Services.Interface;

namespace QuickspatchWeb.Services.Implement
{
    public class ConfigurationReader : IConfigurationReader
    {
        private AppSettingsReader _reader = new AppSettingsReader();

        public string GetValue(string key)
        {
            return _reader.GetValue(key, typeof(string)) as string;
        }
    }
}