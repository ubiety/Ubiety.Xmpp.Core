using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.Tags.Sasl;
using Xunit;

namespace Ubiety.Xmpp.Test.Scram
{
    public class ScramProcessorTest
    {
        [Fact]
        public void InitializeShouldReturnAuthTag()
        {
            var scram = new ScramProcessor(false);
            var tag = scram.Initialize("test@test.com", "peanut");

            Assert.IsType<Auth>(tag);
        }
    }
}
