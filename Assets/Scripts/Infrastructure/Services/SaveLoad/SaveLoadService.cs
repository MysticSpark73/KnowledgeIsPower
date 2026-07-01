using Data;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PlayerProgressKey = "PlayerProgress";
        
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _factory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory factory)
        {
            _progressService = progressService;
            _factory = factory;
        }

        public void SaveProgress()
        {
            foreach (var progressWriter in _factory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_progressService.PlayerProgress);
            }
            
            PlayerPrefs.SetString(PlayerProgressKey, _progressService.PlayerProgress.ToJSON());
        }

        public PlayerProgress LoadProgress() => PlayerPrefs.GetString(PlayerProgressKey)?.Deserialize<PlayerProgress>();
    }
}