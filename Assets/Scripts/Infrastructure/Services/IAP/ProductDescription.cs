using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
    public class ProductDescription
    {
        public string Id;
        public Product Product;
        public ProductConfig ProductConfig;
        public int QuantityLeft;

        public ProductDescription(string id, Product product, ProductConfig productConfig, int quantityLeft)
        {
            Id = id;
            Product = product;
            ProductConfig = productConfig;
            QuantityLeft = quantityLeft;
        }
    }
}