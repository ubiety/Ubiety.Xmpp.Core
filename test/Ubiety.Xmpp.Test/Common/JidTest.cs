using FluentAssertions;
using Ubiety.Xmpp.Core.Common;
using Xunit;

namespace Ubiety.Xmpp.Test.Common
{
    public class JidTest
    {
        [Fact]
        public void CreateWithConstructor()
        {
            var jid = new Jid("test", "testing.com", "tester");
            jid.Resource.Should().Be("tester");
            jid.User.Should().Be("test");
            jid.Server.Should().Be("testing.com");
        }

        [Fact]
        public void CreateWithParse()
        {
            var jid = Jid.Parse("test@test.com/tester");
            jid.User.Should().Be("test");
            jid.Server.Should().Be("test.com");
            jid.Resource.Should().Be("tester");
        }

        [Fact]
        public void EscapeUsername()
        {
            var jid = Jid.Parse("d'artangan@garcon.fr/testing");
            jid.Id.Should().Be(@"d\27artangan@garcon.fr/testing");
        }
    }
}
