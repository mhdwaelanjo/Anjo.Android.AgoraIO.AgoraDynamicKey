using System.Text.RegularExpressions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Common;
using Ionic.Zlib;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Utils
{
    public static class Utils
    {
        public static int GetTimestamp()
        {
            return (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public static int RandomInt()
        {
            return new Random().Next();
        }

        public static byte[] Pack(PrivilegeMessage packableEx)
        {
            ByteBuf buffer = new ByteBuf();
            packableEx.Marshal(buffer);
            return buffer.AsBytes();
        }
        public static byte[] Pack(IPackable packableEx)
        {
            ByteBuf buffer = new ByteBuf();
            packableEx.Marshal(buffer);
            return buffer.AsBytes();
        }

        public static string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64Decode(string data)
        {
            return Convert.FromBase64String(data);
        }

        public static bool IsUuid(string uuid)
        {
            if (uuid.Length != 32)
            {
                return false;
            }

            Regex regex = new Regex("^[0-9a-fA-F]{32}$");
            return regex.IsMatch(uuid);
        }

        public static byte[] Compress(byte[] data)
        {
            byte[] output;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (var zlibStream = new ZlibStream(outputStream, CompressionMode.Compress, CompressionLevel.Level5, true)) // or use Level6
                {
                    zlibStream.Write(data, 0, data.Length);
                }
                output = outputStream.ToArray();
            }

            return output;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] output;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (var zlibStream = new ZlibStream(outputStream, CompressionMode.Decompress))
                {
                    zlibStream.Write(data, 0, data.Length);
                }
                output = outputStream.ToArray();
            }

            return output;
        }
    }
}
