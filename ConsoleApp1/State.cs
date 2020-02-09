using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class State
    {
        public int tick;
        public List<Entity> entities = new List<Entity>();

        public static State Create(List<Entity> entities)
        {
            var newState = new State();
            foreach (var entity in entities.OrderBy(e => e.id))
                newState.entities.Add(entity.Clone());
            return newState;
        }

        private int GetCount<T>() where T: Component
        {
            var count = 0;
            foreach (var entity in entities)
                if (entity.HasComponent<T>())
                    count++;
            return count;
        }

        private IEnumerable<T> GetComponents<T>() where T : Component
        {
            foreach (var entity in entities)
                if (entity.HasComponent<T>())
                    yield return entity.GetComponent<T>();
        }

        public override string ToString()
        {
            var output = $"State [Tick: {tick}]\r\n";
            output += $"  Positions [Count: {GetCount<Position>()}]\r\n";
            foreach (var position in GetComponents<Position>())
                output += $"    {position}\r\n";

            output += $"  Rotations [Count: {GetCount<Rotation>()}]\r\n";
            foreach (var rotation in GetComponents<Rotation>())
                output += $"    {rotation}\r\n";

            output += $"  Scales [Count: {GetCount<Scale>()}]\r\n";
            foreach (var scale in GetComponents<Scale>())
                output += $"    {scale}\r\n";

            output += $"  Velocities [Count: {GetCount<Velocity>()}]\r\n";
            foreach (var velocity in GetComponents<Velocity>())
                output += $"    {velocity}\r\n";

            output += $"  AngularVelocities [Count: {GetCount<AngularVelocity>()}]\r\n";
            foreach (var angularVelocity in GetComponents<AngularVelocity>())
                output += $"    {angularVelocity}\r\n";

            output += $"  Sprites [Count: {GetCount<Sprite>()}]\r\n";
            foreach (var sprite in GetComponents<Sprite>())
                output += $"    {sprite}\r\n";

            output += $"  AnimationFrames [Count: {GetCount<AnimationFrame>()}]\r\n";
            foreach (var animationFrame in GetComponents<AnimationFrame>())
                output += $"    {animationFrame}\r\n";

            return output;
        }

        public string ToBytes()
        {
            var output = "";
            var count = 0;
            foreach (var bite in BitConverter.GetBytes(tick))
            { output += $"{bite:x2}"; count++; }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<Position>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var position in GetComponents<Position>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(position.X))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(position.Y))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(position.Z))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<Rotation>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var rotation in GetComponents<Rotation>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(rotation.X))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(rotation.Y))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(rotation.Z))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(rotation.W))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<Scale>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var scale in GetComponents<Scale>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(scale.X))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(scale.Y))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(scale.Z))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<Velocity>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var velocity in GetComponents<Velocity>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(velocity.X))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(velocity.Y))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(velocity.Z))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<AngularVelocity>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var angularVelocity in GetComponents<AngularVelocity>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(angularVelocity.X))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(angularVelocity.Y))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in BitConverter.GetBytes(angularVelocity.Z))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<Sprite>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var sprite in GetComponents<Sprite>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(sprite.spriteName.Length))
                { output += $"{bite:x2}"; count++; }
                output += " ";
                foreach (var bite in Encoding.UTF8.GetBytes(sprite.spriteName))
                { output += $"{bite:x2}"; count++; }
            }

            output += " ";
            foreach (var bite in BitConverter.GetBytes(GetCount<AnimationFrame>()))
            { output += $"{bite:x2}"; count++; }
            foreach (var animationFrame in GetComponents<AnimationFrame>())
            {
                output += " ";
                foreach (var bite in BitConverter.GetBytes(animationFrame.frame))
                { output += $"{bite:x2}"; count++; }
            }

            return $"{output} [{count}]";
        }
    }
}
