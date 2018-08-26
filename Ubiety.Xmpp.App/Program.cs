using System;
using Ubiety.Xmpp.Core;

namespace Ubiety.Xmpp.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new XmppClientBuilder()
                .Begin()
                .EnableLogging(new SerilogManager())
                .Build();
            Console.ReadLine();
        }
    }
}
