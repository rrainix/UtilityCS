using System.Buffers.Binary;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace UtilityCS
{
    public static class Serializer
    {
        private static readonly JsonSerializerOptions defaultJsonOptions = new JsonSerializerOptions
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
            public static void SaveUnmanagedBlock<T>(string path, ReadOnlySpan<T> span, CompressionLevel level = CompressionLevel.Fastest) where T : unmanaged
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
            public static ReadOnlySpan<T> LoadUnmanagedBlock<T>(string path) where T : unmanaged
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


                int byteCount = count * Marshal.SizeOf<T>();
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

            public static void SaveObject<T>(string path, T item, CompressionLevel compressionLevel = CompressionLevel.Fastest, JsonSerializerOptions? options = null)
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

                options ??= defaultJsonOptions;

                using var gzip = new GZipStream(fs, compressionLevel, leaveOpen: false);
                System.Text.Json.JsonSerializer.Serialize(gzip, item, defaultJsonOptions);
            }
            public static T LoadObject<T>(string path, JsonSerializerOptions? options = null)
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

                options ??= defaultJsonOptions;

                using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);

                return System.Text.Json.JsonSerializer.Deserialize<T>(gzip, defaultJsonOptions)
                       ?? throw new InvalidDataException("Deserialization resulted in null");
            }
        }
        public static class Json
        {
            public static void SaveObject<T>(string path, T obj, JsonSerializerOptions? options = null)
            {
                string dirPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dirPath))
                    Directory.CreateDirectory(dirPath);

                using FileStream fs = new FileStream(
                    path, FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 1 << 20, FileOptions.SequentialScan);

                options ??= defaultJsonOptions;

                using (fs)
                {
                    System.Text.Json.JsonSerializer.Serialize(fs, obj, options);
                }
            }
            public static T LoadObject<T>(string path, T defaultValue = default!, JsonSerializerOptions? options = null)
            {
                if (!File.Exists(path)) return defaultValue;

                using FileStream fs = new FileStream(
                    path, FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 1 << 20, FileOptions.SequentialScan);

                options ??= defaultJsonOptions;

                using (fs)
                {
                    try
                    {
                        return System.Text.Json.JsonSerializer.Deserialize<T>(fs, options)!
                               ?? throw new InvalidDataException("Deserialization resulted in null");
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
            private const int SaltSize = 16;
            private const int IvSize = 16;
            private const int KeySize = 32;
            private const int Iterations = 100_000;

            public static void SaveObject<T>(string path, string password, T obj, JsonSerializerOptions? options = null)
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

                options ??= defaultJsonOptions;

                byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
                byte[] iv = RandomNumberGenerator.GetBytes(IvSize);

                using var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
                byte[] key = kdf.GetBytes(KeySize);

                fs.Write(salt);
                fs.Write(iv);

                using Aes aes = Aes.Create()!;
                aes.KeySize = KeySize * 8;
                aes.BlockSize = IvSize * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;


                using var crypto = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
                JsonSerializer.Serialize(crypto, obj, options);
            }
            public static T LoadObject<T>(string path, string password, T defaultValue = default!, JsonSerializerOptions? options = null)
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

                    byte[] salt = new byte[SaltSize];
                    byte[] iv = new byte[IvSize];
                    if (fs.Read(salt) != SaltSize || fs.Read(iv) != IvSize)
                        throw new InvalidDataException("Invalid fileformat");

                    options ??= defaultJsonOptions;

                    using var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
                    byte[] key = kdf.GetBytes(KeySize);

                    using Aes aes = Aes.Create()!;
                    aes.KeySize = KeySize * 8;
                    aes.BlockSize = IvSize * 8;
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
    }
}
