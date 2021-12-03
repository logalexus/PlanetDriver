using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class RewAd : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";

    private void OnEnable()
    {
        _rewardedAd = new RewardedAd(_rewardedUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(adRequest);
        _rewardedAd.OnUserEarnedReward += OnUserEarnedReward;
    }

    private void OnUserEarnedReward(object sender, Reward e)
    {
        Player.Instance.CollectedCoinsInGame *= 2;
    }

    public void ShowAd()
    {
        if (_rewardedAd.IsLoaded())
            _rewardedAd.Show();
    }
}
