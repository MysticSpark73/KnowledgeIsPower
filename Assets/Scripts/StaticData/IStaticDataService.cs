using Infrastructure.Services;

namespace StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadData();
        MonsterStaticData GetData(MonsterTypeID type);
        LevelStaticData GetLevelData(string sceneKey);
    }
}