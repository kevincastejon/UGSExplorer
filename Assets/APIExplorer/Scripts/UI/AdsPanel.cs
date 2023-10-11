using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class AdsPanel : UIPanel
    {
        [SerializeField] private TMP_InputField _appKeyInput;
        [SerializeField] private Button _initBtn;
        [SerializeField] private Button _loadBannerBtn;
        [SerializeField] private Button _displayBannerBtn;
        [SerializeField] private Button _hideBannerBtn;
        [SerializeField] private Button _destroyBannerBtn;
        [SerializeField] private Button _loadInterstitialBtn;
        [SerializeField] private Button _showInterstitialBtn;
        [SerializeField] private Button _loadRewardedBtn;
        [SerializeField] private Button _showRewardedBtn;

        //private static AdsPanel _instance;
        //public static AdsPanel Instance { get => _instance; }
        //protected override void Awake()
        //{
        //    base.Awake();
        //    _instance = this;
        //    _initBtn.onClick.AddListener(Init);
        //    _loadBannerBtn.onClick.AddListener(LoadBanner);
        //    _displayBannerBtn.onClick.AddListener(DisplayBanner);
        //    _hideBannerBtn.onClick.AddListener(HideBanner);
        //    _destroyBannerBtn.onClick.AddListener(DestroyBanner);
        //    _loadInterstitialBtn.onClick.AddListener(LoadInterstitial);
        //    _showInterstitialBtn.onClick.AddListener(ShowInterstitial);
        //    _loadRewardedBtn.onClick.AddListener(LoadRewarded);
        //    _showRewardedBtn.onClick.AddListener(ShowRewarded);
        //    //Add AdInfo Banner Events
        //    IronSourceBannerEvents.onAdClickedEvent += (IronSourceAdInfo adInfo) => Log("AdClickedEvent");
        //    IronSourceBannerEvents.onAdScreenPresentedEvent += (IronSourceAdInfo adInfo) => Log("AdScreenPresentedEvent");
        //    IronSourceBannerEvents.onAdScreenDismissedEvent += (IronSourceAdInfo adInfo) => Log("AdScreenDismissedEvent");
        //    IronSourceBannerEvents.onAdLeftApplicationEvent += (IronSourceAdInfo adInfo) => Log("AdLeftApplicationEvent");

        //}

        //private void Init()
        //{
        //    StartWait();
        //    Log("Initializing Ads...");
        //    IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        //    try
        //    {
        //        IronSource.Agent.init(_appKeyInput.text);
        //    }
        //    catch (Exception e)
        //    {
        //        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
        //        LogException(e);
        //        Log("Initializing Ads failed.");
        //        StopWait();
        //        return;
        //    }
        //}
        //private void SdkInitializationCompletedEvent()
        //{
        //    IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
        //    Log("Ads initialized.");
        //    IronSource.Agent.validateIntegration();
        //    StopWait();
        //}
        //private void LoadBanner()
        //{
        //    StartWait();
        //    Log("Loading banner...");
        //    IronSourceBannerEvents.onAdLoadedEvent += OnBannerLoaded;
        //    IronSourceBannerEvents.onAdLoadFailedEvent += OnBannerLoadingFailed;
        //    try
        //    {
        //        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        //    }
        //    catch (Exception e)
        //    {
        //        IronSourceBannerEvents.onAdLoadedEvent -= OnBannerLoaded;
        //        IronSourceBannerEvents.onAdLoadFailedEvent -= OnBannerLoadingFailed;
        //        StopWait();
        //        LogException(e);
        //        Log("Loading banner failed.");
        //        return;
        //    }
        //}

        //private void OnBannerLoaded(IronSourceAdInfo info)
        //{
        //    IronSourceBannerEvents.onAdLoadedEvent -= OnBannerLoaded;
        //    IronSourceBannerEvents.onAdLoadFailedEvent -= OnBannerLoadingFailed;
        //    StopWait();
        //    Log("Banner loaded.");
        //}

        //private void OnBannerLoadingFailed(IronSourceError error)
        //{
        //    IronSourceBannerEvents.onAdLoadedEvent -= OnBannerLoaded;
        //    IronSourceBannerEvents.onAdLoadFailedEvent -= OnBannerLoadingFailed;
        //    StopWait();
        //    LogError(error.getDescription());
        //    Log("Loading banner failed.");
        //}

        //private void DisplayBanner()
        //{
        //    IronSource.Agent.displayBanner();
        //}

        //private void HideBanner()
        //{
        //    IronSource.Agent.hideBanner();
        //}

        //private void DestroyBanner()
        //{
        //    IronSource.Agent.destroyBanner();
        //}

        //private void LoadInterstitial()
        //{

        //}

        //private void ShowInterstitial()
        //{

        //}

        //private void LoadRewarded()
        //{

        //}

        //private void ShowRewarded()
        //{

        //}


        //void OnApplicationPause(bool isPaused)
        //{
        //    IronSource.Agent.onApplicationPause(isPaused);
        //}
    }
}