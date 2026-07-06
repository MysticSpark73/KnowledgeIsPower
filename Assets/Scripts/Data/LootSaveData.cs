using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LootSaveData
    {
        public LootSaveData()
        {
            Score = 0;
        }
        
        public int Score;
        public List<UnclaimedLootData> UnclaimedLootDatas = new List<UnclaimedLootData>();
        
        public event Action OnValueChanged;

        public void AddScore(LootData lootData)
        {
            Score += lootData.Value;
            OnValueChanged?.Invoke();
        }
        
        public bool ContainsUnclaimedLoot(string id) => UnclaimedLootDatas.Any(i => i.Id.Equals(id));

        public bool TryGetUnclaimedLoot(string id, out UnclaimedLootData unclaimedLootData)
        {
            unclaimedLootData = new UnclaimedLootData(string.Empty, 0, Vector3.zero, Vector3.zero);
            if (ContainsUnclaimedLoot(id))
            {
                unclaimedLootData = UnclaimedLootDatas.FirstOrDefault(i => i.Id.Equals(id));
                return true;
            }

            return false;
        }

        public void AddUnclaimedLoot(UnclaimedLootData unclaimedLoot)
        {
            if (ContainsUnclaimedLoot(unclaimedLoot.Id)) return;
            UnclaimedLootDatas.Add(unclaimedLoot);
        }

        public void RemoveUnclaimedLoot(string id)
        {
            UnclaimedLootData unclaimedLoot = UnclaimedLootDatas.FirstOrDefault(i => i.Id.Equals(id));
            UnclaimedLootDatas.Remove(unclaimedLoot);
        }
        
        [Serializable]
        public struct UnclaimedLootData
        {
            public string Id;
            public int Value;
            public Vector3Data Position;
            public Vector3Data Rotation;

            public UnclaimedLootData(string id, int value, Vector3 position, Vector3 rotation)
            {
                Id = id;
                Value = value;
                Position = position.ToVectorData();
                Rotation = rotation.ToVectorData();
            }
        }
    }
}