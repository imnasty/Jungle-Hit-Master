using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class PurchaseEvents : MonoBehaviour
{
    public Button test;
    // Start is called before the first frame update
    void Start()
    {
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        if (PurchaseManager.CheckBuyState("offads"))
        {
            test.enabled=false;
            PlayerPrefs.SetInt("removeads",1);
        }
    }
   
    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {

        Debug.Log("You purchase: "+args.purchasedProduct.definition.id);
    }
    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        
        if (args.purchasedProduct.definition.id=="offads")
        {
            test.enabled=false;
            PlayerPrefs.SetInt("removeads",1);
            Debug.Log(PlayerPrefs.GetInt("removeads").ToString());
        }
        Debug.Log("You purchase: "+args.purchasedProduct.definition.id+" Non consumable");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
