using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InterAd : MonoBehaviour
{
    private InterstitialAd _interstitialAd;
    //private string _interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";
    private string _interstitialUnitId = "ca-app-pub-2866108726683711/8067088553";

    private void OnEnable()
    {
        _interstitialAd = new InterstitialAd(_interstitialUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(adRequest);
    }

    public void ShowAd()
    {
        if (_interstitialAd.IsLoaded())
            _interstitialAd.Show();
    }

}
