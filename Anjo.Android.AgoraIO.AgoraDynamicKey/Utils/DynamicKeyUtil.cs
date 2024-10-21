using System.Security.Cryptography;
using System.Text;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Utils
{
    public static class DynamicKeyUtil
    {
        public static byte[] EncodeHmac(string key, byte[] message, string alg = "SHA1")// throws NoSuchAlgorithmException, InvalidKeyException
        {
            return EncodeHmac(Encoding.UTF8.GetBytes(key), message, alg);
        }

        public static byte[] EncodeHmac(byte[] keyBytes, byte[] textBytes, string alg = "SHA1")// throws NoSuchAlgorithmException, InvalidKeyException 
        {
            //SecretKeySpec keySpec = new SecretKeySpec(key, "RAW");

            //Mac mac = Mac.getInstance("HmacSHA1");
            //mac.init(keySpec);
            //return mac.doFinal(message);
            KeyedHashAlgorithm hash;
            switch (alg)
            {
                case "MD5":
                    hash = new HMACMD5(keyBytes);
                    break;
                case "SHA256":
                    hash = new HMACSHA256(keyBytes);
                    break;
                case "SHA384":
                    hash = new HMACSHA384(keyBytes);
                    break;
                case "SHA512":
                    hash = new HMACSHA512(keyBytes);
                    break;
                case "SHA1":
                default:
                    hash = new HMACSHA1(keyBytes);
                    break;
            }


            byte[] hashBytes = hash.ComputeHash(textBytes);

            return hashBytes;
        }

        public static string BytesToHex(byte[] inData)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in inData)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString().ToLower();
        }
    }
}
