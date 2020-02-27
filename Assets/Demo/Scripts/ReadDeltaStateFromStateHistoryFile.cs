using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using static TransformStateCompressed;

public class ReadDeltaStateFromStateHistoryFile : MonoBehaviour
{
    public int tick;
    public int count;
    public TransformState state = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    private FileStream stateHistoryFile;
    public bool isPlaying = true;
    public byte[] bytes = new byte[0];
    public DeltaStateHistory<TransformDeltaState> deltaStateHistory = new DeltaStateHistory<TransformDeltaState>();

    private void OnEnable()
    {
        stateHistoryFile = File.Open(WriteDeltaStateHistoryToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void OnDisable()
    {
        stateHistoryFile.Close();
    }

    private void FixedUpdate()
    {
        if (bytes.Length != stateHistoryFile.Length)
        {
            bytes = new byte[stateHistoryFile.Length];
            stateHistoryFile.Position = 0;
            stateHistoryFile.Read(bytes, 0, (int)stateHistoryFile.Length);
            deltaStateHistory.FromBytes(new ByteQueue(bytes));
            count = deltaStateHistory.LatestTick;
        }
        if (count > 0 && isPlaying)
        {
            var deltaState = deltaStateHistory.GetDeltaState(state.tick);
            if (deltaState != null)
            {
                state = (TransformState)deltaState.GenerateEndState(state);
                SpawnEntities();
                DespawnEntities();
                ApplyChangesToEntites();
            }
            tick = state.tick;
        }
    }

    private void SpawnEntities()
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

    private void DespawnEntities()
    {
        var despawns = clones.Where(clone => !state.entities.Any(entity => clone.entityId == entity.id));
        foreach (var despawn in despawns)
        {
            clones.Remove(despawn);
            Destroy(despawn.gameObject);
        }
    }

    private void ApplyChangesToEntites()
    {
        foreach (var entity in state.entities)
        {
            var clone = clones.First(c => c.entityId == entity.id);
            foreach (var component in entity.components)
                component.Apply(clone.gameObject);
        }
    }
}