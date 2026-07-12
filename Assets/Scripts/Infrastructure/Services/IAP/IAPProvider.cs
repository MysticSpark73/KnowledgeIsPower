using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
    public class IAPProvider
    {
        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;
        
        private const string IAPConfigsPath = "IAP/products";
        
        private StoreController _storeController;

        private readonly List<ProductDefinition> _initialProducts = new ();
        private List<Product> _products;
        private Dictionary<string, ProductConfig> _productConfigsDictionary;

        private IAPService _iapService;

        public IAPProvider(IAPService iapService)
        {
            _iapService = iapService;
            _storeController = UnityIAPServices.StoreController();
        }

        public async Task Initialize()
        {
            LoadInitialProducts();
            SubscribeToEvents();
            
            await _storeController.Connect();
            _storeController.FetchProducts(_initialProducts);
        }

        public void PurchaseProduct(string productId) => _storeController.PurchaseProduct(productId);

        public void ConfirmOrder(PendingOrder order) => _storeController.ConfirmPurchase(order);

        public ProductConfig GetProductConfig(string productId) => 
            _productConfigsDictionary.GetValueOrDefault(productId);

        public IReadOnlyList<Product> GetProducts() => _products;

        private void SubscribeToEvents()
        {
            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnProductsFetchFailed += OnProductsFetchFailed;
            _storeController.OnPurchasesFetched += OnPurchasesFetched;
            _storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            _storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            _storeController.OnPurchaseDeferred += OnPurchaseDeferred;
            _storeController.OnPurchaseFailed += OnPurchaseFailed;
            _storeController.OnPurchasePending += OnPurchasePending;
        }

        private void UnsubscribeFromEvents()
        {
            _storeController.OnProductsFetched -= OnProductsFetched;
            _storeController.OnProductsFetchFailed -= OnProductsFetchFailed;
            _storeController.OnPurchasesFetched -= OnPurchasesFetched;
            _storeController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
            _storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            _storeController.OnPurchaseDeferred -= OnPurchaseDeferred;
            _storeController.OnPurchaseFailed -= OnPurchaseFailed;
            _storeController.OnPurchasePending -= OnPurchasePending;
        }

        private void LoadInitialProducts()
        {
            List<ProductConfig> productConfigs =
                Resources.Load<TextAsset>(IAPConfigsPath)
                    .text
                    .Deserialize<ProductConfigContainer>()
                    .Configs;

            _productConfigsDictionary = productConfigs.ToDictionary(i => i.Id, i => i);
            
            foreach (var config in productConfigs)
            {
                _initialProducts.Add(new ProductDefinition(config.Id, config.Type));
            }
        }

        private void OnProductsFetched(List<Product> products)
        {
            _products = products;
            _storeController.FetchPurchases();
        }

        private void OnProductsFetchFailed(ProductFetchFailed productFetchFailed)
        {
            Debug.Log($"IAPProvider OnProductsFetchFailed: {productFetchFailed.FailureReason}");
        }

        private void OnPurchasesFetched(Orders orders)
        {
            IsInitialized = true;
            OnInitialized?.Invoke();
            Debug.Log("IAPProvider successfully initialized!");
        }

        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription purchasesFetchFailureDescription)
        {
            Debug.Log($"IAPProvider OnPurchasesFetchFailed: {purchasesFetchFailureDescription.Message}");
        }

        private void OnPurchaseConfirmed(Order order)
        {
            Debug.Log($"IAPProvider purchase confirmed {order.Info.TransactionID}");
            foreach (var productInfo in order.Info.PurchasedProductInfo)
            {
                Debug.Log($"IAPProvider purchase confirmed {productInfo.productId}");
            }
        }

        private void OnPurchaseDeferred(DeferredOrder order)
        {
            Debug.Log($"IAPProvider purchase deferred {order.Info.TransactionID}");
            foreach (var productInfo in order.Info.PurchasedProductInfo)
            {
                Debug.Log($"IAPProvider purchase deferred {productInfo.productId}");
            }
        }

        private void OnPurchaseFailed(FailedOrder order)
        {
            Debug.LogError($"IAPProvider product purchase failed: {order.FailureReason}, transaction Id {order.Info.TransactionID}");
            foreach (var productInfo in order.Info.PurchasedProductInfo)
            {
                Debug.LogError($"IAPProvider product purchase failed {productInfo.productId}");
            }
        }

        private void OnPurchasePending(PendingOrder order)
        {
            Debug.Log($"IAPProvider purchase pending {order.Info.TransactionID}");
            _iapService.ProcessPurchase(order);
            foreach (var productInfo in order.Info.PurchasedProductInfo)
            {
                Debug.Log($"IAPProvider purchase pending {productInfo.productId}");
            }
        }
    }
}