using System.Threading.Tasks;
using Infrastructure.Services;
using UI.Windows;
using UI.Windows.Shop;

namespace UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        Task CreateUIRoot();
        ShopWindow CreateShop();
    }
}