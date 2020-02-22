using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentState
{
    public interface IToBytes
    {
        ByteQueue ToBytes();
        void FromBytes(ByteQueue bytes);
    }

    public static class IToBytesTypes
    {
        private static Type Type => typeof(IToBytes);
        private static IEnumerable<Type> allTypes;
        public static IEnumerable<Type> ALL
        {
            get
            {
                if (allTypes == null)
                    return (allTypes = Type.Assembly.GetTypes().Where(otherType => Type.IsAssignableFrom(otherType)));
                else
                    return allTypes;
            }
        }

        public static byte IndexOf(Type type)
        {
            byte i = 0;
            foreach (var otherType in ALL)
                if (type == otherType)
                    return i;
                else
                    i++;
            throw new ArgumentException("Type doesn't exist in IToBytesTypes");
        }
    }
}