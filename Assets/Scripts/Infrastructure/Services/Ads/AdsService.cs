using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsShowListener, IUnityAdsInitializationListener
    {
        private Action _onRwSuccess;
        private const string RewardedPlacementId = "Rewarded_Android";
        private const string GameID = "800083432";

        public bool IsAdsReady => Advertisement.isInitialized;

        public int AdReward => 13;
        
        public event Action RewardedVideoClicked;

        public void Initialize()
        {
            Advertisement.Initialize(GameID, true, this);
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"[OnUnityAdsShowClick] {placementId}");

            if (placementId.Equals(RewardedPlacementId))
            {
                RewardedVideoClicked?.Invoke();
            }
        }

        public void OnInitializationComplete() => Debug.Log("[OnInitializationComplete] Ads Initialized!");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => Debug.LogError($"[OnInitializationFailed] {message}");

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) => Debug.Log($"[OnUnityAdsShowFailure] {message}");

        public void OnUnityAdsShowStart(string placementId) => Debug.Log($"[OnUnityAdsShowStart] {placementId}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.LogError($"[OnUnityAdsShowComplete] {showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onRwSuccess?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.LogError($"[OnUnityAdsShowComplete] {showCompletionState}");
                    break;
                default:
                    Debug.LogError($"[OnUnityAdsShowComplete] {showCompletionState}");
                    break;
            }

            _onRwSuccess = null;
        }

        public void ShowDefaultRewardedAd(Action onSuccess = null) => ShowRewardedAd(RewardedPlacementId, onSuccess);

        public void ShowRewardedAd(string placement, Action onSuccess = null)
        {
            Advertisement.Show(placement, this);
            _onRwSuccess = onSuccess;
        }

    }
}