using System;
using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
    [Serializable]
    public class ProductConfig
    {
        public string Id;
        public ProductType Type;
        public int MaxPurchaseCount;
        public ItemType ItemType;
        public int Quantity;
        public string Price;
        public string IconPath;
    }
}