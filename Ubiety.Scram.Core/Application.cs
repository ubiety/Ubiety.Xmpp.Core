using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Scram.Core;

namespace Scram
{
  public abstract class Application
  {
    private const int NonceLength = 16;
    private const int BufferLength = 2048;

    private static readonly RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();

    protected Hash Hash { get; }

    private readonly Stream _stream;

    protected Application(Stream stream, Hash hash)
    {
      _stream = stream;
      Hash = hash;
    }
    
    protected string CreateNonce()
    {
      var bytes = new byte[NonceLength];
      RandomNumberGenerator.GetBytes(bytes);
      return Convert.ToBase64String(bytes);
    }

    protected void Send(string content)
    {
      var bytes = Encoding.UTF8.GetBytes(content);
      _stream.Write(bytes, 0, bytes.Length);
    }

    protected string Receive()
    {
      var timeout = _stream.ReadTimeout;

      var outputBuffer = new List<byte>();
      var buffer = new byte[BufferLength];

      while (true)
      {
        try
        {
          var bytesRead = _stream.Read(buffer, 0, buffer.Length);
          outputBuffer.AddRange(buffer.Take(bytesRead));

          if (bytesRead == BufferLength)
          {
            _stream.ReadTimeout = 10;
          }
        }
        catch (IOException ex)
        {
          if (ex.InnerException == null || !(ex.InnerException is SocketException))
            throw;

          var socketEx = (SocketException)ex.InnerException;
          if (socketEx.SocketErrorCode != SocketError.TimedOut)
            throw;

          break;
        }
      }

      _stream.ReadTimeout = timeout;
      return Encoding.UTF8.GetString(outputBuffer.ToArray());
    }
  }
}
