namespace ConsoleApp1
{
    public abstract class Component
    {       
        public Entity entity = Entity.NULL;

        public abstract Component Clone();

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is Component other)
                return this.entity.id == other.entity.id;
            else
                return false;
        }

        public static bool operator ==(Component lhs, Component rhs)
        {
            if (lhs is null)
                return rhs is null;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator !=(Component lhs, Component rhs)
            => !(lhs == rhs);

        public override int GetHashCode()
        {
            return entity.id;
        }
    }
}
