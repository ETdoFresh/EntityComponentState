﻿using System;

namespace EntityComponentState
{
    public class Position : Vector3Component
    {
        public Position() { }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
