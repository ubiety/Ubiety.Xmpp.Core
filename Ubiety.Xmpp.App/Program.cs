using CommandLine;
using System;
using Ubiety.Xmpp.Core;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.App
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                using (var client = XmppBuilder.BeginClientBuild()
                    .EnableLogging(new SerilogManager())
                    .Build())
                {

                    client.Error += Client_Error;

                    client.Connect(o.Jid, o.Password);

                    // Needed to keep app from exiting too early
                    // Console apps don't have an inherent run loop
                    Console.ReadLine();
                }
            });
        }

        private static void Client_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.StreamError);
        }
    }
}
