using Infrastructure.Services;
using StaticData.Windows;
using UI.Services.Windows;

namespace StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadData();
        MonsterStaticData GetMonsterData(MonsterTypeID type);
        LevelStaticData GetLevelData(string sceneKey);
        WindowConfig GetWindowsData(WindowType windowType);
    }
}