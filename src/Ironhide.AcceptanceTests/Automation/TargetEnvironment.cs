using System.Configuration;

namespace Starscream.AcceptanceTests.Automation
{
    public static class TargetEnvironment
    {
        static string _currentEnvironment = "development";

        public static string GetApplicationUrl(string route)
        {
            string baseUrl = ConfigurationManager.AppSettings[_currentEnvironment];
            return baseUrl + route;
        }

        public static void Set(string environmentName)
        {
            _currentEnvironment = environmentName;
        }
    }
}