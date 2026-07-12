using System.Threading.Tasks;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _quantityText;
        [SerializeField] private TextMeshProUGUI _itemsLeftText;
        
        private IIAPService _iapService;
        private ProductDescription _productDescription;
        private IAssetsProvider _assetsProvider;

        public async void Initialize(IIAPService iapService, IAssetsProvider assetsProvider, ProductDescription productDescription)
        {
            _iapService = iapService;
            _assetsProvider = assetsProvider;
            _productDescription = productDescription;

            _buyButton.onClick.AddListener(OnBuyClicked);
            await UpdateState(productDescription);
        }

        private async Task UpdateState(ProductDescription productDescription)
        {
            _priceText.text = productDescription.ProductConfig.Price;
            _quantityText.text = productDescription.ProductConfig.Quantity.ToString();
            _itemsLeftText.text = productDescription.QuantityLeft.ToString();
            _icon.sprite = await _assetsProvider.Load<Sprite>(_productDescription.ProductConfig.IconPath);
        }

        private void OnBuyClicked()
        {
            _iapService.PurchaseProduct(_productDescription.Id);
        }
    }
}