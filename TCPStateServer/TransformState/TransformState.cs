using EntityComponentState;
using System;
using System.Collections.Generic;

namespace TransformStateLibrary
{
    public class TransformState : CompressedState
    {
        public static readonly Type[] TYPES = new[]
        {
            typeof(CompressedPosition),
            typeof(CompressedRotation),
            typeof(CompressedScale),
            typeof(Name),
            typeof(Primitive)
        };

        public override IEnumerable<Type> componentTypes { get; } = TYPES;
    }
}
