using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class GetVipStatus : MonoBehaviour
{
    public Button removeAds;
    public Button vipStatusB;

    public void Start()
    {
        PurchaseManager.OnPurchaseSubscribe += PurchaseManager_OnPurchaseSubscribe;

        if (PurchaseManager.CheckBuyState("vip"))
        {
            vipStatusB.interactable = false;
            removeAds.interactable = false;
            vipStatusB.GetComponentInChildren<Animator>().enabled = false;
            PlayFabManager.instance.SetVipStatus(true);
        }
    }

    private void PurchaseManager_OnPurchaseSubscribe(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == "vip")
        {
            PlayerPrefs.SetInt("Vip", 1);
            PlayFabManager.instance.SetVipStatus(true);
            PlayFabManager.instance.GetVipStatus(PlayFabManager.instance.userPlayFabID);
            vipStatusB.interactable = false;
            vipStatusB.GetComponentInChildren<Animator>().enabled = false;
            removeAds.interactable = false;
            KnifeShop.intance.SetupShop();
        }
    }

}
