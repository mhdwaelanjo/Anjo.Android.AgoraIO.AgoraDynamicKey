namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Common
{
    public class ByteBuf
    {
        readonly ByteBuffer Buffer = new ByteBuffer();
        public ByteBuf()
        {

        }

        public ByteBuf(byte[] source)
        {
            Buffer.PushByteArray(source);
        }

        public byte[] AsBytes()
        {
            //byte[] outBuf = new byte[buffer.Position];
            //buffer.rewind();
            //buffer.get(outBuf, 0, out.length);
            //return outBuf;
            return Buffer.ToByteArray();
        }

        // packUint16
        public ByteBuf Put(ushort v)
        {
            Buffer.PushUInt16(v);
            return this;
        }

        // packUint32
        public ByteBuf Put(uint v)
        {
            Buffer.PushLong(v);
            return this;
        }

        public ByteBuf Put(byte[] v)
        {
            Put((ushort)v.Length);
            Buffer.PushByteArray(v);
            return this;
        }

        public ByteBuf Copy(byte[] v)
        {
            Buffer.PushByteArray(v);
            return this;
        }

        public ByteBuf PutIntMap(Dictionary<ushort, uint> extra)
        {
            Put((ushort)extra.Count);

            foreach (var item in extra)
            {
                Put(item.Key);
                Put(item.Value);
            }
            return this;
        }

        public ushort ReadShort()
        {
            return Buffer.PopUInt16();
        }

        public uint ReadInt()
        {
            return Buffer.PopUInt();
        }

        public byte[] ReadBytes()
        {
            ushort length = ReadShort();
            byte[] bytes = new byte[length];
            //buffer.get(bytes);
            //return bytes;
            return Buffer.PopByteArray(length);
        }

        public Dictionary<ushort, uint> ReadIntMap()
        {
            Dictionary<ushort, uint> map = new Dictionary<ushort, uint>();

            ushort length = ReadShort();

            for (short i = 0; i < length; ++i)
            {
                ushort k = ReadShort();
                uint v = ReadInt();
                map.Add(k, v);
            }

            return map;
        }
    }
}
