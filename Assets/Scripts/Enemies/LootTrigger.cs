using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class LootTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickupFx;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;
        
        private LootData _lootData;
        private bool _pickedUp;
        private WorldData _worldData;

        public void Initialize(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void SetLootData(LootData lootData) => _lootData = lootData;

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            if (_pickedUp) return;
            _pickedUp = true;
            
            UpdateScore();
            HideVisuals();
            TriggerPickupFX();
            ShowText();
            StartCoroutine(DestroyRoutine());
        }

        private void UpdateScore() => _worldData.LootData.AddScore(_lootData);

        private void HideVisuals() => _skull.SetActive(false);

        private void TriggerPickupFX() => _pickupFx.SetActive(true);

        private void ShowText()
        {
            _lootText.text = _lootData.Value.ToString();
            _pickupPopup.SetActive(true);
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}