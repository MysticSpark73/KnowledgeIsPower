using Data;
using Infrastructure.Factory;
using Services;
using UnityEngine;

namespace Enemies
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;
        
        private int _min;
        private int _max;
        private IGameFactory _factory;
        private IRandomService _randomService;

        public void Initialize(IGameFactory factory, IRandomService randomService)
        {
            _factory = factory;
            _randomService = randomService;
        }

        public void SetLoot(int min, int max)
        {
            _min = min;
            _max = max;
        }

        private void Start()
        {
            _enemyDeath.OnDeath += SpawnLoot;
        }

        private void OnDestroy()
        {
            _enemyDeath.OnDeath -= SpawnLoot;
        }

        private async void SpawnLoot() 
        {
            LootTrigger lootTrigger = await _factory.CreateLoot();
            if (lootTrigger == null)
            {
                Debug.LogWarning("Loot object is null.");
                return;
            }
            
            lootTrigger.transform.position = transform.position;

            lootTrigger.SetLootData(CreateLootData());
        }

        private LootData CreateLootData() => new(_randomService.Next(_min, _max));
    }
}