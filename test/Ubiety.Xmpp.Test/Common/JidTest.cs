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

        [Theory]
        [InlineData("d'artangan@garcon.fr/testing", @"d\27artangan@garcon.fr/testing")]
        [InlineData("me@you@testing.com", @"me\40you@testing.com")]
        [InlineData(@"me\40you@testing.com", "me@you@testing.com")]
        public void Parse_UsernameWithCharacter_ShouldEscape(string input, string output)
        {
            var jid = Jid.Parse(input);
            jid.Id.Should().Be(output);
        }

        [Fact]
        public void EscapeUsernameWithAt()
        {
            var jid = new Jid("me@you", "testing.com");
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
