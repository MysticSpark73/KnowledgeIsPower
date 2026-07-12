using System;

namespace Data
{
    [Serializable]
    public class IAPData
    {
        public string Id;
        public int PurchasedCount;

        public IAPData(string id, int purchasedCount)
        {
            Id = id;
            PurchasedCount = purchasedCount;
        }
    }
}