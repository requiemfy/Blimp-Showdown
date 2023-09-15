using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using TMPro;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private int m_requestCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            StartCoroutine(TryRequestAds());
        });
    }

    private IEnumerator TryRequestAds()
    {
        while (enabled)
        {
            CheckCondition();
            yield return new WaitForSeconds(15);
        }
    }
    private void CheckCondition()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;
        if (interstitialAd != null) return;
        LoadInterstitialAd();
    }




    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-1485483970932537/6822273342";
    // test: ca-app-pub-3940256099942544/1033173712
    // real: ca-app-pub-1485483970932537/6822273342
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string _adUnitId = "unused";
#endif

    private InterstitialAd interstitialAd;
    private Action onInterstitialAdEnded;

    private void LoadInterstitialAd()
    {
        m_requestCount++;
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterReloadHandler(interstitialAd);
            });
    }
    public void ShowAd(Action onAdEnded = null)
    {
        onInterstitialAdEnded = onAdEnded;
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            onInterstitialAdEnded?.Invoke();
        }
    }

    private void RegisterReloadHandler(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            onInterstitialAdEnded?.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            onInterstitialAdEnded?.Invoke();
        };
    }
}
