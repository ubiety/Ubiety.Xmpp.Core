using System;
using System.IO;
using System.Text;
using Ubiety.Scram.Core;
using Ubiety.Scram.Core.Model;

namespace Scram
{
  public class Client : Application
  {
    private readonly string _username;
    private readonly string _password;
    private readonly string _nonce;

    public Client(Stream stream, Hash hash, string username, string password) : base(stream, hash)
    {
      _username = username;
      _password = SaslPrep.Run(password);

      _nonce = CreateNonce();
    }

    public void Authenticate()
    {
      var clientFirstMessage = new ClientFirstMessage(_username, _nonce);
      Send(clientFirstMessage.Message);
      
      var serverFirstMessage = ServerFirstMessage.ParseResponse(Receive());
      var hashedPassword = Hash.ComputeHash(Encoding.UTF8.GetBytes(_password), serverFirstMessage.Salt.Value,
        serverFirstMessage.Iterations.Value);
      var clientKey = Hash.ComputeHash(Encoding.UTF8.GetBytes("Client Key"), hashedPassword);
      var serverKey = Hash.ComputeHash(Encoding.UTF8.GetBytes("Server Key"), hashedPassword);
      var storedKey = Hash.ComputeHash(clientKey);

      var clientFinalMessage = new ClientFinalMessage(clientFirstMessage, serverFirstMessage);
      var authMessage = $"{clientFirstMessage.BareMessage},{serverFirstMessage},{clientFinalMessage.MessageWithoutProof}";
      var clientSignature = Hash.ComputeHash(Encoding.UTF8.GetBytes(authMessage), storedKey);
      var serverSignature = Hash.ComputeHash(Encoding.UTF8.GetBytes(authMessage), serverKey);
      var clientProof = clientKey.ExclusiveOr(clientSignature);
      clientFinalMessage.SetProof(clientProof);

      Send(clientFinalMessage.Message);

      var serverFinalMessage = ServerFinalMessage.ParseResponse(Receive());
      if (!serverFinalMessage.ServerSignature.Equals(serverSignature))
        throw new InvalidOperationException();
    }
  }
}
