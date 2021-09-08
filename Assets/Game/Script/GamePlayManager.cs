using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
//using Firebase.Analytics;
//using Firebase;

public class GamePlayManager : MonoBehaviour {


	public static GamePlayManager instance;
	[Header("Circle Setting")]
	public Circle[] circlePrefabs;
	public Circle[] circlePrefabsVip;
	[Header("Level")]
	public string Level_en;
	public string Level_ru;
	public string Level_sp;
	public string Level_pt;
	public string Level_jp;
	public string Level_cn;
	public string Level_ar;

	[Header("Boss")]
	public string Boss_en;
	public string Boss_ru;
	public string Boss_sp;
	public string Boss_pt;
	public string Boss_jp;
	public string Boss_cn;
	public string Boss_ar;

	public Bosses[] BossPrefabs;
	public Bosses[] BossPrefabsVip;

	public Transform circleSpawnPoint;
	[Range(0f,1f)]public float circleWidthByScreen=.5f;

	[Header("Knife Setting")]
	public Knife knifePrefab;
	public Transform KnifeSpawnPoint;
	[Range(0f,1f)]public float knifeHeightByScreen=.1f;

	public GameObject CandyPrefab;
	[Header("UI Object")]
	public Text lblScore;
	public Text lblStage;
	public List<Image> stageIcons;
	public Color stageIconActiveColor;
	public Color stageIconNormalColor;
	public GameObject circle;
	


	[Header("UI Boss")]

	public GameObject bossFightStart;
	public GameObject bossFightEnd;
	public AudioClip[] bossFightStartSounds;
	public AudioClip[] bossFightEndSounds;
	[Header("Ads Show")]
	public GameObject adsShowView;
	public Image adTimerImage;
	public Text adSocreLbl;
	public Text adStageLbl;


	[Header("GameOver Popup")]
	public GameObject gameOverView;
	public Text gameOverSocreLbl,gameOverStageLbl;
	public GameObject newBestScore;
	public AudioClip gameOverSfx;

	[Header("FeedBack")]
	[HideInInspector] public GameObject feedBack;

	[Space(50)]

	public int cLevel = 0;
	public bool isDebug = false;
	string currentBossName="";
	Circle currentCircle;
	Knife currentKnife;
	bool usedAdContinue;
	public static string currentBossTag;

	private bool FeedbackOn = false;
	public bool vipKnTrigg = false;
	private string lastBossTag;

	public Button clickButton;

	[Header("Только для тестов (перед релизом отключить)")]public bool freeCont;

	public int totalSpawnKnife
	{
		get
		{ 
			return _totalSpawnKnife;
		}
		set
		{
			_totalSpawnKnife = value;
		}
	}
	int _totalSpawnKnife;

	void Awake()
	{	
		Application.targetFrameRate = 144;
		QualitySettings.vSyncCount = 0;
		if (instance == null) {
			instance = this;		
		}
	}
	void Start () {

		clickButton.onClick.AddListener(delegate
		{
			if (!currentKnife.isFire)
			{
				KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
				currentKnife.ThrowKnife();
				StartCoroutine(GenerateKnife());
			}
		});

		FindFeedback();
		startGame ();
        //CUtils.ShowInterstitialAd();
		KnifeShop.inShopTrigger = false;	
    }

    private void OnEnable()
    {
        Timer.Schedule(this, 0.1f, AddEvents);
    }

    private void AddEvents()
    {

	}

	bool doneWatchingAd = false;

	public void HandleRewardBasedVideoRewarded(object sender)
	{
		if (usedAdContinue)
		{
			doneWatchingAd = true;
			AdShowSucessfully();
		}
	}

