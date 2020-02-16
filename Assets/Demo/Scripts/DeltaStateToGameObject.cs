using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityComponentState.Unity
{
    public class DeltaStateToGameObject : MonoBehaviour
    {
        public StateMB stateMB;
        public List<StateClone> clones = new List<StateClone>();

        public DeltaState DeltaState => stateMB != null ? stateMB.deltaState : null;

        private void Update()
        {
            if (DeltaState != null)
            {
                SpawnEntities();
                DespawnEntities();
                ApplyChangesToEntites();
            }
        }

        private void SpawnEntities()
        {
            var spawns = DeltaState.spawns;
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
            foreach (var despawn in DeltaState.despawns)
            {
                var clone = clones.FirstOrDefault(c => c.entityId == despawn.id);
                if (clone == null) continue;
                clones.Remove(clone);
                Destroy(clone.gameObject);
            }
        }

        private void ApplyChangesToEntites()
        {
            foreach (var entity in DeltaState.endState.entities)
            {
                var clone = clones.FirstOrDefault(c => c.entityId == entity.id);
                if (clone == null) continue;
                foreach (var component in entity.components)
                    component.Apply(clone.gameObject);
            }
        }
    }
}