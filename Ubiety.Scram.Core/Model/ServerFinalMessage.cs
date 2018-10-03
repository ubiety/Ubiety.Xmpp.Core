using System;
using System.Linq;

namespace Ubiety.Scram.Core.Model
{
  internal class ServerFinalMessage
  {
    public static ServerFinalMessage ParseResponse(string response)
    {
      var parts = ScramAttribute.ParseAll(response.Split(','));

      var error = parts.OfType<ErrorAttribute>().ToList();
      if (error.Any()) 
        throw new InvalidOperationException();

      var signature = parts.OfType<ServerSignatureAttribute>().ToList();
      if (!signature.Any())
        throw new InvalidOperationException();
      
      return new ServerFinalMessage(signature.First());
    }

    public ServerSignatureAttribute ServerSignature { get; }

    public ServerFinalMessage(ServerSignatureAttribute serverSignature)
    {
      ServerSignature = serverSignature;
    }
  }
}
