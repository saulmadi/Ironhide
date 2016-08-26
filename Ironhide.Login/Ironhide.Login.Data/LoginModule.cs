using System;
using System.Configuration;

namespace Ironhide.Login.Data
{
    public static class LoginModule
    {
        public class Database
        {
            public static string ConnectionString
            {
                get
                {
                    string connStrEnvVarName = ConfigurationManager.AppSettings["UserModuleConnectionStringName"] ??
                                               "Ironhide_users_connection_string";
                    string connectionString = Environment.GetEnvironmentVariable(connStrEnvVarName);

                    return connectionString;
                }
            }
        }
    }
}