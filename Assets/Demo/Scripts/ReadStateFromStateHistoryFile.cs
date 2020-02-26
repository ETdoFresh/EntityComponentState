using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static TransformStateCompressed;

public class ReadStateFromStateHistoryFile : MonoBehaviour
{
    public int countPosition;
    public int count;
    public TransformState state = new TransformState();
    public List<StateClone> clones = new List<StateClone>();
    private FileStream stateHistoryFile;
    public bool isPlaying = true;
    public bool isLive = false;

    private void OnEnable()
    {
        stateHistoryFile = File.Open(WriteStateHistoryToFile.FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void OnDisable()
    {
        stateHistoryFile.Close();
    }

    private void FixedUpdate()
    {
        try
        {
            var bytes = new byte[stateHistoryFile.Length];
            stateHistoryFile.Position = 0;
            stateHistoryFile.Read(bytes, 0, (int)stateHistoryFile.Length);
            count = StateHistory.GetCountFromBytes(new ByteQueue(bytes));
            if (count > 0)
            {
                if (isLive && isPlaying)
                    countPosition = count - 1;
                else
                    countPosition = Math.Min(countPosition, count - 1);
                state = StateHistory.GetStateFromBytes<TransformState>(new ByteQueue(bytes), countPosition);
                SpawnEntities();
                DespawnEntities();
                ApplyChangesToEntites();
            }
        }
        catch
        {
        }
        if (isPlaying)
            countPosition++;
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