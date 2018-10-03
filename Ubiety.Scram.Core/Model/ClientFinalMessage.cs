namespace Ubiety.Scram.Core.Model
{
  public class ClientFinalMessage
  {
    public ChannelAttribute Channel { get; }
    public NonceAttribute Nonce { get; }
    public ClientProofAttribute Proof { get; private set; }

    public string MessageWithoutProof => $"{Channel},{Nonce}";
    public string Message => $"{MessageWithoutProof},{Proof}";

    public ClientFinalMessage(ClientFirstMessage clientFirstMessage, ServerFirstMessage serverFirstMessage)
    {
      Channel = new ChannelAttribute(clientFirstMessage.Gs2Header);
      Nonce = new NonceAttribute(clientFirstMessage.Nonce.Value, serverFirstMessage.Nonce.Value);
    }

    public void SetProof(byte[] proof)
    {
      Proof = new ClientProofAttribute(proof);
    }
  }
}
