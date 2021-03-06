﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class StateToGameObject : MonoBehaviour
    {
        public StateMB stateMB;
        public List<StateClone> clones = new List<StateClone>();

        public State State => stateMB != null ? stateMB.state : null;

        private void Update()
        {
            if (State != null)
            {
                SpawnEntities();
                DespawnEntities();
                ApplyChangesToEntites();
            }
        }

        private void SpawnEntities()
        {
            var spawns = State.entities.Where(entity => !clones.Any(clone => clone.entityId == entity.id));
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
            var despawns = clones.Where(clone => !State.entities.Any(entity => clone.entityId == entity.id));
            foreach (var despawn in despawns)
            {
                clones.Remove(despawn);
                Destroy(despawn.gameObject);
            }
        }

        private void ApplyChangesToEntites()
        {
            foreach (var entity in State.entities)
            {
                var clone = clones.First(c => c.entityId == entity.id);
                foreach (var component in entity.components)
                    component.Apply(clone.gameObject);
            }
        }
    }
}