using Anjo.Android.AgoraIO.AgoraDynamicKey.Common;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Utils;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Media
{
    public class AccessToken2
    {
        private static readonly string Version = "007";
        private static readonly int VersionLength = 3;

        public string AppCert = "";
        public string AppId = "";
        public uint Expire;
        public uint IssueTs;
        public uint Salt;
        public Dictionary<ushort, Service> Services = new Dictionary<ushort, Service>();

        public AccessToken2()
        {
        }

        public AccessToken2(string appId, string appCert, uint expire)
        {
            AppCert = appCert;
            AppId = appId;
            Expire = expire;
            IssueTs = (uint)Utils.Utils.GetTimestamp();
            Salt = (uint)Utils.Utils.RandomInt();
        }

        public void AddService(Service service)
        {
            Services.Add((ushort)service.GetServiceType(), service);
        }

        public static short ServiceTypeRtc = 1;
        public static short ServiceTypeRtm = 2;
        public static short ServiceTypeFpa = 4;
        public static short ServiceTypeChat = 5;
        public static short ServiceTypeApaas = 7;

        public Service GetService(short serviceType)
        {
            if (serviceType == ServiceTypeRtc)
            {
                return new ServiceRtc();
            }
            if (serviceType == ServiceTypeRtm)
            {
                return new ServiceRtm();
            }
            if (serviceType == ServiceTypeFpa)
            {
                return new ServiceFpa();
            }
            if (serviceType == ServiceTypeChat)
            {
                return new ServiceChat();
            }
            if (serviceType == ServiceTypeApaas)
            {
                return new ServiceApaas();
            }
            throw new ArgumentException("unknown service type:", serviceType.ToString());
        }

        public static string GetUidStr(uint uid)
        {
            if (uid == 0)
            {
                return "";
            }
            return (uid & 0xFFFFFFFFL).ToString();
        }

        public string GetVersion()
        {
            return Version;
        }

        public byte[] GetSign()
        {
            byte[] signing = DynamicKeyUtil.EncodeHmac(BitConverter.GetBytes(IssueTs), AppCert.GetBytes(), "SHA256");
            return DynamicKeyUtil.EncodeHmac(BitConverter.GetBytes(Salt), signing, "SHA256");
        }

        public string Build()
        {
            if (!Utils.Utils.IsUuid(AppId) || !Utils.Utils.IsUuid(AppCert))
            {
                return "";
            }

            ByteBuf buf = new ByteBuf().Put(AppId.GetBytes()).Put((uint)IssueTs).Put(Expire).Put((uint)Salt).Put((ushort)Services.Count);
            byte[] signing = GetSign();

            foreach (var it in Services)
            {
                it.Value.Pack(buf);
            }

            byte[] signature = DynamicKeyUtil.EncodeHmac(signing, buf.AsBytes(), "SHA256");

            ByteBuf bufferContent = new ByteBuf();
            bufferContent.Put(signature);
            bufferContent.Copy(buf.AsBytes());

            return GetVersion() + Utils.Utils.Base64Encode(Utils.Utils.Compress(bufferContent.AsBytes()));
        }

        public bool Parse(string token)
        {
            if (GetVersion().CompareTo(token.Substring(0, VersionLength)) != 0)
            {
                return false;
            }

            byte[] data = Utils.Utils.Decompress(Utils.Utils.Base64Decode(token.Substring(VersionLength)));

            ByteBuf buff = new ByteBuf(data);
            string signature = buff.ReadBytes().GetString();
            AppId = buff.ReadBytes().GetString();
            IssueTs = buff.ReadInt();
            Expire = buff.ReadInt();
            Salt = buff.ReadInt();
            short servicesNum = (short)buff.ReadShort();

            for (short i = 0; i < servicesNum; i++)
            {
                short serviceType = (short)buff.ReadShort();
                Service service = GetService(serviceType);
                service.Unpack(buff);
                Services.Add((ushort)serviceType, service);
            }

            return true;
        }

        public enum PrivilegeRtcEnum
        {
            PrivilegeJoinChannel = 1,
            PrivilegePublishAudioStream = 2,
            PrivilegePublishVideoStream = 3,
            PrivilegePublishDataStream = 4
        }

        public enum PrivilegeRtmEnum
        {
            PrivilegeLogin = 1
        }

        public enum PrivilegeFpaEnum
        {
            PrivilegeLogin = 1
        }

        public enum PrivilegeChatEnum
        {
            PrivilegeChatUser = 1,
            PrivilegeChatApp = 2
        }
        public enum PrivilegeApaasEnum
        {
            PrivilegeRoomUser = 1,
            PrivilegeUser = 2,
            PrivilegeApp = 3
        }

        public class Service
        {
            private short Type;
            private Dictionary<ushort, uint> Privileges = new Dictionary<ushort, uint>();

            public Service()
            {

            }

            public Service(short serviceType)
            {
                Type = serviceType;
            }

            public void AddPrivilegeRtc(PrivilegeRtcEnum privilege, uint expire)
            {
                Privileges.Add((ushort)privilege, expire);
            }

            public void AddPrivilegeRtm(PrivilegeRtmEnum privilege, uint expire)
            {
                Privileges.Add((ushort)privilege, expire);
            }

            public void AddPrivilegeFpa(PrivilegeFpaEnum privilege, uint expire)
            {
                Privileges.Add((ushort)privilege, expire);
            }

            public void AddPrivilegeChat(PrivilegeChatEnum privilege, uint expire)
            {
                Privileges.Add((ushort)privilege, expire);
            }

            public void AddPrivilegeApaas(PrivilegeApaasEnum privilege, uint expire)
            {
                Privileges.Add((ushort)privilege, expire);
            }

            public Dictionary<ushort, uint> GetPrivileges()
            {
                return Privileges;
            }

            public short GetServiceType()
            {
                return Type;
            }

            public void SetServiceType(short type)
            {
                Type = type;
            }

            public virtual ByteBuf Pack(ByteBuf buf)
            {
                return buf.Put((ushort)Type).PutIntMap(Privileges);
            }

            public virtual void Unpack(ByteBuf byteBuf)
            {
                Privileges = byteBuf.ReadIntMap();
            }
        }

        public class ServiceRtc : Service
        {
            public string ChannelName;
            public string Uid;

            public ServiceRtc()
            {
                SetServiceType(ServiceTypeRtc);
            }

            public ServiceRtc(string channelName, string uid)
            {
                SetServiceType(ServiceTypeRtc);
                ChannelName = channelName;
                Uid = uid;
            }

            public string GetChannelName()
            {
                return ChannelName;
            }

            public string GetUid()
            {
                return Uid;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).Put(ChannelName.GetBytes()).Put(Uid.GetBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                ChannelName = byteBuf.ReadBytes().GetString();
                Uid = byteBuf.ReadBytes().GetString();
            }
        }

        public class ServiceRtm : Service
        {
            public string UserId;

            public ServiceRtm()
            {
                SetServiceType(ServiceTypeRtm);
            }

            public ServiceRtm(string userId)
            {
                SetServiceType(ServiceTypeRtm);
                UserId = userId;
            }

            public string GetUserId()
            {
                return UserId;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).Put(UserId.GetBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                UserId = byteBuf.ReadBytes().GetString();
            }
        }

        public class ServiceFpa : Service
        {
            public ServiceFpa()
            {
                SetServiceType(ServiceTypeFpa);
            }

            public new ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf);
            }

            public new void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
            }
        }

        public class ServiceChat : Service
        {
            public string UserId;

            public ServiceChat()
            {
                SetServiceType(ServiceTypeChat);
                UserId = "";
            }

            public ServiceChat(string userId)
            {
                SetServiceType(ServiceTypeChat);
                UserId = userId;
            }

            public string GetUserId()
            {
                return UserId;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).Put(UserId.GetBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                UserId = byteBuf.ReadBytes().GetString();
            }
        }

        public class ServiceApaas : Service
        {
            public string RoomUuid;
            public string UserUuid;
            public short Role;

            public ServiceApaas()
            {
                SetServiceType(ServiceTypeApaas);
                RoomUuid = "";
                UserUuid = "";
                Role = -1;
            }

            public ServiceApaas(string roomUuid, string userUuid, short role)
            {
                SetServiceType(ServiceTypeApaas);
                RoomUuid = roomUuid;
                UserUuid = userUuid;
                Role = role;
            }

            public ServiceApaas(string userUuid)
            {
                SetServiceType(ServiceTypeApaas);
                RoomUuid = "";
                UserUuid = userUuid;
                Role = -1;
            }

            public string GetRoomUuid()
            {
                return RoomUuid;
            }

            public string GetUserUuid()
            {
                return UserUuid;
            }

            public short GetRole()
            {
                return Role;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).Put(RoomUuid.GetBytes()).Put(UserUuid.GetBytes()).Put((ushort)Role);
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                RoomUuid = byteBuf.ReadBytes().GetString();
                UserUuid = byteBuf.ReadBytes().GetString();
                Role = (short)byteBuf.ReadShort();
            }
        }
    }
}
