using System;
using System.Data.Entity;
using System.Linq;
using Ironhide.App.Data;

namespace DatabaseDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ironhide Database Deployer");
            Console.WriteLine("--------------------------");

            string connectionString = AppModule.Database.ConnectionString;

            Console.WriteLine("Using '{0}'...", connectionString);
            Console.WriteLine("");

            if (args.Contains("rebuild"))
            {
                Console.WriteLine("Rebuilding...");
                Database.SetInitializer(new DatabaseRebuilder());
                new AppDataContext(connectionString).Database.Initialize(true);
            }
            else
            {
                Console.WriteLine("Updating...");
                Database.SetInitializer(new DatabaseUpdater());
                new AppDataContext(connectionString).Database.Initialize(true);
            }

            Console.WriteLine("Done.");
        }
    }
}