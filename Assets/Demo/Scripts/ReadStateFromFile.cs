using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static EntityComponentState.Constants;

[RequireComponent(typeof(StateMB))]
public class ReadStateFromFile : MonoBehaviour
{
    public StateMB stateMB;
    public List<StateClone> clones = new List<StateClone>();
    public ByteQueue byteQueue = new ByteQueue();
    private FileStream file;

    private void OnEnable()
    {
        if (!stateMB) stateMB = GetComponent<StateMB>();
        file = File.Open(STATE_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private void OnDisable()
    {
        file.Close();
    }

    private void Update()
    {
        try
        {
            var bytes = new byte[file.Length];
            file.Position = 0;
            file.Read(bytes, 0, (int)file.Length);
            stateMB.state.FromBytes(new ByteQueue(bytes));
            SpawnEntities();
            DespawnEntities();
            ApplyChangesToEntites();
        }
        catch
        {
        }
    }

    private void SpawnEntities()
    {
        var spawns = stateMB.state.entities.Where(entity => !clones.Any(clone => clone.entityId == entity.id));
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
        var despawns = clones.Where(clone => !stateMB.state.entities.Any(entity => clone.entityId == entity.id));
        foreach (var despawn in despawns)
        {
            clones.Remove(despawn);
            Destroy(despawn.gameObject);
        }
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
