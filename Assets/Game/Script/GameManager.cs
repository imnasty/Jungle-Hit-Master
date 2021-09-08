using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

	public static bool isGameOver=false;
	public static Knife selectedKnifePrefab=null;
	public static bool NOTIFICATIONS_ON
	{
		get { return PlayerPrefs.GetInt("NotificationsTrigger", 1) == 1 ? true : false; }
		set { PlayerPrefs.SetInt("NotificationsTrigger", value ? 1 : 0); }
	}
	public static float ScreenHeight{
		get
		{ 
			if(Camera.main!=null)
				return Camera.main.orthographicSize * 2f;
			return 0f;
		}
	}
	public static float ScreenWidth{
		get
		{ 
			if(Camera.main!=null)
				return  ScreenHeight / Screen.height * Screen.width;
			return 0f;
		}
	}
	public static int Score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			if(GamePlayManager.instance != null)
			GamePlayManager.instance.UpdateLable ();
		}
	}
	static int _score;
	public static int Stage
	{
		get
		{
			return _stage;
		}
		set
		{
			_stage = value;
			if(GamePlayManager.instance != null)
				GamePlayManager.instance.UpdateLable ();
		}
	}
	static int _stage;
	public static int HighScore
	{
		get
		{
			return PlayerPrefs.GetInt ("Player's HighScore", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("Player's HighScore", value);
		}
	}

	public static int FromAds
	{
		get
		{
			return PlayerPrefs.GetInt("Player's FromAds", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Player's FromAds", value);
		}
	}

	public static int MaxScore
	{
		get
		{
			return PlayerPrefs.GetInt("Player's MaxScore", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Player's MaxScore", value);
		}
	}

	public static int MaxStage
	{
		get
		{
			return PlayerPrefs.GetInt("Player's MaxStage", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Player's MaxStage", value);
			PlayFabManager.instance.SetUserMaxStage(value);
		}
	}

	public static int OffFeedBacks
	{
		get
		{
			return PlayerPrefs.GetInt("Player's OffFeedBacks", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Player's OffFeedBacks", value);
		}
	}

	public static int FirstTimeBoss
	{
		get
		{
			return PlayerPrefs.GetInt("Player's FirstTimeBoss", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Player's FirstTimeBoss", value);
		}
	}

	public static int Candy
	{
		get
		{
			return PlayerPrefs.GetInt ("Player's Candy", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("Player's Candy", value);
			if (PlayFabManager.instance.userPlayFabID != null) PlayFabManager.instance.SetUserCandy();
			if (GeneralFunction.intance != null)
				GeneralFunction.intance.candyLbl.text = GameManager.Candy + "";
			
		}
	}
	public static int SelectedKnifeIndex
	{
		get
		{
			return PlayerPrefs.GetInt ("SelectedKnifeIndex", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("SelectedKnifeIndex", value);
		}
	}
	public static bool Sound
	{
		get
		{
			return PlayerPrefs.GetInt ("GameSound", 1)==1;
		}
		set
		{
			PlayerPrefs.SetInt ("GameSound", value?1:0);
		}
	}
	public static bool Vibration
	{
		get
		{
			return PlayerPrefs.GetInt ("GameVibration", 1)==1;
		}
		set
		{
			PlayerPrefs.SetInt ("GameVibration", value?1:0);
		}
	}
	public static bool GiftAvalible
	{
		get
		{
			return	RemendingTimeSpanForGift.TotalSeconds<= 0;
		}
	}
	public static TimeSpan RemendingTimeSpanForGift
	{
		get {
			return (NextGiftTime - DateTime.Now);
		}
	}
	public static DateTime NextGiftTime
	{
		get
		{
			return DateTime.FromFileTime(long.Parse(PlayerPrefs.GetString("LastBonusTime",DateTime.Now.ToFileTime()+"")));
		}
		set
		{
			PlayerPrefs.SetString ("LastBonusTime",value.ToFileTime()+"");
		}
	}
}
