using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StateMB))]
public class ReadDeltaStateFromFile : MonoBehaviour
{
    public StateMB stateMB;
    public List<StateClone> clones = new List<StateClone>();
    private FileStream stateFile;
    private FileStream deltaStateFile;
    public TransformStateCompressed.TransformDeltaState deltaState = new TransformStateCompressed.TransformDeltaState();

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
        stateFile = File.Open(WriteStateToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var bytes = new byte[stateFile.Length];
        stateFile.Position = 0;
        stateFile.Read(bytes, 0, (int)stateFile.Length);
        stateMB.FromBytes(new ByteQueue(bytes));
        deltaStateFile = File.Open(WriteDeltaStateToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void OnDisable()
    {
        stateFile.Close();
        deltaStateFile.Close();
    }

    private void Update()
    {
        //try
        //{
        var startState = stateMB.state;
        var bytes = new byte[deltaStateFile.Length];
        deltaStateFile.Position = 0;
        deltaStateFile.Read(bytes, 0, (int)deltaStateFile.Length);
        deltaState.FromBytes(new ByteQueue(bytes), startState);
        ((TransformStateCompressed)stateMB).deltaStateString = deltaState.ToString();
        ((TransformStateCompressed)stateMB).deltaStateBytes = $"{deltaState.ToBytes().ToHexString()} [{deltaState.ToString()}]";

        if (deltaState.startState.tick == startState.tick)
        {
            SpawnEntities(deltaState.endState);
            DespawnEntities(deltaState.endState);
            ApplyChangesToEntites(deltaState.endState);
        }
        else if (deltaState.startState.tick > startState.tick)
        {
            
            SpawnEntities(stateMB.state);
            DespawnEntities(stateMB.state);
            ApplyChangesToEntites(stateMB.state);
        }
        else // deltaState.startState.tick < startState.tick
        { }
        //}
        //catch
        //{
        //}
    }

    private void SpawnEntities(State state)
    {
        var spawns = state.entities.Where(entity => !clones.Any(clone => clone.entityId == entity.id));
        foreach (var spawn in spawns)
        {
            GameObject newGameObject = null;
            if (spawn.HasComponent<Primitive>())
            {
                var primitive = spawn.GetComponent<Primitive>();
                if (primitive.primitiveType == Primitive.PrimitiveType.Capsule)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                else if (primitive.primitiveType == Primitive.PrimitiveType.Cube)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                else if (primitive.primitiveType == Primitive.PrimitiveType.Cylinder)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                else if (primitive.primitiveType == Primitive.PrimitiveType.Plane)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                else if (primitive.primitiveType == Primitive.PrimitiveType.Quad)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                else if (primitive.primitiveType == Primitive.PrimitiveType.Sphere)
                    newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
            if (!newGameObject)
                newGameObject = new GameObject();

            newGameObject.transform.SetParent(transform);
            newGameObject.layer = LayerMask.NameToLayer("AnotherPhysicsWorld");
            clones.Add(new StateClone { gameObject = newGameObject, entityId = spawn.id });
        }
    }

    private void DespawnEntities(State state)
    {
        var despawns = clones.Where(clone => !state.entities.Any(entity => entity.id == clone.entityId));
        foreach (var despawn in despawns)
            Destroy(despawn.gameObject);
    }

    private void ApplyChangesToEntites(State state)
    {
        foreach (var entity in state.entities)
        {
            var clone = clones.First(c => c.entityId == entity.id);
            foreach (var component in entity.components)
                component.Apply(clone.gameObject);
        }
    }
}