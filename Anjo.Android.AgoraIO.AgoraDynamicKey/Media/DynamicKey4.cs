using Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Utils;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Media
{
    public class DynamicKey4
    {
        private static readonly string PublicSharingService = "APSS";
        private static readonly string RecordingService = "ARS";
        private static readonly string MediaChannelService = "ACS";
        /**
         * Generate Dynamic Key for Public Sharing Service
         * @param appID App IDassigned by Agora
         * @param appCertificate App Certificate assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs should be 0
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static string GeneratePublicSharingKey(string appId, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)// throws Exception
        {
            return DoGenerate(appId, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, PublicSharingService);
        }


        /**
         * Generate Dynamic Key for recording service
         * @param appID Vendor key assigned by Agora
         * @param appCertificate Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs should be 0
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static string GenerateRecordingKey(string appId, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)// throws Exception
        {
            return DoGenerate(appId, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, RecordingService);
        }

        /**
         * Generate Dynamic Key for media channel service
         * @param appID Vendor key assigned by Agora
         * @param appCertificate Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs service expiring timestamp. After this timestamp, user will not be able to stay in the channel.
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static string GenerateMediaChannelKey(string appId, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)// throws Exception
        {
            return DoGenerate(appId, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, MediaChannelService);
        }

        private static string DoGenerate(string appId, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs, string serviceType) //throws Exception
        {
            string version = "004";
            string unixTsStr = ("0000000000" + unixTs).Substring(unixTs.ToString().Length);

            string randomIntStr = ("00000000" + randomInt.ToString("x4")).Substring(randomInt.ToString("x4").Length);
            //uid = uid & 0xFFFFFFFFL;
            string uidStr = uid; //("0000000000" + uid.ToString()).Substring(uid.ToString().Length);
            string expiredTsStr = ("0000000000" + expiredTs.ToString()).Substring(expiredTs.ToString().Length);


            //String randomIntStr = ("00000000" + Integer.toHexString(randomInt)).Substring(Integer.toHexString(randomInt).length());
            //uid = uid & 0xFFFFFFFFL;
            //String uidStr = ("0000000000" + Long.toString(uid)).Substring(Long.toString(uid).length());
            //String expiredTsStr = ("0000000000" + Integer.toString(expiredTs)).Substring(Integer.toString(expiredTs).length());
            string signature = GenerateSignature4(appId, appCertificate, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr, serviceType);
            return string.Format("{0}{1}{2}{3}{4}{5}", version, signature, appId, unixTsStr, randomIntStr, expiredTsStr);
        }

        private static string GenerateSignature4(string appId, string appCertificate, string channelName, string unixTsStr, string randomIntStr, string uidStr, string expiredTsStr, string serviceType) //throws Exception
        {
            using (var ms = new MemoryStream())
            using (BinaryWriter baos = new BinaryWriter(ms))
            {
                baos.Write(serviceType.GetBytes());
                baos.Write(appId.GetBytes());
                baos.Write(unixTsStr.GetBytes());
                baos.Write(randomIntStr.GetBytes());
                baos.Write(channelName.GetBytes());
                baos.Write(uidStr.GetBytes());
                baos.Write(expiredTsStr.GetBytes());

                byte[] sign = DynamicKeyUtil.EncodeHmac(appCertificate, ms.ToArray());
                return DynamicKeyUtil.BytesToHex(sign);
            }


            //byte[] sign = DynamicKeyUtil.encodeHMAC(appCertificate, baos.toByteArray());
            //return DynamicKeyUtil.bytesToHex(sign);
        }
    }
}
