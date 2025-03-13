using FluentAssertions;
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Test.Sasl
{
    public class ScramProcessorTest
    {
        public void InitializeShouldReturnAuthTag()
        {
            var scram = new ScramProcessor();
            var tag = scram.Initialize("test@test.com", "peanut");

            tag.Should().BeOfType<Auth>("SCRAM processor was just initialized");
        }
    }
}
