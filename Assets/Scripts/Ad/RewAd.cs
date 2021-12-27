using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class RewAd : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    //private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string _rewardedUnitId = "ca-app-pub-2866108726683711/6754006882";

    

    private void OnEnable()
    {
        GetRequest();
    }
    private void GetRequest()
    {
        _rewardedAd = new RewardedAd(_rewardedUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.OnUserEarnedReward += OnUserEarnedReward;
        _rewardedAd.LoadAd(adRequest);
    }

    private void OnUserEarnedReward(object sender, Reward e)
    {
        Player.Instance.CollectedCoinsInGame *= 2;
    }

    public void ShowAd()
    {
        if (_rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
            GetRequest();
        }
    }
}
