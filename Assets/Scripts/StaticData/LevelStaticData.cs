using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Levels")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public List<EnemySpawnerData> EnemySpawnerDatas;
    }
}