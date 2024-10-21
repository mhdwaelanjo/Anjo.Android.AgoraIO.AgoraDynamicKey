using Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Utils;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Media
{
    public class DynamicKey3
    {
        public static string Generate(string appId, string appCertificate, string channelName, int unixTs, int randomInt, long uid, int expiredTs) //throws Exception
        {
            string version = "003";
            string unixTsStr = ("0000000000" + unixTs).Substring(unixTs.ToString().Length);
            string randomIntStr = ("00000000" + randomInt.ToString("x4")).Substring(randomInt.ToString("x4").Length);
            uid = uid & 0xFFFFFFFFL;
            string uidStr = ("0000000000" + uid.ToString()).Substring(uid.ToString().Length);
            string expiredTsStr = ("0000000000" + expiredTs.ToString()).Substring(expiredTs.ToString().Length);
            string signature = GenerateSignature3(appId, appCertificate, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr);
            return string.Format("{0}{1}{2}{3}{4}{5}{6}", version, signature, appId, unixTsStr, randomIntStr, uidStr, expiredTsStr);
        }

        public static string GenerateSignature3(string appId, string appCertificate, string channelName, string unixTsStr, string randomIntStr, string uidStr, string expiredTsStr)// throws Exception
        {
            using (var ms = new MemoryStream())
            using (BinaryWriter baos = new BinaryWriter(ms))
            {
                baos.Write(appId.GetByteArray());
                baos.Write(unixTsStr.GetByteArray());
                baos.Write(randomIntStr.GetByteArray());
                baos.Write(channelName.GetByteArray());
                baos.Write(uidStr.GetByteArray());
                baos.Write(expiredTsStr.GetByteArray());
                baos.Flush();

                byte[] sign = DynamicKeyUtil.EncodeHmac(appCertificate, ms.ToArray());
                return DynamicKeyUtil.BytesToHex(sign);
            }
        }
    }
}
