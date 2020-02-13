namespace EntityComponentState
{
    public struct Vector4
    {
        public float x, y, z, w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector4 other)
            {
                if (this.x != other.x) return false;
                if (this.y != other.y) return false;
                if (this.z != other.z) return false;
                if (this.w != other.w) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)x + (int)y * 100 + (int)z * 1000 + (int)w * 10000;
        }

        public static bool operator ==(Vector4 lhs, Vector4 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector4 lhs, Vector4 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
