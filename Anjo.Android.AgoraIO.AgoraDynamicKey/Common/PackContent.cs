namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Common
{
    public class PackContent : IPackable
    {
        public byte[] Signature;
        public uint CrcChannelName;
        public uint CrcUid;
        public byte[] RawMessage;

        public PackContent()
        {
        }

        public PackContent(byte[] signature, uint crcChannelName, uint crcUid, byte[] rawMessage)
        {
            Signature = signature;
            CrcChannelName = crcChannelName;
            CrcUid = crcUid;
            RawMessage = rawMessage;
        }


        public ByteBuf Marshal(ByteBuf outBuf)
        {
            return outBuf.Put(Signature).Put(CrcChannelName).Put(CrcUid).Put(RawMessage);
        }


        public void Unmarshal(ByteBuf inBuf)
        {
            Signature = inBuf.ReadBytes();
            CrcChannelName = inBuf.ReadInt();
            CrcUid = inBuf.ReadInt();
            RawMessage = inBuf.ReadBytes();
        }
    }
}
