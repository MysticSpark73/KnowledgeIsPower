using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _currencyText;

        protected override void OnStart() =>
            OnScoreValueChanged();

        protected override void SubscribeToEvents() =>
            Progress.WorldData.LootData.OnValueChanged += OnScoreValueChanged;

        protected override void UnSubscribeFromEvents() =>
            Progress.WorldData.LootData.OnValueChanged -= OnScoreValueChanged;

        private void OnScoreValueChanged() =>
            _currencyText.text = Progress.WorldData.LootData.Score.ToString();
    }
}