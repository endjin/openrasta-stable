namespace OpenRasta.Testing.Hosting.Iis7.WorkerProcess
{
    using System;

    using OpenRasta.Testing.Hosting.Iis7;

    public static class WorkerProcess32
    {
        public static Iis7Server Server { get; set; }

        static void Main(string[] args)
        {
            // Debugger.Launch();
            Server = Iis7Starter.Start(args);
            Server.Start();

            Console.WriteLine("Ready");
            Console.ReadLine();
        }
    }
}