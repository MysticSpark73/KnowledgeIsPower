using System.Collections.Generic;
using System.Linq;
using StaticData.Windows;
using UI.Services.Windows;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonstersDataPath = "StaticData/Monsters";
        private const string LevelsDataPath = "StaticData/Levels";
        private const string WindowsDataPath = "StaticData/Windows/WindowsData";
        
        private Dictionary<MonsterTypeID, MonsterStaticData> _monstersData;
        private Dictionary<string, LevelStaticData> _levelsData;
        private Dictionary<WindowType, WindowConfig> _windowsData;

        public void LoadData()
        {
            _monstersData = Resources.LoadAll<MonsterStaticData>(MonstersDataPath)
                .ToDictionary(i => i.Type, i => i);
            _levelsData = Resources.LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(i => i.LevelKey, i => i);
            _windowsData = Resources.Load<WindowStaticData>(WindowsDataPath).Configs.
                ToDictionary(i => i.Type, i => i);
        }

        public MonsterStaticData GetMonsterData(MonsterTypeID type) => _monstersData.GetValueOrDefault(type);

        public LevelStaticData GetLevelData(string sceneKey) => _levelsData.GetValueOrDefault(sceneKey);

        public WindowConfig GetWindowsData(WindowType windowType) => _windowsData.GetValueOrDefault(windowType);
    }
}