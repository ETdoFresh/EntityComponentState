using EntityComponentState;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        public static string statePath = "state.txt";

        static void Main()
        {
            var allEntities = new List<Entity>();
            var stopwatch = new Stopwatch();
            foreach (var entity in allEntities)
                entity.Destroy();

            allEntities.Clear();
            stopwatch.Reset();
            stopwatch.Start();

            var ship = new Entity();
            ship.AddComponent(new Position());

            var asteroid1 = new Entity();
            asteroid1.AddComponent(new Position(), new Rotation(), new Scale());

            var asteroid2 = new Entity();
            asteroid2.AddComponent(new Position(), new Rotation(), new Scale());

            var asteroid3 = new Entity();
            asteroid3.AddComponent(new Position(), new Rotation(), new Scale());

            var bullet = new Entity();
            bullet.AddComponent(new Position(), new Scale());

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

            var newState2 = deltaState1.GenerateEndState();

            ConsoleWriteState(newState2);

            Console.WriteLine($"Time to create and show state: {stopwatch.Elapsed.TotalMilliseconds}");
        }

        private static void ConsoleWriteState(State state)
        {
            Console.WriteLine(state);
            Console.WriteLine($"Bytes: {state.ToByteHexString()}");
            Console.WriteLine($"Compressed Bytes: {state.ToCompressedByteHexString()}");
            Console.WriteLine($"Base64: {Convert.ToBase64String(state.ToBytes())} [{Convert.ToBase64String(state.ToBytes()).Length}]");
            Console.WriteLine($"Compressed Base64: {Convert.ToBase64String(state.ToCompressedBytes())} [{Convert.ToBase64String(state.ToCompressedBytes()).Length}]");
        }

        private static void ConsoleWriteState(DeltaState deltaState)
        {
            Console.WriteLine(deltaState);
            Console.WriteLine($"Bytes: {deltaState.ToByteHexString()}");
            Console.WriteLine($"Compressed Bytes: {deltaState.ToCompressedByteHexString()}");
            Console.WriteLine($"Base64: {Convert.ToBase64String(deltaState.ToBytes())} [{Convert.ToBase64String(deltaState.ToBytes()).Length}]");
            Console.WriteLine($"Compressed Base64: {Convert.ToBase64String(deltaState.ToCompressedBytes())} [{Convert.ToBase64String(deltaState.ToCompressedBytes()).Length}]");
        }
    }
}
