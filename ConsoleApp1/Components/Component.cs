namespace ConsoleApp1
{
    public abstract class Component
    {       
        public Entity entity = Entity.NULL;

        public abstract Component Clone();
    }
}
