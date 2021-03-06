﻿using UnityEngine;

namespace EntityComponentState.Unity
{
    public class EntityMB : MonoBehaviour
    {
        public Entity entity;
        public int id;

        private void OnEnable()
        {
            if (entity == null)
            {
                entity = new Entity(IDAssignment.GetID(typeof(Entity).FullName));
                id = entity.id;
            }
        }

        private void OnDisable()
        {
            entity.Destroy();
            entity = null;
        }
    }
}