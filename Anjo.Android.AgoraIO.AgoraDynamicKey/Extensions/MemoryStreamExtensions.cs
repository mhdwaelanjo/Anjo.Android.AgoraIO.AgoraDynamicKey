using System.Text;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static void Write(this MemoryStream obj, string data)
        {
            var array = Encoding.UTF8.GetBytes(data);
            obj.Write(array, (int)obj.Length, array.Length);
        }

        public static byte[] GetByteArray(this string obj)
        {
            return Encoding.UTF8.GetBytes(obj);
        }
        public static byte[] GetBytes(this string obj)
        {
            return Encoding.UTF8.GetBytes(obj);
        }

        public static string GetString(this byte[] obj)
        {
            return Encoding.UTF8.GetString(obj);
        }
    }
}
