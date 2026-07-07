using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonstersDataPath = "StaticData/Monsters";
        private const string LevelsDataPath = "StaticData/Levels";
        
        private Dictionary<MonsterTypeID, MonsterStaticData> _monstersData;
        private Dictionary<string, LevelStaticData> _levelsData;

        public void LoadData()
        {
            _monstersData = Resources.LoadAll<MonsterStaticData>(MonstersDataPath)
                .ToDictionary(i => i.Type, i => i);
            _levelsData = Resources.LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(i => i.LevelKey, i => i);
        }

        public MonsterStaticData GetData(MonsterTypeID type) => _monstersData.GetValueOrDefault(type);

        public LevelStaticData GetLevelData(string sceneKey) => _levelsData.GetValueOrDefault(sceneKey);
    }
}