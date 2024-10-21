namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Common
{
    public class PrivilegeMessage : IPackable
    {
        public uint Salt;
        public uint Ts;
        public Dictionary<ushort, uint> Messages;
        public PrivilegeMessage()
        {
            Salt = (uint)Utils.Utils.RandomInt();
            Ts = (uint)(Utils.Utils.GetTimestamp() + 24 * 3600);
            Messages = new Dictionary<ushort, uint>();
        }

        public ByteBuf Marshal(ByteBuf outBuf)
        {
            return outBuf.Put(Salt).Put(Ts).PutIntMap(Messages);
        }

        public void Unmarshal(ByteBuf inBuf)
        {
            Salt = inBuf.ReadInt();
            Ts = inBuf.ReadInt();
            Messages = inBuf.ReadIntMap();
        }
    }
}
