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

                var state1 = State.Create(1, entities);

                ship.GetComponent<Position>().X = 5;
                entities.Add(asteroid3);

                var state2 = State.Create(2, entities);

                entities.Remove(asteroid2);

                var state3 = State.Create(3, entities);

                entities.Add(bullet);

                var state4 = State.Create(4, entities);

                ConsoleWriteState(state1);
                ConsoleWriteState(state2);
                ConsoleWriteState(state3);
                ConsoleWriteState(state4);

                var deltaState1 = new DeltaState(state1, state2);
                var deltaState2 = new DeltaState(state2, state3);
                var deltaState3 = new DeltaState(state3, state4);
                var deltaState4 = new DeltaState(state1, state4);

                ConsoleWriteState(deltaState1);
                ConsoleWriteState(deltaState2);
                ConsoleWriteState(deltaState3);
                ConsoleWriteState(deltaState4);

                var newState2 = deltaState1.Apply(state1);

                ConsoleWriteState(newState2);

                Console.WriteLine($"Time to create and show state: {stopwatch.Elapsed.TotalMilliseconds}");

                input = Console.ReadKey().KeyChar.ToString().ToLower();
            }
        }

        private static void ConsoleWriteState(State state)
        {
            Console.WriteLine(state);
            Console.WriteLine($"Bytes: {state.ToByteHexString()}");
        }

        private static void ConsoleWriteState(DeltaState deltaState)
        {
            Console.WriteLine(deltaState);
            Console.WriteLine($"Bytes: {deltaState.ToByteHexString()}");
        }
    }
}
