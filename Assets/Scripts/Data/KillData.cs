using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class KillData
    {
        public List<string> clearedSpawnerIds = new List<string>();

        public void AddSafe(string id)
        {
            if (clearedSpawnerIds.Contains(id)) return;
            
            clearedSpawnerIds.Add(id);
        }
    }
}