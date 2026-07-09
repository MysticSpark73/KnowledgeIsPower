using Infrastructure.Services;
using UI.Windows;
using UI.Windows.Shop;

namespace UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        ShopWindow CreateShop();
    }
}