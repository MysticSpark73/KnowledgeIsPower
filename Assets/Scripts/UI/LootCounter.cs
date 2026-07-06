using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lootText;
        
        private WorldData _worldData;

        private void Start()
        {
            OnLootValueChanged();
        }

        private void OnDestroy()
        {
            
            _worldData.LootData.OnValueChanged -= OnLootValueChanged;
        }

        public void Initialize(WorldData worldData)
        {
            _worldData = worldData;
            worldData.LootData.OnValueChanged += OnLootValueChanged;
        }

        private void OnLootValueChanged() => _lootText.text = _worldData.LootData.Score.ToString();
    }
}