	public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        if (usedAdContinue)
        {
            if (doneWatchingAd == false)
            {
                adsShowView.SetActive(false);
                usedAdContinue = false;
                showGameOverPopup();
            }
        }
    }

    private void OnDisable()
    {

    }
	
	public void startGame()
	{
		GameManager.Score = 0;
		GameManager.Stage = 1;
		GameManager.isGameOver = false;
		usedAdContinue = false;
		if (isDebug) {
			GameManager.Stage = cLevel;
		}
		setupGame ();
	}

	public void UpdateLable()
	{
		lblScore.text = GameManager.Score+"";
		if (GameManager.Stage % 5 == 0) {
			for (int i = 0; i < stageIcons.Count-1; i++) {
				stageIcons [i].gameObject.SetActive(false);
			}
			stageIcons [stageIcons.Count-1].color = stageIconActiveColor;
			lblStage.color = stageIconActiveColor;
			lblStage.text = currentBossName;
		}
		else {
			if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
			{
                lblStage.text = Level_en + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
			{
				lblStage.text = Level_ru + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
			{
				lblStage.text = Level_sp + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
			{
				lblStage.text = Level_pt + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
			{
				lblStage.text = Level_ar + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
			{
				lblStage.text = Level_jp + GameManager.Stage;

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
			{
				lblStage.text = Level_cn + GameManager.Stage;

			}
			for (int i = 0; i < stageIcons.Count; i++) {
				stageIcons [i].gameObject.SetActive(true);
				stageIcons [i].color = GameManager.Stage % stageIcons.Count <= i ? stageIconNormalColor : stageIconActiveColor;
			}
			lblStage.color = stageIconNormalColor;
		}
	}
	public void setupGame()
	{
		spawnCircle();
		KnifeCounter.intance.setUpCounter(currentCircle.totalKnife);

		totalSpawnKnife = 0;

			StartCoroutine(GenerateKnife());
		
		
	}
	void Update () {
		if (currentKnife == null)
			return;
	/*	if (Input.GetMouseButtonDown (0) && !currentKnife.isFire && !KnifeShop.inShopTrigger && !FeedbackOn) {
			KnifeCounter.intance.setHitedKnife (totalSpawnKnife);
			currentKnife.ThrowKnife ();
			StartCoroutine(GenerateKnife ());
		}
	*/
		if (GameManager.FromAds > 2 && GameManager.Stage > 3)
		{
			AdsManager.instance.ShowInterstitial();
			GameManager.FromAds = 0;
		}

		if (gameOverStageLbl.gameObject.activeInHierarchy) UpdateGOstageLabel();
	}
	public void spawnCircle()
	{
		if (PlayerPrefs.GetInt("Vip", 0) == 0)
		{

			GameObject tempCircle;
			if (GameManager.Stage % 5 == 0)
			{
				Bosses b = BossPrefabs[Random.Range(0, BossPrefabs.Length)];
				tempCircle = Instantiate<Circle>(b.BossPrefab, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
				{
					currentBossName = Boss_en + b.Bossname_en;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
				{
					currentBossName = Boss_ru + b.Bossname_ru;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
				{
					currentBossName = Boss_sp + b.Bossname_sp;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
				{
					currentBossName = Boss_pt + b.Bossname_pt;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
				{
					currentBossName = Boss_ar + b.Bossname_ar;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
				{
					currentBossName = Boss_jp + b.Bossname_jp;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
				{
					currentBossName = Boss_cn + b.Bossname_cn
						;

				}

				UpdateLable();
				OnBossFightStart();
			}
			else
			{
				if (GameManager.Stage > 50)
				{
					tempCircle = Instantiate<Circle>(circlePrefabs[Random.Range(11, circlePrefabs.Length - 1)], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				}
				else
				{
					tempCircle = Instantiate<Circle>(circlePrefabs[GameManager.Stage - 1], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				}

			}

			tempCircle.transform.localScale = Vector3.one;
			float circleScale = (GameManager.ScreenWidth * circleWidthByScreen) / tempCircle.GetComponent<SpriteRenderer>().bounds.size.x;
			tempCircle.transform.localScale = Vector3.one * .2f;
			LeanTween.scale(tempCircle, new Vector3(circleScale, circleScale, circleScale), .3f).setEaseOutBounce();
			//tempCircle.transform.localScale = Vector3.one*circleScale;
			currentCircle = tempCircle.GetComponent<Circle>();
		}
        else
        {
			GameObject tempCircleVip;
			if (GameManager.Stage % 5 == 0)
			{
				Bosses b = BossPrefabsVip[Random.Range(0, BossPrefabsVip.Length)];
				tempCircleVip = Instantiate<Circle>(b.BossPrefab, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
				{
					currentBossName = Boss_en + b.Bossname_en;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
				{
					currentBossName = Boss_ru + b.Bossname_ru;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
				{
					currentBossName = Boss_sp + b.Bossname_sp;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
				{
					currentBossName = Boss_pt + b.Bossname_pt;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
				{
					currentBossName = Boss_ar + b.Bossname_ar;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
				{
					currentBossName = Boss_jp + b.Bossname_jp;

				}
				else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
				{
					currentBossName = Boss_cn + b.Bossname_cn
						;

				}

				UpdateLable();
				OnBossFightStart();
			}
			else
			{
				if (GameManager.Stage > 50)
				{
					tempCircleVip = Instantiate<Circle>(circlePrefabsVip[Random.Range(11, circlePrefabsVip.Length - 1)], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				}
				else
				{
					tempCircleVip = Instantiate<Circle>(circlePrefabsVip[GameManager.Stage - 1], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
				}

			}

			tempCircleVip.transform.localScale = Vector3.one;
			float circleScale = (GameManager.ScreenWidth * circleWidthByScreen) / tempCircleVip.GetComponent<SpriteRenderer>().bounds.size.x;
			tempCircleVip.transform.localScale = Vector3.one * .2f;
			LeanTween.scale(tempCircleVip, new Vector3(circleScale, circleScale, circleScale), .3f).setEaseOutBounce();
			//tempCircle.transform.localScale = Vector3.one*circleScale;
			currentCircle = tempCircleVip.GetComponent<Circle>();
		}
	}
	public IEnumerator OnBossFightStart()
	{
		bossFightStart.SetActive (true);
		SoundManager.instance.PlaySingle (bossFightStartSounds[Random.Range(0,bossFightEndSounds.Length-1)],1f);
		yield return new WaitForSeconds (2f);
		bossFightStart.SetActive (false);
		setupGame ();
	}

	public IEnumerator OnBossFightEnd()
	{
		bossFightEnd.SetActive (true);
		SoundManager.instance.PlaySingle (bossFightEndSounds[Random.Range(0,bossFightEndSounds.Length-1)],1f);
		yield return new WaitForSeconds (2f);

		bossFightEnd.SetActive (false);

		lastBossTag = currentBossTag;
		vipKnTrigg = false;
		setupGame();

		if (GameManager.Stage == 6 && currentBossTag != "BossVip" && (GameManager.OffFeedBacks < 3 || GameManager.FirstTimeBoss == 0))
		{
			StartCoroutine(BonusAndFeedBack());
		}
		Debug.LogWarning("Босс: " + currentBossTag);
		
		if (currentBossTag == "BossVip")
        {
			ShopItem shopItem = KnifeShop.intance.shopItems[KnifeShop.intance.shopItems.Count - 1];
			if (!shopItem.KnifeUnlock)
				StartCoroutine(VipBossDefender());
        }
		
	}

	public IEnumerator GenerateKnife()
	{
		//	yield return new WaitForSeconds(0.1f);

			yield return new WaitUntil(() =>
			{
				return KnifeSpawnPoint.childCount == 0;
			});

			if (currentCircle.totalKnife > totalSpawnKnife/* && !GameManager.isGameOver && !KnifeShop.inShopTrigger*/)
			{

				totalSpawnKnife++;
				GameObject tempKnife;
				if (GameManager.selectedKnifePrefab == null)
				{
					tempKnife = Instantiate<Knife>(knifePrefab, new Vector3(KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;
				}
				else
				{
						tempKnife = Instantiate<Knife>(GameManager.selectedKnifePrefab, new Vector3(KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;
				}
				tempKnife.transform.localScale = Vector3.one;
				float knifeScale = (GameManager.ScreenHeight * knifeHeightByScreen) / tempKnife.GetComponent<SpriteRenderer>().bounds.size.y;
				tempKnife.transform.localScale = Vector3.one * knifeScale;
				LeanTween.moveLocalY(tempKnife, 0, 0.1f);
				tempKnife.name = "Knife" + totalSpawnKnife;
				currentKnife = tempKnife.GetComponent<Knife>();
			}
	}
	public void NextLevel()
	{
		Debug.Log ("Level " + (GameManager.Stage + 1));
		if (currentCircle != null) {
			currentCircle.destroyMeAndAllKnives ();
		}
		if (GameManager.Stage % 5 == 0) {
			GameManager.Stage++;
			StartCoroutine (OnBossFightEnd ());

		} else {
			GameManager.Stage++;
			if (GameManager.Stage % 5 == 0) {
				StartCoroutine (OnBossFightStart ());
			} else {
				Invoke ("setupGame", .3f);
			}
		}
	}

	public IEnumerator BonusAndFeedBack()
    {
		GameManager.isGameOver = true;
		FeedbackOn = true;
		if (GameManager.FirstTimeBoss == 0 && currentBossTag == "Wood")
		{
			KnifeShop.intance.shopUIParent.SetActive(true);

			KnifeShop.intance.shopUIParent.transform.Find("Back_Btn").gameObject.GetComponent<Button>().interactable = false;
			KnifeShop.intance.shopUIParent.transform.Find("UnlockBtn").gameObject.GetComponent<Button>().interactable = false;
			KnifeShop.intance.shopUIParent.transform.Find("UnlockRandomBtn").gameObject.GetComponent<Button>().interactable = false;
			KnifeShop.intance.shopUIParent.transform.Find("ViewAdsButton").gameObject.GetComponent<Button>().interactable = false;

			KnifeShop.intance.UnlockRandomKnifeAfterBoss();
			GameManager.FirstTimeBoss = 1;
			PlayFabManager.instance.SetFirstTimeBoss();
		}

		if (GameManager.OffFeedBacks < 3)
		{
			GameManager.OffFeedBacks += 1;
			feedBack.SetActive(true);
		}

		feedBack.GetComponent<Button>().onClick.AddListener(delegate { FeedbackOn = false; PauseOff();  });
		feedBack.transform.Find("RateUs_pnl").transform.Find("Back").GetComponent<Button>().onClick.AddListener(delegate { FeedbackOn = false; PauseOff(); });

		yield return new WaitForSeconds(0.1f);

	}
	public IEnumerator VipBossDefender()
    {
		GameManager.isGameOver = true;
		KnifeShop.intance.UnlockVipKnife();		
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator currentShowingAdsPopup;
	public void GameOver()
	{
		GameManager.isGameOver = true;

		if (usedAdContinue) {
			showGameOverPopup ();
		} else {
			currentShowingAdsPopup = showAdPopup ();
			StartCoroutine (currentShowingAdsPopup);
		}
	}
	public IEnumerator showAdPopup()
	{
		adsShowView.SetActive (true);
		GameManager.FromAds += 1;
//		print(GameManager.FromAds);
		adSocreLbl.text = GameManager.Score+"";

		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			adStageLbl.text = Level_en + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			adStageLbl.text = Level_ru + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			adStageLbl.text = Level_sp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			adStageLbl.text = Level_pt + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			adStageLbl.text = Level_ar + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			adStageLbl.text = Level_jp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			adStageLbl.text = Level_cn + GameManager.Stage;

		}

		SoundManager.instance.PlayTimerSound ();
		for (float i=1f; i>0; i-=0.01f) 
		{
			adTimerImage.fillAmount =i;
			yield return new WaitForSeconds (0.1f);
		}
		CancleAdsShow ();
		SoundManager.instance.StopTimerSound ();
	}
	public void OnShowAds()
	{
		if (!freeCont)
		{
			doneWatchingAd = false;

			SoundManager.instance.StopTimerSound();
			SoundManager.instance.PlaybtnSfx();
			usedAdContinue = true;
			StopCoroutine(currentShowingAdsPopup);

			AdsManager.instance.ShowRewardedVideoRestart();
		}
        else
        {
			SoundManager.instance.StopTimerSound();
			StopCoroutine(currentShowingAdsPopup);
			AdShowSucessfully();
        }
	}
	public  void AdShowSucessfully()
    {
        adsShowView.SetActive(false);
        totalSpawnKnife--;
		GameManager.isGameOver = false;
		print (currentCircle.hitedKnife.Count);
		print (totalSpawnKnife);
		KnifeCounter.intance.setHitedKnife (totalSpawnKnife);
		if (KnifeSpawnPoint.childCount == 0) {		
			StartCoroutine (GenerateKnife ());
		}
	}

    public void PauseOff()
    {
			GameManager.isGameOver = false;

		if (GameManager.Stage % 5 == 1 && PlayerPrefs.GetInt("VipKnifeCounter", 19) != 39 && !vipKnTrigg && lastBossTag == "BossVip")
		{
			vipKnTrigg = true;
		}
		else
		{
			StartCoroutine(GenerateKnife());
		}
	}
    public  void CancleAdsShow()
	{
		SoundManager.instance.StopTimerSound ();
		SoundManager.instance.PlaybtnSfx ();
		StopCoroutine (currentShowingAdsPopup);
		adsShowView.SetActive (false);
		showGameOverPopup ();
	}
	public void showGameOverPopup()
	{
		gameOverView.SetActive (true);
		circle.SetActive(false);
		gameOverSocreLbl.text = GameManager.Score+"";

		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			gameOverStageLbl.text = Level_en + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			gameOverStageLbl.text = Level_ru + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			gameOverStageLbl.text = Level_sp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			gameOverStageLbl.text = Level_pt + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			gameOverStageLbl.text = Level_ar + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			gameOverStageLbl.text = Level_jp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			gameOverStageLbl.text = Level_cn + GameManager.Stage;

		}
		
		if (GameManager.Score >= GameManager.HighScore) {
			GameManager.HighScore = GameManager.Score;
			newBestScore.SetActive (true);
			if (GameManager.OffFeedBacks < 3 && GameManager.Score > 70)
			{
				GameManager.OffFeedBacks += 1;
				feedBack.SetActive(true);
			}
		} else {
			newBestScore.SetActive (false);
		}

        CUtils.ShowInterstitialAd();
	}
	public void OpenShop()
	{
		SoundManager.instance.PlaybtnSfx ();
		KnifeShop.intance.showShop ();
	}
	public void RestartGame()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneByName("GameScene");
	
	}
	public void BackToHome()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneByName("HomeScene");
	}
	public void FBClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        StartCoroutine(CROneStepSharing());
    }
	public void ShareClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        StartCoroutine(CROneStepSharing());
	}
	public void SettingClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		SettingUI.intance.showUI ();
	}

    IEnumerator CROneStepSharing()
    {
        yield return new WaitForEndOfFrame();
        MobileNativeShare.ShareScreenshot("screenshot", "");
    }

	void FindFeedback()
    {
		GameObject[] allGo = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (var o in allGo)
		{
			if (o.scene.name == "DontDestroyOnLoad" && o.name == "FeedBack")
			{
				feedBack = o;
			}
		}
	}

	private void UpdateGOstageLabel()
    {
		if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
		{
			gameOverStageLbl.text = Level_en + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
		{
			gameOverStageLbl.text = Level_ru + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
		{
			gameOverStageLbl.text = Level_sp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
		{
			gameOverStageLbl.text = Level_pt + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
		{
			gameOverStageLbl.text = Level_ar + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
		{
			gameOverStageLbl.text = Level_jp + GameManager.Stage;

		}
		else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
		{
			gameOverStageLbl.text = Level_cn + GameManager.Stage;

		}
	}
}



[System.Serializable]
public class Bosses{
	public string Bossname_en;
	public string Bossname_ru;
	public string Bossname_sp;
	public string Bossname_pt;
	public string Bossname_jp;
	public string Bossname_cn;
	public string Bossname_ar;


	public Circle BossPrefab;
}

