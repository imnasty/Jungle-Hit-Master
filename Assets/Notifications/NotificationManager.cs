using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
#if UNITY_ANDROID
public class NotificationManager : MonoBehaviour
{

	public static NotificationManager instance;
	AndroidNotificationChannel defaultChannel;
	private string GiftTitle, GiftBody, RecordTitle, RecordBody, PositionTitle, PositionBody;
	int id_gift, id_record, id_position;
	[Header("Время до показа уведомления о готовноти подарка(ч)")] public int giftNotifDelay;
	[Header("Время до показа уведомления <<Побей рекорд>>(ч)")] public int recordNotifDelay;
	[Header("Время до показа уведомления <<Ваш рекорд побили>>(ч)")] public int positionDelay;

	private void Awake() { instance = this; }

	void Start()
	{
		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			GiftTitle = "The gift is ready!";
			GiftBody = "Ploy in the game and take it)";
			RecordTitle = "Beat your record!";
			RecordBody = "Go to the game and show everyone what you are capable";
			PositionTitle = "AMOGUS";
			PositionBody = "ABOBUS";

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			GiftTitle = "Подарок готов!";
			GiftBody = "Загляни в игру и забери его)";
			RecordTitle = "Побей свой рекорд!";
			RecordBody = "Зайди в игру и покажи всем, на что ты способен";
			PositionTitle = "Амогус";
			PositionBody = "Абобус";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			GiftTitle = "¡El regalo está listo!";
			GiftBody = "Ploy en el juego y tómalo)";
			RecordTitle = "¡Vence su registro!";
			RecordBody = "Ve al juego y muestra a todos lo que eres capaz.";
			PositionTitle = "";
			PositionBody = "";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			GiftTitle = "O presente está pronto!";
			GiftBody = "Ploy no jogo e pegue)";
			RecordTitle = "Bata seu registro!";
			RecordBody = "Vá para o jogo e mostre a todos o que você é capaz";
			PositionTitle = "";
			PositionBody = "";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			GiftTitle = "الهدية جاهزة!";
			GiftBody = "حيلة في اللعبة وأخذها)";
			RecordTitle = "تغلب على السجل الخاص بك!";
			RecordBody = "انتقل إلى اللعبة وإظهار الجميع ما أنت قادر";
			PositionTitle = "";
			PositionBody = "";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			GiftTitle = "贈り物は準備ができています！";
			GiftBody = "ゲームのペルズとそれを取る）";
			RecordTitle = "あなたの記録を破った！";
			RecordBody = "ゲームに行き、あなたができることをみんなに見せる";
			PositionTitle = "";
			PositionBody = "";
		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			GiftTitle = "礼物准备好了！";
			GiftBody = "游戏中的伎俩并带它）";
			RecordTitle = "击败你的唱片！";
			RecordBody = "去游戏并告诉大家你有能力的东西";
			PositionTitle = "";
			PositionBody = "";
		}


		AndroidNotificationCenter.CancelAllNotifications();

		defaultChannel = new AndroidNotificationChannel()
		{
			Id = "default_channel",
			Name = "Jungle",
			Description = "Generic Notification",
			EnableLights = true,
			Importance = Importance.Default
		};

		AndroidNotificationCenter.RegisterNotificationChannel(defaultChannel);

		if (GameManager.NOTIFICATIONS_ON)
		{
			SendRecordNotification();
		}

		AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
		{
			var msg = "Notification received: " + data.Id + "\n";
			msg += "\n Notification received: ";
			msg += "\n .Title: " + data.Notification.Title;
			msg += "\n .Body: " + data.Notification.Text;
			msg += "\n .Channel: " + data.Channel;
			Debug.Log(msg);
		};

		AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

		var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

		if (notificationIntentData != null) Debug.Log("App was opened with notifcation!");
	}

	public void SendGiftNotification()
	{

		AndroidNotification notification = new AndroidNotification()
		{
			Title = GiftTitle,
			Text = GiftBody,
			LargeIcon = "app_icon_large",
			FireTime = System.DateTime.Now.AddHours(giftNotifDelay)
		};

		id_gift = AndroidNotificationCenter.SendNotification(notification, defaultChannel.Id);
	}

	public void CancelRecordNotification()
	{
		AndroidNotificationCenter.CancelNotification(id_record);
	}

	public void SendRecordNotification()
	{
		AndroidNotification notification_record = new AndroidNotification()
		{
			Title = RecordTitle,
			Text = RecordBody,
			LargeIcon = "app_icon_large",
			FireTime = System.DateTime.Now.AddHours(recordNotifDelay)
		};

		id_record = AndroidNotificationCenter.SendNotification(notification_record, defaultChannel.Id);
	}

	public void SendPositionNotification()
    {
		AndroidNotification notification_position = new AndroidNotification()
		{
			Title = PositionTitle,
			Text = PositionBody,
			LargeIcon = "app_icon_large",
			FireTime = System.DateTime.Now.AddHours(positionDelay)
		};

		id_position = AndroidNotificationCenter.SendNotification(notification_position, defaultChannel.Id);
	}
}
#endif