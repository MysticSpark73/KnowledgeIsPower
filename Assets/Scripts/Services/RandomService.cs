using UnityEngine;

namespace Services
{
    public class RandomService : IRandomService
    {
        public int Next(int min, int max) => Random.Range(min, max + 1);
    }
}