using EntityComponentState;
using System;
using System.Collections.Generic;

namespace TransformStateLibrary
{
    public class TransformDeltaState : CompressedDeltaState
    {
        public override IEnumerable<Type> componentTypes => TransformState.TYPES;
    }
}
