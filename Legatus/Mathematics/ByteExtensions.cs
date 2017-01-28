

namespace Legatus.Mathematics
{
    public static class ByteExtensions
    {
        public static void SetFlag(this byte flags, byte flag, bool value)
        {
            if (value)
                flags |= flag;
            else
                flags &= (byte)~flag;
        }

        public static bool GetFlag(this byte flags, byte flag)
        {
            return ((flags & flag) == flag);
        }
    }
}
