using System;
using Ubiety.Xmpp.Core;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.App
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var client = new XmppBuilder()
                .BuildClient()
                .EnableLogging(new SerilogManager())
                .Build();

            var id = new Jid("dieter@dieterlunn.ca");

            client.Connect(id);
            Console.ReadLine();
        }
    }
}
