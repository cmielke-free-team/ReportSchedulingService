using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Emdat.Diagnostics;
using System.Diagnostics;

namespace InVision.SchedulingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(params string[] args)
        {
            //initialize a custom Logger (using a TraceSource from the app.config file)
            Logger.TraceSource = new TraceSource("InVision.SchedulingService");

            if (args.Length > 0 && args[0] == "/console")
            {
                //for debugging, just run the service as a console application
                //in the project properties, make sure to change the output type to Console Application            
                SchedulingService app = new SchedulingService();
                try
                {
                    app.StartService(null);
                    Console.ReadLine();
                }
                finally
                {
                    app.StopService();
                }
            }
            else
            {
                // More than one user Service may run within the same process. To add
                // another service to this process, change the following line to
                // create a second service object. For example,
                //
                //   ServicesToRun = new ServiceBase[] {new SchedulingService(), new MySecondUserService()};
                //
                ServiceBase[] ServicesToRun = new ServiceBase[] 
                { 
                    new SchedulingService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
