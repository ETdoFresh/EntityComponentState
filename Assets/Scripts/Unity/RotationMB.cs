﻿using UnityEngine;

namespace EntityComponentState.Unity
{
    public class RotationMB : ComponentMB<Rotation>
    {
        public Quaternion value;

        private void Update()
        {
            value.x = component.X = transform.rotation.x;
            value.y = component.Y = transform.rotation.y;
            value.z = component.Z = transform.rotation.z;
            value.w = component.W = transform.rotation.w;
        }
    }
}