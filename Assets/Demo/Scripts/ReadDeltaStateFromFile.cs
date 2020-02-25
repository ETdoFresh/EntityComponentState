﻿using EntityComponentState;
using EntityComponentState.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AState))]
public class ReadDeltaStateFromFile : MonoBehaviour
{
    public AState stateMB;
    public DeltaState deltaStateMB;
    public List<StateClone> clones = new List<StateClone>();
    public ByteQueue byteQueue = new ByteQueue();
    private FileStream stateFile;
    private FileStream deltaStateFile;
    public TransformStateCompressed.TransformDeltaState deltaState = new TransformStateCompressed.TransformDeltaState();
    private State empty = new TransformStateCompressed.TransformState();

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<AState>();
        stateFile = File.Open(WriteStateToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        deltaStateFile = File.Open(WriteDeltaStateToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void OnDisable()
    {
        stateFile.Close();
        deltaStateFile.Close();
    }

    private void Update()
    {
        try
        {
            if (stateMB.state == empty) throw new Exception("Empty State");

            var bytes = new byte[deltaStateFile.Length];
            deltaStateFile.Position = 0;
            deltaStateFile.Read(bytes, 0, (int)deltaStateFile.Length);
            deltaState.FromBytes(new ByteQueue(bytes), stateMB.state);
        }
        catch // if cannot read or previousState not yet set
        {
            deltaState.Clear();
            var bytes = new byte[stateFile.Length];
            stateFile.Position = 0;
            stateFile.Read(bytes, 0, (int)stateFile.Length);
            stateMB.FromBytes(new ByteQueue(bytes));
        }
        finally
        {
            SpawnEntities();
            DespawnEntities();
            ApplyChangesToEntites();

        }
    }

    private void SpawnEntities()
    {
        var state = deltaState != null && deltaState.endState != empty ? deltaState.endState : stateMB.state;
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

    private void DespawnEntities()
    {
        var state = deltaState != null && deltaState.endState != empty ? deltaState.endState : stateMB.state;
        var a = state.entities.Any(e => e.id == 1);
        var despawns = clones.Where(clone => !state.entities.Any(entity => entity.id == clone.entityId));
        foreach (var despawn in despawns)
            Destroy(despawn.gameObject);
    }

    private void ApplyChangesToEntites()
    {
        foreach (var entity in stateMB.state.entities)
        {
            var clone = clones.First(c => c.entityId == entity.id);
            foreach (var component in entity.components)
                component.Apply(clone.gameObject);
        }
    }
}