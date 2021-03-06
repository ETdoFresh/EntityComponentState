﻿using UnityVector3 = UnityEngine.Vector3;

namespace EntityComponentState.Unity
{
    public class ScaleMB : ComponentMB<Scale>
    {
        public UnityVector3 value;

        private void Update()
        {
            value.x = component.X = transform.localScale.x;
            value.y = component.Y = transform.localScale.y;
            value.z = component.Z = transform.localScale.z;
        }
    }
}