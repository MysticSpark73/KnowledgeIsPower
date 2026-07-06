using System;

namespace Data
{
    [Serializable]
    public class LootSaveData
    {
        public int Score;
        public event Action OnValueChanged;

        public void AddScore(LootData lootData)
        {
            Score += lootData.Value;
            OnValueChanged?.Invoke();
        }
    }
}