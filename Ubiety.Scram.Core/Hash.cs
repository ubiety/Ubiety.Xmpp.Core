using System;
using System.Linq;
using System.Security.Cryptography;
using Scram;

namespace Ubiety.Scram.Core
{
  public class Hash
  {
    public static Hash Sha1()
    {
      return new Hash(new SHA1Managed(), GetHmacSha1);
    }

    public static Hash Sha256()
    {
      return new Hash(new SHA256Managed(), GetHmacSha256);
    }

    private static HMAC GetHmacSha1(byte[] salt)
    {
      return new HMACSHA1(salt);
    }

    private static HMAC GetHmacSha256(byte[] salt)
    {
      return new HMACSHA256(salt);
    }

    private readonly HashAlgorithm _hashAlgorithm;
    private readonly Func<byte[], HMAC> _hmacFactory;
    
    private Hash(HashAlgorithm algorithm, Func<byte[], HMAC> hmacFactory)
    {
      _hashAlgorithm = algorithm;
      _hmacFactory = hmacFactory;
    }

    public byte[] ComputeHash(byte[] value)
    {
      return _hashAlgorithm.ComputeHash(value);
    }

    public byte[] ComputeHash(byte[] value, byte[] salt)
    {
      var hmacAlgorithm = _hmacFactory(salt);
      return hmacAlgorithm.ComputeHash(value);
    }

    public byte[] ComputeHash(byte[] value, byte[] salt, int iterations)
    {
      var completeSalt = salt.Concat(BitConverter.GetBytes(1)).ToArray();
      var iteration = ComputeHash(value, completeSalt);
      var final = iteration;

      for (var i = 1; i < iterations; i++)
      {
        iteration = ComputeHash(value, iteration);
        final = final.ExclusiveOr(iteration);
      }

      return final;
    }
  }
}
