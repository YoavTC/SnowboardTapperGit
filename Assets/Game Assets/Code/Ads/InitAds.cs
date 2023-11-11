using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitAds : Singleton<InitAds>, IUnityAdsInitializationListener
{
    public string ANDROID_ID;
    public string IOS_ID;

    private bool isTestingMode;

    private string gameID;

    protected override void OnInitialize()
    {
        InitializeAds();
    }

    void InitializeAds()
    {
        #if UNITY_IOS
        gameID = IOS_ID;
        #elif UNITY_ANDROID
        gameID = ANDROID_ID;
        #elif UNITY_EDITOR
        gameID = ANDROID_ID;
        #endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, isTestingMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ad Init Complete!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ad Init Failed: " + message);
        Debug.Log(error);
    }
}
