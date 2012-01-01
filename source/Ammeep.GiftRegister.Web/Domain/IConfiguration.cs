using System.Configuration;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IConfiguration
    {
        string GiftmeConnectionString { get; }
    }

    public class Configuration : IConfiguration
    {      
        private static string GetApplicationConfigurationValue(string key)
        {
            return key != null ? ConfigurationManager.AppSettings.Get(key) : string.Empty;
        }

        private static string GetConnectionStringValue(string key)
        {
            if (key != null)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[key];
                return connectionString !=null  ? connectionString.ConnectionString : string.Empty;
            }
            return string.Empty;
        }

        public string GiftmeConnectionString
        {
            get { return GetConnectionStringValue("giftmedataconnectionstring"); }
        }
    }


}