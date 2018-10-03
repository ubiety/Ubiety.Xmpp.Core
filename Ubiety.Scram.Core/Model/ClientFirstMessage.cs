namespace Ubiety.Scram.Core.Model
{
  public class ClientFirstMessage
  {
    public string Gs2Header { get; } = "n,,";
    public UserAttribute Username { get; }
    public NonceAttribute Nonce { get; }

    public string BareMessage => $"{Username},{Nonce}";

    public string Message => $"{Gs2Header},{BareMessage}";

    public ClientFirstMessage(string username, string nonce)
    {
      Username = new UserAttribute(username);
      Nonce = new NonceAttribute(nonce);
    }
  }
}
