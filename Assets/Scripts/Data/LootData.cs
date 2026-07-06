using System;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public int Value;
        public LootData(int value)
        {
            Value = value;
        }
    }
}