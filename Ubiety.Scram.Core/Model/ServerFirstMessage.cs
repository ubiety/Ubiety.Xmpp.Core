using System;
using System.Linq;

namespace Ubiety.Scram.Core.Model
{
  public class ServerFirstMessage
  {
    public static ServerFirstMessage ParseResponse(string response)
    {
      var parts = ScramAttribute.ParseAll(response.Split(','));

      var errors = parts.OfType<ErrorAttribute>();
      if (errors.Any())
        throw new InvalidOperationException();

      var iterations = parts.OfType<IterationsAttribute>().ToList();
      var nonces = parts.OfType<NonceAttribute>().ToList();
      var salts = parts.OfType<SaltAttribute>().ToList();

      if (!iterations.Any() || !nonces.Any() || !salts.Any())
        throw new InvalidOperationException();

      return new ServerFirstMessage(iterations.First(), nonces.First(), salts.First());
    }

    public ScramAttribute<int> Iterations { get; }
    public ScramAttribute<string> Nonce { get; }
    public ScramAttribute<byte[]> Salt { get; }

    private ServerFirstMessage(IterationsAttribute iterations, NonceAttribute nonce, SaltAttribute salt)
    {
      Iterations = iterations;
      Nonce = nonce;
      Salt = salt;
    }

    public ServerFirstMessage(int iterations, string nonce, byte[] salt)
    {
      Iterations = new IterationsAttribute(iterations);
      Nonce = new NonceAttribute(nonce);
      Salt = new SaltAttribute(salt);
    }
  }
}
