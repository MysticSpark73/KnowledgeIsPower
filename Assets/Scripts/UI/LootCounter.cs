using Data;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LootCounter : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private TextMeshProUGUI _lootText;
        private WorldData _worldData;
        
        public void LoadProgress(PlayerProgress playerProgress)
        {
            _worldData = playerProgress.WorldData;
            OnLootValueChanged();
            playerProgress.WorldData.LootData.OnValueChanged += OnLootValueChanged;
        }

        private void OnDestroy()
        {
            _worldData.LootData.OnValueChanged -= OnLootValueChanged;
        }

        private void OnLootValueChanged() => _lootText.text = _worldData.LootData.Score.ToString();
    }
}