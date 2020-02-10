namespace ConsoleApp1
{
    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3 other)
            {
                if (this.x != other.x) return false;
                if (this.y != other.y) return false;
                if (this.z != other.z) return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)x + (int)y * 100 + (int)z * 1000;
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {            
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
