using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class remove_ads : MonoBehaviour
{
    public Button adsButton;
    public static remove_ads instance;

    private void Awake() { instance = this; }

    private void Start()
    {
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;

        if (PurchaseManager.CheckBuyState("remove_ads2"))
        {
            adsButton.interactable = false;
            PlayerPrefs.SetInt("ads", 0);
        }
    }

/*   public void SetDefault(string playFabID)
    {
        PlayFabManager.instance.GetRemoveAds(playFabID);
        if (PlayerPrefs.GetInt("ads", 1) == 0 || PlayerPrefs.GetInt("Vip", 0)==1)
            adsButton.interactable = false;
    }
*/

    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == "remove_ads2")
        {
            PlayerPrefs.SetInt("ads", 0);
            PlayFabManager.instance.SetRemoveAds();
            adsButton.interactable = false;
        }
    }
/*
    public void RemoveAds()
    {
        if (PlayerPrefs.GetInt("Vip", 0) == 0)
        {
            PlayerPrefs.SetInt("ads", 0);
            PlayFabManager.instance.SetRemoveAds();
            adsButton.interactable = false;
        }
    }
*/
}