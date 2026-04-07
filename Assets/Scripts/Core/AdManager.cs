using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class AdManager : MonoBehaviour
{
    [SerializeField] private bool _testMode = true;

    private string _bannerAdUnitId;
    private string _interstitialAdUnitId;

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;

    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });

#if UNITY_ANDROID
        _bannerAdUnitId = _testMode ? "ca-app-pub-3940256099942544/6300978111" : "ca-app-pub-6530733960152815/9557327133";
        _interstitialAdUnitId = _testMode ? "ca-app-pub-3940256099942544/1033173712" : "ca-app-pub-6530733960152815/7701319987";
#else
        _bannerAdUnitId = "unexpected_platform";
        _interstitialAdUnitId = "unexpected_platform";
#endif
    }

    private void Start()
    {
        LoadBannerAd();
        LoadInterstitialAd();

        ShowInterstitialAd();
    }

    #region

    private void LoadBannerAd()
    {
        //
        _bannerView?.Destroy();

        AdSize adSize = AdSize.Banner; // 320x50
        _bannerView = new BannerView(_bannerAdUnitId, adSize, AdPosition.Bottom);

        AdRequest adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

    #endregion

    #region

    private void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(_interstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"  ε : {error}");
                return;
            }

            _interstitialAd = ad;
            RegisterEventHandlers(ad);
            RegisterReloadHandler(ad);
        });

    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("  ");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"   : {error}");
        };
    }

    private void RegisterReloadHandler(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();
        };
    }

    public void ShowInterstitialAd()
    {/*
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("   غ ʾҽϴ.");
        }*/
    }

    #endregion
}
