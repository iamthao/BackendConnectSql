namespace QuickspatchWeb.Services.Interface
{
    public interface IConfigurationReader
    {
        string GetValue(string key);
    }
}