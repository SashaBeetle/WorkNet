using System.Security.Cryptography;

namespace Worknet.Shared.Helpers;

public static class PasswordHelper
{
    private const int SaltSize = 16; // 16 bytes
    private const int HashSize = 32; // 32 bytes
    private const int Iterations = 310000;

    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt;
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt = new byte[SaltSize]);
        }

        // Derive a key from the password, salt, and iterations
        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            hash = pbkdf2.GetBytes(HashSize);
        }

        // Combine salt, iterations, and hash into a single string
        // We convert to Base64 strings for easy storage and to avoid issues with character encodings.
        return $"{Convert.ToBase64String(salt)}.{Iterations}.{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies a plain-text password against a stored hashed password.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="hashedPassword">The stored hashed password (obtained from HashPassword).</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Parse the stored hashed password to extract salt, iterations, and the stored hash
        var parts = hashedPassword.Split('.');

        if (parts.Length != 3)
        {
            // Invalid hashed password format
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[0]);
            var iterations = int.Parse(parts[1]);
            var storedHash = Convert.FromBase64String(parts[2]);

            // Re-derive the hash using the provided password and the extracted salt and iterations
            byte[] derivedHash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                derivedHash = pbkdf2.GetBytes(HashSize);
            }

            // Compare the newly derived hash with the stored hash
            // This comparison must be done in constant time to prevent timing attacks.
            // A simple byte-by-byte comparison is vulnerable to timing attacks.
            // Cryptographic operations are designed to be constant time.
            return CryptographicEquals(derivedHash, storedHash);
        }
        catch (FormatException)
        {
            // Base64 decoding failed, or iterations is not a valid integer.
            return false;
        }
        catch (OverflowException)
        {
            // Iterations value is too large.
            return false;
        }
        catch (ArgumentNullException)
        {
            // Input password or hashed password was null.
            return false;
        }
        catch (Exception)
        {
            // Catch any other unexpected exceptions.
            return false;
        }
    }

    /// <summary>
    /// Performs a constant-time comparison of two byte arrays to prevent timing attacks.
    /// </summary>
    /// <param name="a">The first byte array.</param>
    /// <param name="b">The second byte array.</param>
    /// <returns>True if the arrays are equal, false otherwise.</returns>
    private static bool CryptographicEquals(byte[] a, byte[] b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            diff |= (uint)(a[i] ^ b[i]);
        }
        return diff == 0;
    }
}