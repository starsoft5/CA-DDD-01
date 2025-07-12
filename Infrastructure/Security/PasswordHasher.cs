using System.Security.Cryptography;

namespace Infrastructure.Security;

public class PasswordHasher
{
    public (string Hash, string Salt) Hash(string plain)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool Verify(string plain, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var pbkdf2 = new Rfc2898DeriveBytes(plain, saltBytes, 100_000, HashAlgorithmName.SHA256);
        var hashBytes = pbkdf2.GetBytes(32);
        string computedHash = Convert.ToBase64String(hashBytes);
        return computedHash == hash;
    }
}
