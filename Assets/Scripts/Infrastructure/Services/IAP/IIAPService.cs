using System;
using System.Collections.Generic;

namespace Infrastructure.Services.IAP
{
    public interface IIAPService : IService
    {
        bool IsInitialized { get; }
        event Action OnInitialized;
        void Initialize();
        void PurchaseProduct(string productId);
        List<ProductDescription> GetAvailableDescriptions();
    }
}