using EntityComponentState;
using EntityComponentState.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StateToScene
{
    public static void SpawnEntities(State state, List<StateClone> clones, Transform parent = null)
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

            newGameObject.transform.SetParent(parent);
            newGameObject.layer = LayerMask.NameToLayer("AnotherPhysicsWorld");
            clones.Add(new StateClone { gameObject = newGameObject, entityId = spawn.id });
        }
    }

    public static void DespawnEntities(State state, List<StateClone> clones)
    {
        var despawns = clones.Where(clone => !state.entities.Any(entity => clone.entityId == entity.id));
        for(int i = despawns.Count() - 1; i >= 0; i--)
        {
            var despawn = despawns.ElementAt(i);
            clones.Remove(despawn);
            Object.Destroy(despawn.gameObject);
        }
    }

    public static void ApplyChangesToEntites(State state, List<StateClone> clones)
    {
        foreach (var entity in state.entities)
        {
            var clone = clones.First(c => c.entityId == entity.id);
            foreach (var component in entity.components)
                component.Apply(clone.gameObject);
        }
    }
}
