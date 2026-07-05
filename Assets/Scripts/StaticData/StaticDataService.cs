using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonstersDataPath = "StaticData/Monsters";
        
        private Dictionary<MonsterTypeID, MonsterStaticData> _monstersData;

        public void LoadData()
        {
            _monstersData = Resources.LoadAll<MonsterStaticData>(MonstersDataPath).ToDictionary(i => i.Type, i => i);
        }

        public MonsterStaticData GetData(MonsterTypeID type) => 
            _monstersData.TryGetValue(type, out MonsterStaticData data) ? data : null;
    }
}