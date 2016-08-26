using System;
using Ironhide.App.Api.Modules.Home;
using Topshelf;

namespace Ironhide.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Type t1 = typeof(AppModule);

            HostFactory.Run(
                x =>
                {
                    x.Service<IronhideHost>(
                        s =>
                        {
                            s.ConstructUsing(name => new IronhideHost());

                            s.WhenStarted(
                                tc => tc.Start("http://localhost:9000"));

                            s.WhenStopped(
                                tc => tc.Stop());
                        });
                    x.RunAsLocalSystem();

                    x.SetDisplayName("Ironhide API Server");
                    x.SetDescription("Ironhide REST API Server");
                    x.SetServiceName("IronhideAPI");
                });
        }
    }
}