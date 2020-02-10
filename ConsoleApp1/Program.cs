using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        public static State state = new State();

        static void Main()
        {
            var allEntities = new List<Entity>();
            var stopwatch = new Stopwatch();
            Console.WriteLine("Press a key to run code");
            var input = Console.ReadKey().KeyChar.ToString().ToLower();
            while (input != "q")
            {
                foreach (var entity in allEntities)
                    entity.Destroy();

                allEntities.Clear();
                stopwatch.Reset();
                stopwatch.Start();

                var ship = new Entity();
                ship.AddComponents(new Position());

                var asteroid1 = new Entity();
                asteroid1.AddComponents(new Position(), new Rotation(), new Scale());

                var asteroid2 = new Entity();
                asteroid2.AddComponents(new Position(), new Rotation(), new Scale());
                
                var asteroid3 = new Entity();
                asteroid3.AddComponents(new Position(), new Rotation(), new Scale());

                var bullet = new Entity();
                bullet.AddComponents(new Position(), new Scale());

                allEntities.AddRange(new[] { ship, asteroid1, asteroid2, asteroid3, bullet });

                var entities = new List<Entity> { ship, asteroid1, asteroid2 };

                var state1 = State.Create(entities);

                entities.Add(asteroid3);

                var state2 = State.Create(entities);

                entities.Remove(asteroid2);

                var state3 = State.Create(entities);

                entities.Add(bullet);

                var state4 = State.Create(entities);

                ConsoleWriteState(state1);
                ConsoleWriteState(state2);
                ConsoleWriteState(state3);
                ConsoleWriteState(state4);

                Console.WriteLine($"Time to create and show state: {stopwatch.Elapsed.TotalMilliseconds}");

                input = Console.ReadKey().KeyChar.ToString().ToLower();
            }
        }

        private static void ConsoleWriteState(State state)
        {
            Console.WriteLine(state);
            Console.WriteLine($"Bytes: {state.ToBytes()}");
        }

    }
}
