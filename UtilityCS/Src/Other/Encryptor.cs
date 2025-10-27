using System.Security.Cryptography;
using System.Text;

namespace BenScr.Cryptography
{
    public static class Encryptor
    {
        public static class Bytes
        {
            public static byte[] Encrypt(byte[] data)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();

                    // Hier nur Demo: Schlüssel + IV vorne anhängen, damit man später entschlüsseln kann
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        ms.Write(aes.Key, 0, aes.Key.Length);

                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }

                        return ms.ToArray();
                    }
                }
            }
            public static byte[] Decrypt(byte[] encryptedData)
            {
                using (Aes aes = Aes.Create())
                {
                    // IV und Key müssen bekannt sein (z. B. vom vorherigen Encrypt gespeichert)
                    // Hier nur zur Demonstration: Wir nehmen sie aus den ersten Bytes
                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] key = new byte[aes.KeySize / 8];

                    Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                    Buffer.BlockCopy(encryptedData, iv.Length, key, 0, key.Length);

                    aes.IV = iv;
                    aes.Key = key;

                    int offset = iv.Length + key.Length;
                    byte[] cipherText = new byte[encryptedData.Length - offset];
                    Buffer.BlockCopy(encryptedData, offset, cipherText, 0, cipherText.Length);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
            public static byte[] Encrypt(byte[] data, string password)
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] salt = GenerateRandomBytes(16);
                    var key = new Rfc2898DeriveBytes(password, salt, 100_000);

                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);

                    using (var ms = new MemoryStream())
                    {
                        // Salt mitschreiben (wird zum Entschlüsseln gebraucht)
                        ms.Write(salt, 0, salt.Length);

                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }

                        return ms.ToArray();
                    }
                }
            }
            public static byte[] Decrypt(byte[] encryptedData, string password)
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] salt = new byte[16];
                    Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);

                    var key = new Rfc2898DeriveBytes(password, salt, 100_000);
                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);

                    int offset = salt.Length;
                    byte[] cipherText = new byte[encryptedData.Length - offset];
                    Buffer.BlockCopy(encryptedData, offset, cipherText, 0, cipherText.Length);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
        }
        public static class String
        {
            public static string Encrypt(string content)
            {
                byte[] data = Encoding.UTF8.GetBytes(content);

                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();

                    // Hier nur Demo: Schlüssel + IV vorne anhängen, damit man später entschlüsseln kann
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        ms.Write(aes.Key, 0, aes.Key.Length);

                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            public static string Decrypt(string encryptedContent)
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedContent);

                using (Aes aes = Aes.Create())
                {
                    // IV und Key müssen bekannt sein (z. B. vom vorherigen Encrypt gespeichert)
                    // Hier nur zur Demonstration: Wir nehmen sie aus den ersten Bytes
                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] key = new byte[aes.KeySize / 8];

                    Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                    Buffer.BlockCopy(encryptedData, iv.Length, key, 0, key.Length);

                    aes.IV = iv;
                    aes.Key = key;

                    int offset = iv.Length + key.Length;
                    byte[] cipherText = new byte[encryptedData.Length - offset];
                    Buffer.BlockCopy(encryptedData, offset, cipherText, 0, cipherText.Length);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            public static string Encrypt(string content, string password)
            {
                byte[] data = Encoding.UTF8.GetBytes(content);

                using (Aes aes = Aes.Create())
                {
                    byte[] salt = GenerateRandomBytes(16);
                    var key = new Rfc2898DeriveBytes(password, salt, 100_000);

                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);

                    using (var ms = new MemoryStream())
                    {
                        // Salt mitschreiben (wird zum Entschlüsseln gebraucht)
                        ms.Write(salt, 0, salt.Length);

                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            public static string Decrypt(string encryptedContent, string password)
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedContent);

                using (Aes aes = Aes.Create())
                {
                    byte[] salt = new byte[16];
                    Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);

                    var key = new Rfc2898DeriveBytes(password, salt, 100_000);
                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);

                    int offset = salt.Length;
                    byte[] cipherText = new byte[encryptedData.Length - offset];
                    Buffer.BlockCopy(encryptedData, offset, cipherText, 0, cipherText.Length);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
    }
}
