namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Common
{
    /// <summary>
        /// 创建一个可变长的Byte数组方便Push数据和Pop数据
        /// 数组的最大长度为1024,超过会产生溢出
        /// 数组的最大长度由常量MAX_LENGTH设定
        /// 
        /// 注:由于实际需要,可能要从左到右取数据,所以这里
        /// 定义的Pop函数并不是先进后出的函数,而是从0开始.
        /// 
        /// @Author: Red_angelX
        /// </summary>
    public class ByteBuffer
    {
        //数组的最大长度
        private const int MaxLength = 1024;

        //固定长度的中间数组
        private readonly byte[] TempByteArray = new byte[MaxLength];

        //当前数组长度
        private int CurrentLength = 0;

        //当前Pop指针位置
        private int CurrentPosition = 0;

        //最后返回数组
        private byte[] ReturnArray;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ByteBuffer()
        {
            Initialize();
        }

        /// <summary>
        /// 重载的构造函数,用一个Byte数组来构造
        /// </summary>
        /// <param name="bytes">用于构造ByteBuffer的数组</param>
        public ByteBuffer(byte[] bytes)
        {
            Initialize();
            PushByteArray(bytes);
        }


        /// <summary>
                /// 获取当前ByteBuffer的长度
                /// </summary>
        public int Length => CurrentLength;

        /// <summary>
        /// 获取/设置当前出栈指针位置
        /// </summary>
        public int Position
        {
            get => CurrentPosition;
            set => CurrentPosition = value;
        }

        /// <summary>
        /// 获取ByteBuffer所生成的数组
        /// 长度必须小于 [MAXSIZE]
        /// </summary>
        /// <returns>Byte[]</returns>
        public byte[] ToByteArray()
        {
            //分配大小
            ReturnArray = new byte[CurrentLength];
            //调整指针
            Array.Copy(TempByteArray, 0, ReturnArray, 0, CurrentLength);
            return ReturnArray;
        }

        /// <summary>
        /// 初始化ByteBuffer的每一个元素,并把当前指针指向头一位
        /// </summary>
        public void Initialize()
        {
            TempByteArray.Initialize();
            CurrentLength = 0;
            CurrentPosition = 0;
        }

        /// <summary>
        /// 向ByteBuffer压入一个字节
        /// </summary>
        /// <param name="by">一位字节</param>
        public void PushByte(byte by)
        {
            TempByteArray[CurrentLength++] = by;
        }

        /// <summary>
        /// 向ByteBuffer压入数组
        /// </summary>
        /// <param name="byteArray">数组</param>
        public void PushByteArray(byte[] byteArray)
        {
            //把自己CopyTo目标数组
            byteArray.CopyTo(TempByteArray, CurrentLength);
            //调整长度
            CurrentLength += byteArray.Length;
        }

        /// <summary>
        /// 向ByteBuffer压入两字节的Short
        /// </summary>
        /// <param name="num">2字节Short</param>
        public void PushUInt16(ushort num)
        {
            TempByteArray[CurrentLength++] = (byte)((num & 0x00ff) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0xff00) >> 8) & 0xff);
        }

        /// <summary>
        /// 向ByteBuffer压入一个无符Int值
        /// </summary>
        /// <param name="num">4字节UInt32</param>
        public void PushInt(uint num)
        {
            TempByteArray[CurrentLength++] = (byte)((num & 0x000000ff) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0x0000ff00) >> 8) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0x00ff0000) >> 16) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0xff000000) >> 24) & 0xff);
        }

        /// <summary>
        /// 向ByteBuffer压入一个Long值
        /// </summary>
        /// <param name="num">4字节Long</param>
        public void PushLong(long num)
        {
            TempByteArray[CurrentLength++] = (byte)((num & 0x000000ff) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0x0000ff00) >> 8) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0x00ff0000) >> 16) & 0xff);
            TempByteArray[CurrentLength++] = (byte)(((num & 0xff000000) >> 24) & 0xff);
        }

        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个Byte,并提升一位
        /// </summary>
        /// <returns>1字节Byte</returns>
        public byte PopByte()
        {
            byte ret = TempByteArray[CurrentPosition++];
            return ret;
        }

        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个Short,并提升两位
        /// </summary>
        /// <returns>2字节Short</returns>
        public ushort PopUInt16()
        {
            //溢出
            if (CurrentPosition + 1 >= CurrentLength)
            {
                return 0;
            }
            //UInt16 ret = (UInt16)(TEMP_BYTE_ARRAY[CURRENT_POSITION] << 8 | TEMP_BYTE_ARRAY[CURRENT_POSITION + 1]);
            ushort ret = (ushort)(TempByteArray[CurrentPosition] | TempByteArray[CurrentPosition + 1] << 8);
            CurrentPosition += 2;
            return ret;
        }

        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个uint,并提升4位
        /// </summary>
        /// <returns>4字节UInt</returns>
        public uint PopUInt()
        {
            if (CurrentPosition + 3 >= CurrentLength)
                return 0;
            uint ret = (uint)(TempByteArray[CurrentPosition] | TempByteArray[CurrentPosition + 1] << 8 | TempByteArray[CurrentPosition + 2] << 16 | TempByteArray[CurrentPosition + 3] << 24);
            CurrentPosition += 4;
            return ret;
        }

        /// <summary>
        /// 从ByteBuffer的当前位置弹出一个long,并提升4位
        /// </summary>
        /// <returns>4字节Long</returns>
        public long PopLong()
        {
            if (CurrentPosition + 3 >= CurrentLength)
                return 0;
            long ret = (long)(TempByteArray[CurrentPosition] << 24 | TempByteArray[CurrentPosition + 1] << 16 | TempByteArray[CurrentPosition + 2] << 8 | TempByteArray[CurrentPosition + 3]);
            CurrentPosition += 4;
            return ret;
        }

        /// <summary>
        /// 从ByteBuffer的当前位置弹出长度为Length的Byte数组,提升Length位
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <returns>Length长度的byte数组</returns>
        public byte[] PopByteArray(int length)
        {
            //溢出
            if (CurrentPosition + length > CurrentLength)
            {
                return new byte[0];
            }
            byte[] ret = new byte[length];
            Array.Copy(TempByteArray, CurrentPosition, ret, 0, length);
            //提升位置
            CurrentPosition += length;
            return ret;
        }

        public byte[] PopByteArray2(int length)
        {
            //溢出
            if (CurrentPosition <= length)
            {
                return new byte[0];
            }
            byte[] ret = new byte[length];
            Array.Copy(TempByteArray, CurrentPosition - length, ret, 0, length);
            //提升位置
            CurrentPosition -= length;
            return ret;
        }

    }
}
