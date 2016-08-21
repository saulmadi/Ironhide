using System;
using System.Data.Entity;
using System.Linq;
using Ironhide.Users.Data;

namespace DatabaseDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ironhide Database Deployer");
            Console.WriteLine("--------------------------");

            var connectionString = UserModule.Database.ConnectionString;
            
            Console.WriteLine("Using '{0}'...", connectionString);
            Console.WriteLine("");
            
            if (args.Contains("rebuild"))
            {
                Console.WriteLine("Rebuilding...");
                Database.SetInitializer(new DatabaseRebuilder());
                new UserDataContext(connectionString).Database.Initialize(true);            
            }
            else 
            {
                Console.WriteLine("Updating...");
                Database.SetInitializer(new DatabaseUpdater());
                new UserDataContext(connectionString).Database.Initialize(true);            
            }
            
            Console.WriteLine("Done.");
        }
    }
}