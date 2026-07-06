using System;
using System.Collections;
using Data;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class LootTrigger : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickupFx;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;
        private int _hash;
        
        private LootData _lootData;

        private bool _isPickedUp;

        private WorldData _worldData;

        public void Initialize(WorldData worldData)
        {
            _worldData = worldData;
            _hash = $"{gameObject.scene.name}_{transform.position}_{DateTime.Now.Millisecond}".GetHashCode();
        }

        public void SetLootData(LootData lootData) => _lootData = lootData;

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            if (_isPickedUp) return;
            _isPickedUp = true;
            
            UpdateScore();
            HideVisuals();
            TriggerPickupFX();
            ShowText();
            StartCoroutine(DestroyRoutine());
        }

        private void UpdateScore() => _worldData.LootData.AddScore(_lootData);

        public void LoadProgress(PlayerProgress playerProgress) { }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            if (!_isPickedUp)
            {
                playerProgress.WorldData.LootData.AddUnclaimedLoot(
                    new LootSaveData.UnclaimedLootData(_hash.ToString(), _lootData.Value, transform.position,
                        transform.rotation.eulerAngles));
            }
        }

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