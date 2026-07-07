using StaticData;
using UnityEngine;

namespace Logic.EnemySpawners
{
    public class EnemySpawnMarker : MonoBehaviour
    {
        [SerializeField] private UniqueID _uniqueID;
        [SerializeField] private MonsterTypeID _monsterType;

        public (string id, MonsterTypeID monsterType) GetData() => (_uniqueID.ID, _monsterType);
    }
}