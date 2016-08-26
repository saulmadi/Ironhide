using System;
using System.Configuration;

namespace Ironhide.App.Data
{
    public static class AppModule
    {
        public class Database
        {
            public static string ConnectionString
            {
                get
                {
                    string connStrEnvVarName = ConfigurationManager.AppSettings["AppModuleConnectionStringName"] ??
                                               "Ironhide_app_connection_string";
                    string connectionString = Environment.GetEnvironmentVariable(connStrEnvVarName);

                    return connectionString;
                }
            }
        }
    }
}