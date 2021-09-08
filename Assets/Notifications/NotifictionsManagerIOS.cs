using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

#if UNITY_IOS

public class NotifictionsManagerIOS : MonoBehaviour
{
	public static NotifictionsManagerIOS instance;
	private string record_notificationID = "Record_notification";
	private string gift_notificationID = "Gift_notification";
	private int ID;
	private string RecordTitle, RecordBody, GiftTitle, GiftBody;
	[Header("Время до уведомления")] public int giftDelay;

	private void Awake() { instance = this; }

	private void Start()
	{
		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			GiftTitle = "The gift is ready!";
			GiftBody = "Ploy in the game and take it)";
			RecordTitle = "Beat your record!";
			RecordBody = "Go to the game and show everyone what you are capable";

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			GiftTitle = "Подарок готов!";
			GiftBody = "Загляни в игру и забери его)";
			RecordTitle = "Побей свой рекорд!";
			RecordBody = "Зайди в игру и покажи всем, на что ты способен";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			GiftTitle = "¡El regalo está listo!";
			GiftBody = "Ploy en el juego y tómalo)";
			RecordTitle = "¡Vence su registro!";
			RecordBody = "Ve al juego y muestra a todos lo que eres capaz.";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			GiftTitle = "O presente está pronto!";
			GiftBody = "Ploy no jogo e pegue)";
			RecordTitle = "Bata seu registro!";
			RecordBody = "Vá para o jogo e mostre a todos o que você é capaz";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			GiftTitle = "الهدية جاهزة!";
			GiftBody = "حيلة في اللعبة وأخذها)";
			RecordTitle = "تغلب على السجل الخاص بك!";
			RecordBody = "انتقل إلى اللعبة وإظهار الجميع ما أنت قادر";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			GiftTitle = "贈り物は準備ができています！";
			GiftBody = "ゲームのペルズとそれを取る）";
			RecordTitle = "あなたの記録を破った！";
			RecordBody = "ゲームに行き、あなたができることをみんなに見せる";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			GiftTitle = "礼物准备好了！";
			GiftBody = "游戏中的伎俩并带它）";
			RecordTitle = "击败你的唱片！";
			RecordBody = "去游戏并告诉大家你有能力的东西";
		}
		iOSNotificationCenter.RemoveAllScheduledNotifications();

		iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger()
		{
			// Year = xxxx,
			// Month = xx,
			// Day = xx,
			Hour = 12,
			Minute = 0,
			// Second = xx,
			Repeats = true
		};

		iOSNotificationLocationTrigger locationTrigger = new iOSNotificationLocationTrigger()
		{
			Center = new Vector2(2.3f, 49f),
			Radius = 250f,
			NotifyOnEntry = true,
			NotifyOnExit = false
		};

		iOSNotification Recordnotification = new iOSNotification()
		{
			Identifier = record_notificationID,
			Title = RecordTitle,
			Body = RecordBody,
			ShowInForeground = true,
			ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
			CategoryIdentifier = "category_a",
			ThreadIdentifier = "thread1",
			Trigger = calendarTrigger
		};

		iOSNotificationCenter.ScheduleNotification(Recordnotification);

		iOSNotificationCenter.OnRemoteNotificationReceived += recievedNotification =>
		{
			Debug.Log("Recieved notification " + Recordnotification.Identifier + "!");
		};

		iOSNotification notificationIntentData = iOSNotificationCenter.GetLastRespondedNotification();
	}

	public void CancelRecordNotification()
	{
		iOSNotificationCenter.RemoveScheduledNotification(record_notificationID);

		iOSNotificationCenter.RemoveDeliveredNotification(record_notificationID);
	}

	public void SendGiftNotification()
	{
		iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
		{
			TimeInterval = new TimeSpan(giftDelay, 0, 0),
			Repeats = false
		};

		iOSNotification Gift_notification = new iOSNotification()
		{
			Identifier = gift_notificationID,
			Title = RecordTitle,
			Body = RecordBody,
			ShowInForeground = true,
			ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
			CategoryIdentifier = "category_a",
			ThreadIdentifier = "thread1",
			Trigger = timeTrigger
		};

		iOSNotificationCenter.ScheduleNotification(Gift_notification);
	}
}
#endif