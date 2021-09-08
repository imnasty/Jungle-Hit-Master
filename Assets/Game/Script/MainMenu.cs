using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
	[Header("Candys")]
	public string candys_en;
	public string candys_ru;
	public string candys_sp;
	public string candys_pt;
	public string candys_jp;
	public string candys_cn;
	public string candys_ar;

	public string get_en;
	public string get_ru;
	public string get_sp;
	public string get_pt;
	public string get_jp;
	public string get_cn;
	public string get_ar;


	[Header("Main View")]
	public Button giftButton;
	public Text giftLable;
	public CanvasGroup giftLableCanvasGroup;
	public GameObject giftBlackScreen;
	public GameObject giftParticle;
	public Image selectedKnifeImage;
	public AudioClip giftSfx;
	public GameObject GiftButton;
	public Text menuMaxScore;
	public Text menuMaxStage;

	public static MainMenu intance;
	[SerializeField] private PlayFabManager playFabManager;

	// Gift Setting

	int timeForNextGift = 60*8;
	int minGiftCandy = 40;// Minimum Candy for Gift
	int maxGiftCandy = 70;// Maxmum Candy for Gift
	void Awake()
	{
		intance = this;
		
	}
	void Start()
	{
		menuMaxScore.text =GameManager.MaxScore + ""; 
		menuMaxStage.text =GameManager.MaxStage + "";

		CUtils.ShowInterstitialAd();
		InvokeRepeating ("updateGiftStatus", 0f, 1f);
		KnifeShop.intance.UpdateUI();                            
		playFabManager.SendLeaderboard(GameManager.MaxScore);
	}

   /* void Update()
    {			
		
			menuMaxScore.text =GameManager.MaxScore + "";
			menuMaxStage.text =GameManager.MaxStage + "";
		if(GameManager.MaxScore < GameManager.Score)
        {
			playFabManager.SendLeaderboard(GameManager.MaxScore);
		}
				
	}*/

	public void OnPlayClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
	}
	public void RateGame()
	{
		SoundManager.instance.PlaybtnSfx ();
        CUtils.OpenStore();
	}

	void updateGiftStatus()
	{
		if (GameManager.GiftAvalible) {
			giftButton.interactable = true;
			GiftButton.GetComponent<Animator>().enabled = true;
			LeanTween.alphaCanvas (giftLableCanvasGroup, 0f, .4f).setOnComplete (() => {
				LeanTween.alphaCanvas (giftLableCanvasGroup, 1f, .4f);
			});
			if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
			{
				giftLable.text = "READY!";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
			{
				giftLable.text = "Готов!";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
			{
				giftLable.text = "Listo!";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
			{
				giftLable.text = "Preparar!";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
			{
				giftLable.text = "مستعد";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
			{
				giftLable.text = "準備ができました";

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
			{
				giftLable.text = "做好准备";

			}

		}
		else {
			giftButton.interactable = false;
			GiftButton.GetComponent<Animator>().enabled = false;
			giftLable.text = GameManager.RemendingTimeSpanForGift.Hours.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Minutes.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Seconds.ToString("00");
		}
	}

	[ContextMenu("Get Gift")]
	public void OnGiftClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		int Gift = UnityEngine.Random.Range(minGiftCandy, maxGiftCandy);
		if (PlayerPrefs.GetInt("Vip", 0) == 1)
		{
			Gift *= 2;
		}
        
		
		GameManager.Candy += Gift;
		GameManager.NextGiftTime = DateTime.Now.AddMinutes(timeForNextGift);
		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			Toast.instance.ShowMessage(get_en + " " + Gift + " " + candys_en);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			Toast.instance.ShowMessage(get_ru + " " + Gift + " " + candys_ru);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			Toast.instance.ShowMessage(get_sp + " " + Gift + " " + candys_sp);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			Toast.instance.ShowMessage(get_pt + " " + Gift + " " + candys_pt);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			Toast.instance.ShowMessage(get_ar + " " + Gift + " " + candys_ar);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			Toast.instance.ShowMessage(get_jp + " " + Gift + " " + candys_jp);


		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			Toast.instance.ShowMessage(get_cn + " " + Gift + " " + candys_cn);


		}

		updateGiftStatus();
		giftBlackScreen.SetActive (true);
		Instantiate<GameObject>(giftParticle);
		SoundManager.instance.PlaySingle (giftSfx);
		Invoke("HideGiftParticle",2f);
	}
	public void HideGiftParticle()
	{
		giftBlackScreen.SetActive (false);
	}
	public void OpenShopUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		KnifeShop.intance.showShop ();	
	}
	public void OpenSettingUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		SettingUI.intance.showUI();	
	}
}

