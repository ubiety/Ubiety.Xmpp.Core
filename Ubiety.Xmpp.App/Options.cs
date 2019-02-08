using CommandLine;

namespace Ubiety.Xmpp.App
{
    public class Options
    {
        [Option('j', "jid", Required = true, HelpText = "JID to connect to")]
        public string Jid { get; set; }

        [Option('p', "password", Required = true, HelpText = "Password for the account")]
        public string Password { get; set; }
    }
}
