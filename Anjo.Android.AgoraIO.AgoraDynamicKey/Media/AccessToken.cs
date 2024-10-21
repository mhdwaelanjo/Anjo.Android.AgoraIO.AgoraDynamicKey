using Anjo.Android.AgoraIO.AgoraDynamicKey.Common;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Utils;
using Force.Crc32;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Media
{
    public class AccessToken
    {
        private readonly string AppId;
        private readonly string AppCertificate;
        private readonly string ChannelName;
        private readonly string Uid;
        private uint Ts;
        private uint Salt;
        private byte[] Signature;
        private uint CrcChannelName;
        private uint CrcUid;
        private byte[] MessageRawContent;
        public PrivilegeMessage Message = new PrivilegeMessage();

        public AccessToken(string appId, string appCertificate, string channelName, string uid)
        {
            AppId = appId;
            AppCertificate = appCertificate;
            ChannelName = channelName;
            Uid = uid;
        }

        public AccessToken(string appId, string appCertificate, string channelName, string uid, uint ts, uint salt)
        {
            AppId = appId;
            AppCertificate = appCertificate;
            ChannelName = channelName;
            Uid = uid;
            Ts = ts;
            Salt = salt;
        }

        public void AddPrivilege(Privileges kJoinChannel, uint expiredTs)
        {
            Message.Messages.Add((ushort)kJoinChannel, expiredTs);
        }

        public string Build()
        {
            //if (!Utils.isUUID(this.appId))
            //{
            //    return "";
            //}

            //if (!Utils.isUUID(this.appCertificate))
            //{
            //    return "";
            //}

            MessageRawContent = Utils.Utils.Pack(Message);
            Signature = GenerateSignature(AppCertificate
                    , AppId
                    , ChannelName
                    , Uid
                    , MessageRawContent);

            CrcChannelName = Crc32Algorithm.Compute(ChannelName.GetByteArray());
            CrcUid = Crc32Algorithm.Compute(Uid.GetByteArray());

            PackContent packContent = new PackContent(Signature, CrcChannelName, CrcUid, MessageRawContent);
            byte[] content = Utils.Utils.Pack(packContent);
            return GetVersion() + AppId + Utils.Utils.Base64Encode(content);
        }
        public static string GetVersion()
        {
            return "006";
        }

        public static byte[] GenerateSignature(string appCertificate
                , string appId
                , string channelName
                , string uid
                , byte[] message)
        {

            using (var ms = new MemoryStream())
            using (BinaryWriter baos = new BinaryWriter(ms))
            {
                baos.Write(appId.GetByteArray());
                baos.Write(channelName.GetByteArray());
                baos.Write(uid.GetByteArray());
                baos.Write(message);
                baos.Flush();

                byte[] sign = DynamicKeyUtil.EncodeHmac(appCertificate, ms.ToArray(), "SHA256");
                return sign;
            }
        }
    }
}
