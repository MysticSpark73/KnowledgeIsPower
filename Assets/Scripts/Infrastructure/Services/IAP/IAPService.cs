using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        public bool IsInitialized => _provider.IsInitialized;
        public event Action OnInitialized;
        
        private readonly IAPProvider _provider;
        private readonly IPersistentProgressService _progressService;

        public IAPService(IPersistentProgressService progressService)
        {
            _provider =  new IAPProvider(this);
            _progressService = progressService;
        }

        public void Initialize()
        {
            _provider.OnInitialized += () => OnInitialized?.Invoke();
            _provider.Initialize().GetAwaiter().GetResult();
        }

        public void PurchaseProduct(string productId) => _provider.PurchaseProduct(productId);

        public void ProcessPurchase(PendingOrder pendingOrder)
        {
            foreach (var purchasedProductInfo in pendingOrder.Info.PurchasedProductInfo)
            {
                ProcessProduct(purchasedProductInfo);
            }
            
            _provider.ConfirmOrder(pendingOrder);
        }

        private void ProcessProduct(IPurchasedProductInfo product)
        {
            ProductConfig config = _provider.GetProductConfig(product.productId);

            switch (config.ItemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Currency:
                    _progressService.PlayerProgress.WorldData.LootData.AddScore(new LootData(config.Quantity));
                    _progressService.PlayerProgress.PurchaseData.AddPurchase(product.productId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<ProductDescription> GetAvailableDescriptions() => GetAvailableToPurchaseDescriptions().ToList();

        private IEnumerable<ProductDescription> GetAvailableToPurchaseDescriptions()
        {
            PurchaseData purchaseData = _progressService.PlayerProgress.PurchaseData;

            foreach (var product in _provider.GetProducts())
            {
                ProductConfig config = _provider.GetProductConfig(product.definition.id);

                IAPData purchasedItem = purchaseData.GetPurchasedItem(product.definition.id);

                if (purchasedItem != null && purchasedItem.PurchasedCount >= config.Quantity) continue;

                yield return new ProductDescription(product.definition.id, product, config,
                    purchasedItem == null ? config.Quantity : config.Quantity - purchasedItem.PurchasedCount);
            }
        }
    }
}