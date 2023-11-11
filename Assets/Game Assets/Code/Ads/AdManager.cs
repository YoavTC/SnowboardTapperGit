using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : Singleton<AdManager>, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField]
    private string androidUnitID, IOSUnitID, adUnitID;

    private void Start()
    {
        DontDestroyOnLoad(this);
        #if UNITY_IOS
        adUnitID = IOSUnitID;
        #elif UNITY_ANDROID
        adUnitID = androidUnitID;
        #elif UNITY_EDITOR
        adUnitID = androidUnitID;
        #endif
    }

    #region Load Ads
    public void LoadAd()
    {
        Debug.Log("UnitID: " + adUnitID);
        Advertisement.Load(adUnitID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Ad " + placementId + " Failed to Load: " + message);
        Debug.Log("Error: " + error);
    }
    #endregion

    #region Show Ads
    public void ShowAd()
    {
        Debug.Log("Showing ad..");
        // SoundManager.Instance.ToggleMute("mainTheme");
        
        //LoadAd();
        
        Debug.Log("-------");
        Debug.Log(gameObject);
        Debug.Log(this);
        Debug.Log("IsInit: " + Advertisement.isInitialized);
        Debug.Log("IsSupp: " + Advertisement.isSupported);
        Debug.Log("-------");

        if (Advertisement.isInitialized)
        {
            Debug.Log("AdUnitID: " + adUnitID);
            Debug.Log("this: " + this);
            Advertisement.Show(adUnitID, this);
        } else {
            HideAd();
            Debug.Log("Hided Ad!");
        }
    }
    
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Ad " + placementId + " Show Failed: " + message);
        Debug.Log("Error: " + error);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Ad Show Started: " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Ad Show Click: " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        HideAd();
        Debug.Log("Ad Show Complete: " + showCompletionState);
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Finished watching ad!");
        }

        if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            Debug.Log("Skipped watching ad!");
        }
    }

    private void HideAd()
    {
        Health health = FindObjectOfType<Health>();
        health.ShowScore();
        // SoundManager.Instance.ToggleMute("mainTheme");
    }
    #endregion
}
