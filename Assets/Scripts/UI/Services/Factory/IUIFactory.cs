using Infrastructure.Services;
using UI.Windows;

namespace UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        ShopWindow CreateShop();
    }
}