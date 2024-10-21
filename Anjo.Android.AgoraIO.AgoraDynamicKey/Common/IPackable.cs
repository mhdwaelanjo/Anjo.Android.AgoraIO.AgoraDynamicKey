namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Common
{
    public interface IPackable
    {
        ByteBuf Marshal(ByteBuf outBuf);
    }
}
