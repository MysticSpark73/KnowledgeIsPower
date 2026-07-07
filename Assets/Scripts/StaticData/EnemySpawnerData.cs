using System;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;

        public MonsterTypeID MonsterTypeID;

        public Vector3 Position;

        public EnemySpawnerData(string id, MonsterTypeID monsterTypeID, Vector3 position)
        {
            Id = id;
            MonsterTypeID = monsterTypeID;
            Position = position;
        }
    }
}