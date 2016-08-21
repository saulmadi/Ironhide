using System;
using System.Configuration;

namespace Ironhide.Users.Data
{
    public static class UserModule
    {
        public class Database
        {
            public static string ConnectionString {
                get
                {
                    string connStrEnvVarName = ConfigurationManager.AppSettings["UserModuleConnectionStringName"] ?? "Ironhide_users_connection_string";
                    string connectionString = Environment.GetEnvironmentVariable(connStrEnvVarName);

                    return connectionString;
                }
            }
        }
    }
}