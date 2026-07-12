using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PurchaseData
    {
        public event Action OnPurchasedItemsChanged;
        
        public List<IAPData> _purchasedItems = new ();

        public void AddPurchase(string id)
        {
            IAPData purchasedItem = _purchasedItems.Find(i => i.Id == id);
            
            if (purchasedItem != null)
            {
                purchasedItem.PurchasedCount++;
            }
            else
            {
                _purchasedItems.Add(new IAPData(id, 1));
            }
            
            OnPurchasedItemsChanged?.Invoke();
        }
        
        public IAPData GetPurchasedItem(string id) => _purchasedItems.Find(i => i.Id == id);
    }
}