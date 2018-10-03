using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ubiety.Scram.Core
{
  public class ScramAttribute
  {
    protected const char AuthorizationIdentityName = 'a';
    protected const char UserName = 'n';
    protected const char MessageName = 'm';
    protected const char NonceName = 'r';
    protected const char ChannelName = 'c';
    protected const char SaltName = 's';
    protected const char IterationsName = 'i';
    protected const char ClientProofName = 'p';
    protected const char ServerSignatureName = 'v';
    protected const char ErrorName = 'e';

    public static ICollection<ScramAttribute> ParseAll(IEnumerable<string> attributes)
    {
      return attributes.Select(Parse).ToList();
    }

    public static ScramAttribute Parse(string attribute)
    {
      var parts = attribute.Split(new[] { '=' }, 2);
      if (parts.Length != 2) throw new FormatException();
      if (parts[0].Length > 1) throw new FormatException();

      switch (parts[0][0])
      {
        case AuthorizationIdentityName: return new AuthorizationIdentityAttribute(parts[1]);
        case UserName: return new UserAttribute(parts[1], true);
        case NonceName: return new NonceAttribute(parts[1]);
        case ChannelName: return new ChannelAttribute(parts[1]);
        case SaltName: return new SaltAttribute(parts[1]);
        case IterationsName: return new IterationsAttribute(parts[1]);
        case ClientProofName: return new ClientProofAttribute(parts[1]);
        case ServerSignatureName: return new ServerSignatureAttribute(parts[1]);
        case ErrorName: return new ErrorAttribute(parts[1]);
        default: return new UnknownAttribute(parts[0][0], parts[1]);
      }
    }

    public char Name { get; }

    public ScramAttribute(char name)
    {
      Name = name;
    }
  }

  public class ScramAttribute<TValue> : ScramAttribute
  {
    public TValue Value { get; }

    public ScramAttribute(char name, TValue value) : base(name)
    {
      Value = value;
    }

    public override string ToString()
    {
      return $"{Name}={Value}";
    }
  }

  internal class AuthorizationIdentityAttribute : ScramAttribute<string>
  {
    public AuthorizationIdentityAttribute(string value)
      : base(AuthorizationIdentityName, value)
    {

    }
  }

  public class UserAttribute : ScramAttribute<string>
  {
    private const string EqualReplacement = "=3D";
    private const string CommaReplacement = "=2C";

    private static string Replace(string value, bool doReplace)
    {
      if (!doReplace) return value;

      var lastIndex = -1;
      while ((lastIndex = value.IndexOf('=', lastIndex + 1)) > -1)
      {
        var escapeCheck = value.Substring(lastIndex, 3);
        switch (escapeCheck)
        {
          case EqualReplacement:
            value = Replace(value, lastIndex, '=', EqualReplacement.Length);
            break;
          case CommaReplacement:
            value = Replace(value, lastIndex, ',', CommaReplacement.Length);
            break;
          default:
            throw new FormatException();
        }
      }

      return value;
    }

    private static string Replace(string value, int index, char replacement, int len)
    {
      var temp1 = value.Substring(0, index);
      var temp2 = value.Substring(index + len, value.Length - index - len);
      return $"{temp1}{replacement}{temp2}";
    }

    public UserAttribute(string value, bool fromWire = false)
      : base(UserName, Replace(value, fromWire))
    {

    }

    public override string ToString()
    {
      var printableValue = Value.Replace("=", EqualReplacement).Replace(",", CommaReplacement);
      return $"{Name}={printableValue}";
    }
  }

  public class NonceAttribute : ScramAttribute<string>
  {
    public NonceAttribute(string value)
      : base(NonceName, value)
    {

    }

    public NonceAttribute(string clientNonce, string serverNonce)
      : base(NonceName, $"{clientNonce}{serverNonce}")
    {
      
    }
  }

  public class ChannelAttribute : ScramAttribute<string>
  {
    public ChannelAttribute(string value)
      : base(ChannelName, Convert.ToBase64String(Encoding.UTF8.GetBytes(value)))
    {

    }
  }

  internal class SaltAttribute : ScramAttribute<byte[]>
  {
    public SaltAttribute(byte[] value)
      : base(SaltName, value)
    {

    }

    public SaltAttribute(string value)
      : base(SaltName, Convert.FromBase64String(value))
    {

    }

    public override string ToString()
    {
      return $"{Name} = {Convert.ToBase64String(Value)}";
    }
  }

  internal class IterationsAttribute : ScramAttribute<int>
  {
    public IterationsAttribute(int value)
      : base(IterationsName, value)
    {

    }

    public IterationsAttribute(string value)
      : base(IterationsName, int.Parse(value))
    {

    }
  }

  public class ClientProofAttribute : ScramAttribute<byte[]>
  {
    public ClientProofAttribute(string value)
      : base(ClientProofName, Convert.FromBase64String(value))
    {

    }

    public ClientProofAttribute(byte[] value)
      : base(ClientProofName, value)
    {

    }

    public override string ToString()
    {
      return $"{Name}={Convert.ToBase64String(Value)}";
    }
  }

  internal class ServerSignatureAttribute : ScramAttribute<byte[]>
  {
    public ServerSignatureAttribute(byte[] value)
      : base(ServerSignatureName, value)
    {
      
    }

    public ServerSignatureAttribute(string value)
      : base(ServerSignatureName, Convert.FromBase64String(value))
    {

    }

    public bool Equals(byte[] other)
    {
      return Value.SequenceEqual(other);
    }
  }

  internal class ErrorAttribute : ScramAttribute<string>
  {
    public ErrorAttribute(string value)
      : base(ErrorName, value)
    {

    }
  }

  internal class UnknownAttribute : ScramAttribute<string>
  {
    public UnknownAttribute(char name, string value)
      : base(name, value)
    {

    }
  }
}
