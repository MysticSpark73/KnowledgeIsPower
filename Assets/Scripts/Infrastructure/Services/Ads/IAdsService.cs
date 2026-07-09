using System;

namespace Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action RewardedVideoClicked;
        bool IsAdsReady { get; }
        int AdReward { get; }
        void Initialize();
        void ShowDefaultRewardedAd(Action onSuccess = null);
        void ShowRewardedAd(string placement, Action onSuccess = null);
    }
}