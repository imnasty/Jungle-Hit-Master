using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour {

	[Header("Setting View")]
	public  Toggle soundToggle;
	public  Toggle vibrationToggle;
	public Toggle notificationsToggle;
	public  GameObject UIParent;
    public Text removeAdPriceText;
	public static SettingUI intance;

	void Awake()
	{
		if (intance == null) 
		{
			intance = this;
		}
	}

	void Start()
	{

		soundToggle.onValueChanged.RemoveAllListeners ();
		vibrationToggle.onValueChanged.RemoveAllListeners ();
		updateUI ();
		soundToggle.onValueChanged.AddListener ((arg0) =>{ 
			GameManager.Sound=arg0;
			if(arg0)
				SoundManager.instance.PlaybtnSfx ();
		} );
		vibrationToggle.onValueChanged.AddListener ((arg0) =>{ 
			GameManager.Vibration=arg0;
			if(arg0)
				SoundManager.instance.playVibrate();
		} );
		notificationsToggle.onValueChanged.AddListener((arg0) =>
		{
			GameManager.NOTIFICATIONS_ON = arg0;
			if (arg0)
			{
#if UNITY_ANDOID
				NotificationManager.instance.SendRecordNotification();
#elif UNITY_IOS
				NotifictionsManagerIOS.instance.SendGiftNotification();
#endif
			}

				if (arg0 == false) {
#if UNITY_ANDROID
				NotificationManager.instance.CancelRecordNotification();
#elif UNITY_IOS
				NotifictionsManagerIOS.instance.CancelRecordNotification();
#endif
				}
		});

	}


	public void showUI()
	{
		UIParent.SetActive (true);
        CUtils.ShowInterstitialAd();
	}

	public void updateUI()
	{
		soundToggle.isOn = GameManager.Sound;
		vibrationToggle.isOn = GameManager.Vibration;
		notificationsToggle.isOn = GameManager.NOTIFICATIONS_ON;
	}

	

}
