using System.Security.Cryptography;

namespace Database.Common.User;

public class PasswordHasherResult
{
  public string PasswordHash { get; set; }
  public string Salt { get; set; }
}

public static class PasswordHasher 
{
    private const int SaltSize = 16;

    private const int HashSize = 20;

    public static string Hash(string password, int iterations = 10000)
    {
        var salt = GenerateSalt();

        var hash = GenerateHash(password, iterations, salt);

        var hashBytes = CombineSaltAndHash(salt, hash);

        return FormatBase64Hash(iterations, hashBytes);
    }

    private static string FormatBase64Hash(int iterations, byte[] hashBytes)
    {
        var base64Hash = Convert.ToBase64String(hashBytes);
        return string.Format("{0}${1}", iterations, base64Hash);
    }

    private static byte[] CombineSaltAndHash(byte[] salt, byte[] hash)
    {
        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
        return hashBytes;
    }

    private static byte[] GenerateHash(string password, int iterations, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        var hash = pbkdf2.GetBytes(HashSize);
        return hash;
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
        return salt;
    }

    public static bool Verify(string password, string hashedPassword)
    {
        var splittedHashString = hashedPassword.Split('$');
        var iterations = int.Parse(splittedHashString[0]);
        var base64Hash = splittedHashString[1];

        var hashBytes = Convert.FromBase64String(base64Hash);

        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        for (var i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}