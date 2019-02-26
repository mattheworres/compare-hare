using System.Security.Cryptography;
using System.Text;
using CompareHare.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace CompareHare.Domain.Services
{
    public class ObjectHasher : IObjectHasher
  {
    const string HASH_FORMAT = "X2";

    public string HashObject(object obj)
    {
      var json = GetJsonOfObject(obj);

      using (SHA1Managed sha1 = new SHA1Managed())
      {
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(json));
        return GetNormalizedString(hash);
      }
    }

    private string GetJsonOfObject(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    private string GetNormalizedString(byte[] hash) {
      var stringBuilder = new StringBuilder(hash.Length * 2);

      foreach (byte b in hash)
      {
          stringBuilder.Append(b.ToString(HASH_FORMAT));
      }

      return stringBuilder.ToString();
    }
  }
}
