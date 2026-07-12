using System;
using System.Collections.Generic;

namespace Infrastructure.Services.IAP
{
    [Serializable]
    public class ProductConfigContainer
    {
        public List<ProductConfig> Configs;
    }
}