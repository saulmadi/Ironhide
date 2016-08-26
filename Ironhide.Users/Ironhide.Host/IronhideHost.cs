using System;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Infrastructure.Configuration;
using Nancy.Hosting.Self;

namespace Ironhide.Host
{
    public class IronhideHost
    {
        NancyHost _nancyHost;

        public bool Start(string hostUrl)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine("");
            _nancyHost = new NancyHost(new Uri(hostUrl), new Bootstrapper(), new HostConfiguration
                                                                             {
                                                                                 UrlReservations = new UrlReservations
                                                                                                   {
                                                                                                       CreateAutomatically
                                                                                                           = true
                                                                                                   }
                                                                             });

            _nancyHost.Start();
            Console.WriteLine(StartupMessages.GetStartupMessageWithModules());
            return true;
        }

        public bool Stop()
        {
            Console.WriteLine("Stopping...");
            _nancyHost.Stop();
            _nancyHost.Dispose();
            return true;
        }
    }
}