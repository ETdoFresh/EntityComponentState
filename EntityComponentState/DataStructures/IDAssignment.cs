using System;
using System.Collections.Generic;

namespace EntityComponentState
{
    public static class IDAssignment
    {
        private static readonly Dictionary<int, string> ids = new Dictionary<int, string>();

        public static int GetID(string description)
        {
            for(int id = 0; id < 1000; id++)
                if (!ids.ContainsKey(id))
                {
                    ids.Add(id, description);
                    return id;
                }
            throw new InvalidOperationException("Too many objects assigned an ID");
        }

        public static void ReleaseID(int id)
        {
            ids.Remove(id);
        }

        public static void Clear()
        {
            ids.Clear();
        }
    }
}
