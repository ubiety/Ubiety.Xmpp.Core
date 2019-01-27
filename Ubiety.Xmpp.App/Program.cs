using System;
using Ubiety.Xmpp.Core;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.App
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var client = XmppBuilder.BeginClientBuild()
                .EnableLogging(new SerilogManager())
                .Build();

            client.Error += Client_Error;

            client.Connect("dieter@dieterlunn.ca", "8o0tcAmp");
            Console.ReadLine();
        }

        private static void Client_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.StreamError);
        }
    }
}
