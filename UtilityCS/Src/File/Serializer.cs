using System.Buffers.Binary;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Serialization;

namespace BenScr.IO
{
    public static class Serializer
    {
        public static readonly JsonSerializerOptions DefaultJson = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
        public static readonly JsonSerializerOptions FormatedJson = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static class Binary
        {
            public static void SerializeUnmanagedBlock<T>(string path, ReadOnlySpan<T> span, CompressionLevel level = CompressionLevel.Fastest) where T : unmanaged
            {
                string dirPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dirPath))
                    Directory.CreateDirectory(dirPath);

                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1 << 20, FileOptions.SequentialScan);
                using var gzip = new GZipStream(fs, level, leaveOpen: false);


                Span<byte> len = stackalloc byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(len, span.Length);
                gzip.Write(len);


                ReadOnlySpan<byte> raw = MemoryMarshal.AsBytes(span);
                gzip.Write(raw);
            }
            public static ReadOnlySpan<T> DeserializeUnmanagedBlock<T>(string path) where T : unmanaged
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"File not found at path ({path})");

                using var fs = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 1 << 20,
                    FileOptions.SequentialScan);

                using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);


                Span<byte> lenSpan = stackalloc byte[4];
                int read = gzip.Read(lenSpan);
                if (read != lenSpan.Length)
                    throw new EndOfStreamException($"Expected {lenSpan.Length} bytes but only read {read}.");

                int count = BinaryPrimitives.ReadInt32LittleEndian(lenSpan);


                int byteCount = count * Unsafe.SizeOf<T>();
                byte[] buffer = new byte[byteCount];
                int offset = 0;
                while (offset < byteCount)
                {
                    int n = gzip.Read(buffer, offset, byteCount - offset);
                    if (n == 0)
                        throw new EndOfStreamException();
                    offset += n;
                }

                return MemoryMarshal.Cast<byte, T>(buffer);
            }
            public static void Serialize<T>(string path, T item, CompressionLevel compressionLevel = CompressionLevel.Fastest, JsonSerializerOptions? options = null)
            {
                string dirPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dirPath))
                    Directory.CreateDirectory(dirPath);

                using var fs = new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 1 << 20,
                    options: FileOptions.SequentialScan);

                options ??= DefaultJson;

                using var gzip = new GZipStream(fs, compressionLevel, leaveOpen: false);
                System.Text.Json.JsonSerializer.Serialize(gzip, item, DefaultJson);
            }
            public static T Deserialize<T>(string path, JsonSerializerOptions? options = null)
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"File not found at path ({path})");

                using var fs = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 1 << 20,
                    options: FileOptions.SequentialScan);

                options ??= DefaultJson;

                using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);

                return System.Text.Json.JsonSerializer.Deserialize<T>(gzip, DefaultJson)
                       ?? throw new InvalidDataException("Deserialization resulted in null");
            }
        }
        public static class Json
        {
            public static void Serialize<T>(string path, T obj, JsonSerializerOptions? options = null)
            {
                string dirPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dirPath))
                    Directory.CreateDirectory(dirPath);

                using FileStream fs = new FileStream(
                    path, FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 1 << 20, FileOptions.SequentialScan);

                options ??= DefaultJson;

                using (fs)
                {
                    System.Text.Json.JsonSerializer.Serialize(fs, obj, options);
                }
            }
            public static T Deserialize<T>(string path, T defaultValue = default!, JsonSerializerOptions? options = null)
            {
                if (!File.Exists(path)) return defaultValue;

                using FileStream fs = new FileStream(
                    path, FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 1 << 20, FileOptions.SequentialScan);

                options ??= DefaultJson;

                using (fs)
                {
                    try
                    {
                        return JsonSerializer.Deserialize<T>(fs, options)! ?? throw new InvalidDataException("Deserialization resulted in null");
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
        }
        public static class JsonSecure
        {
            private const int saltSize = 16;
            private const int ivSize = 16;
            private const int keySize = 32;
            private const int iterations = 100_000;

            public static void Serialize<T>(string path, string password, T obj, JsonSerializerOptions? options = null)
            {
                string dirPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dirPath))
                    Directory.CreateDirectory(dirPath);

                using FileStream fs = new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 1 << 20,
                    options: FileOptions.SequentialScan);

                options ??= DefaultJson;

                byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
                byte[] iv = RandomNumberGenerator.GetBytes(ivSize);

                using var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                byte[] key = kdf.GetBytes(keySize);

                fs.Write(salt);
                fs.Write(iv);

                using Aes aes = Aes.Create()!;
                aes.KeySize = keySize * 8;
                aes.BlockSize = ivSize * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;


                using var crypto = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
                JsonSerializer.Serialize(crypto, obj, options);
            }
            public static T Deserialize<T>(string path, string password, T defaultValue = default!, JsonSerializerOptions? options = null)
            {
                if (!File.Exists(path))
                    return defaultValue;

                try
                {
                    using FileStream fs = new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        bufferSize: 1 << 20,
                        options: FileOptions.SequentialScan);

                    byte[] salt = new byte[saltSize];
                    byte[] iv = new byte[ivSize];
                    if (fs.Read(salt) != saltSize || fs.Read(iv) != ivSize)
                        throw new InvalidDataException("Invalid fileformat");

                    options ??= DefaultJson;

                    using var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                    byte[] key = kdf.GetBytes(keySize);

                    using Aes aes = Aes.Create()!;
                    aes.KeySize = keySize * 8;
                    aes.BlockSize = ivSize * 8;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = iv;

                    using var crypto = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read);

                    return JsonSerializer.Deserialize<T>(crypto, options)!
                           ?? throw new InvalidDataException("Deserialization resulted in null");
                }
                catch
                {
                    throw new InvalidDataException("Wrong password or damaged file");
                }
            }
        }
        public static class Xml
        {
            public static void Serialize<T>(string path, T obj)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                FileStream fs = new FileStream(path, FileMode.Create);
                xmlSerializer.Serialize(fs, obj);
                fs.Close();
            }

            public static T Deserialize<T>(string path)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                FileStream fs = new FileStream(path, FileMode.Open);
                T obj = (T)xmlSerializer.Deserialize(fs);
                fs.Close();
                return obj;
            }
        }
    }
}
