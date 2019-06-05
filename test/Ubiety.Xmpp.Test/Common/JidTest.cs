using FluentAssertions;
using Ubiety.Xmpp.Core.Common;
using Xunit;

namespace Ubiety.Xmpp.Test.Common
{
    public class JidTest
    {
        [Fact]
        public void Constructor_WithAllParams_ReturnsNewInstance()
        {
            var jid = new Jid("test", "testing.com", "tester");
            jid.Resource.Should().Be("tester");
            jid.User.Should().Be("test");
            jid.Server.Should().Be("testing.com");
        }

        [Fact]
        public void Parse_WithString_ReturnsJidInstance()
        {
            var jid = Jid.Parse("test@test.com/tester");
            jid.User.Should().Be("test");
            jid.Server.Should().Be("test.com");
            jid.Resource.Should().Be("tester");
        }

        [Fact]
        public void Parse_FromImplicitOperator_ReturnsJidInstance()
        {
            Jid jid = "test@test.com/testing";
            jid.User.Should().Be("test");
            jid.Server.Should().Be("test.com");
            jid.Resource.Should().Be("testing");
            jid.Should().Be("test@test.com/testing");
        }

        [Fact]
        public void EscapeUsernameWithApostrophe()
        {
            var jid = Jid.Parse("d'artangan@garcon.fr/testing");
            jid.Id.Should().Be(@"d\27artangan@garcon.fr/testing");
            jid.User.Should().Be(@"d'artangan");
        }

        [Fact]
        public void EscapeUsernameWithAt()
        {
            var jid = new Jid("me@you", "testing.com");
            jid.Id.Should().Be(@"me\40you@testing.com");
        }

        [Fact]
        public void EscapeUsernameWithAtFromParse()
        {
            var jid = Jid.Parse("me@you@testing.com");
            jid.Id.Should().Be(@"me\40you@testing.com");
        }

        [Fact]
        public void EqualsWithDifferentObjects()
        {
            var left = Jid.Parse("test@test.com");
            var right = new Jid("test", "test.com");

            left.Should().Be(right);
        }

        [Fact]
        public void EqualsTheSameObject()
        {
            var jid = Jid.Parse("test@test.com");
            var clone = jid;

            jid.Should().BeSameAs(clone);
        }
    }
}
