using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/MonsterData")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeID Type;
        [Range(1, 100)] public int Health;
        [Range(1f, 10f)] public float MoveSpeed;
        [Range(1, 10)] public float AttackCooldown;
        [Range(0.5f, 1f)] public float WeaponRadius;
        [Range(0.5f, 1f)] public float AttackRange;
        [Range(1, 30)] public float Damage;
        [Header("Loot")]
        public int MinLoot;
        public int MaxLoot;
        [Header("Prefab")]
        public GameObject Prefab;
    }
}