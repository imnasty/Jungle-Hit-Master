using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KnifeShop : MonoBehaviour {
	[Header("just_unlock")]
	public string just_unlock_en;
	public string just_unlock_ru;
	public string just_unlock_sp;
	public string just_unlock_pt;
	public string just_unlock_cn;
	public string just_unlock_jp;
	public string just_unlock_ar;

	[Header("ups! no candy")]
	public string no_candy_en;
	public string no_candy_ru;
	public string no_candy_sp;
	public string no_candy_pt;
	public string no_candy_cn;
	public string no_candy_jp;
	public string no_candy_ar;


	[Header("Setting")]
	public GameObject shopUIParent;
	public ShopItem shopKnifePrefab;
	public Transform shopPageContent;
	public Transform shopPageContent2;
	public Text unlockKnifeCounterLbl;
	public Button unlockNowBtn, unlockRandomBtn;
	public Image selectedKnifeImageUnlock;
	public Image selectedKnifeImageLock;
	public GameObject knifeBackeffect1, knifeBackeffect2;
	public int UnlockPrice = 250, UnlockRandomPrice = 250;
	public List<Knife> shopKnifeList;
	public List<Knife> shopKnifeListVip;                           
	public GameObject shopItem;
	public GameObject BlockPanelVip;
	

	private bool checkAfterBoss = false;
	private bool CheckHidePanel = true;
	public static bool inShopTrigger;
	//public GameObject feedback;
	private GameObject vipKnifePanel;
	private GameObject equipVipKnife;
	private GameObject closeVipPanel;
	private GameObject selectedKnifeImageVip;
	private ShopItem selectV;

	public static KnifeShop intance;
	public static ShopItem selectedItem;
	public AudioClip onUnlocksfx, RandomUnlockSfx;
	public List<ShopItem> shopItems;

	public static bool setupTrigger = false;
	public static bool setupBlockTrigger = false;

	ShopItem  selectedShopItem
	{
		get
		{ 
			return shopItems.Find ((obj) => { return obj.selected; });
		}
	}
	
	void Start() 
	{
		if (intance == null) 
		{
			intance = this;
			if (setupTrigger)
			{
				SetupShop();
			}
		}
	}

    private void Update()
    {
		if (PlayerPrefs.GetInt("ShopPage", 0) == 1)
        {
			shopUIParent.transform.Find("UnlockRandomBtn").gameObject.GetComponent<Button>().interactable = false;
			shopUIParent.transform.Find("UnlockBtn").gameObject.GetComponent<Button>().interactable = false;
		}
        else
        {
			if (!inShopTrigger) {
				shopUIParent.transform.Find("UnlockRandomBtn").gameObject.GetComponent<Button>().interactable = true;
				shopUIParent.transform.Find("UnlockBtn").gameObject.GetComponent<Button>().interactable = true;
			}
		}
	}

    [ContextMenu("Clear PlayerPref")]
	void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll ();
	}

	[ContextMenu("Add Candy")]
		void addCandy()
		{
		GameManager.Candy += 500;
		}

	public void showShop()
	{

		shopUIParent.SetActive(true);
		

			if (!shopItems[GameManager.SelectedKnifeIndex].selected)
			{
				shopItems[GameManager.SelectedKnifeIndex].selected = true;
			}

			if (PlayerPrefs.GetInt("Vip", 0) == 0)                                 
		{
				for (int i = 0; i < shopKnifeList.Count; i++)
					shopItems[i].GetComponent<Button>().interactable = true;
			}
			else
			{
				for (int i = 0; i < shopKnifeListVip.Count; i++)
						shopItems[i].GetComponent<Button>().interactable = true;
			}

		UpdateUI ();

        CUtils.ShowInterstitialAd();
	}
	public void SetupShop ()
	{
		
		unlockNowBtn.GetComponentInChildren<Text> ().text = UnlockPrice + "";
		unlockRandomBtn.GetComponentInChildren<Text> ().text = UnlockRandomPrice + "";

		shopItems = new List<ShopItem> ();

		if (PlayerPrefs.GetInt("Vip", 0) == 0)
		{

			for (int i = 0; i < shopKnifeList.Count; i++)
			{

				ShopItem temp = Instantiate<ShopItem>(shopKnifePrefab, shopPageContent);
				temp.setup(i, this);
				temp.name = i + "";
				shopItems.Add(temp);

			}
			shopPageContent2.gameObject.GetComponent<GridLayoutGroup>().enabled = false;
			Instantiate(BlockPanelVip, shopPageContent2);
			
		}
        else
        {
			shopItems.Clear();
			foreach (Transform ob in shopPageContent)
            {
				Destroy(ob.gameObject);
            }
			foreach (Transform ob in shopPageContent2)                                  
			{
				Destroy(ob.gameObject);
			}
			for (int i = 0; i < shopKnifeListVip.Count; i++)
			{
				if (i > 19)
				{
					shopPageContent2.gameObject.GetComponent<GridLayoutGroup>().enabled = true;
					
					ShopItem temp2 = Instantiate<ShopItem>(shopKnifePrefab, shopPageContent2);
					temp2.setup(i, this);
					temp2.name = i + "";
					shopItems.Add(temp2);
				}
				else
				{
					
					ShopItem temp = Instantiate<ShopItem>(shopKnifePrefab, shopPageContent);
					temp.setup(i, this);
					temp.name = i + "";
					shopItems.Add(temp);
				}
			}
		}
					shopItems[GameManager.SelectedKnifeIndex].OnClick();
		
	}
	public void UpdateUI()
	{

			selectedKnifeImageUnlock.sprite = selectedShopItem.knifeImage.sprite;
			selectedKnifeImageLock.sprite = selectedShopItem.knifeImage.sprite;
			selectedKnifeImageUnlock.gameObject.SetActive(selectedShopItem.KnifeUnlock);
			selectedKnifeImageLock.gameObject.SetActive(!selectedShopItem.KnifeUnlock);

			knifeBackeffect1.SetActive(selectedShopItem.KnifeUnlock);
			knifeBackeffect2.SetActive(selectedShopItem.KnifeUnlock);

		if (PlayerPrefs.GetInt("Vip", 0) == 0)                                        
		{

			int unlockCount = 0;
			if (shopItems.FindAll((obj) => { return obj.KnifeUnlock; }) != null)
			{
				unlockCount = shopItems.FindAll((obj) =>
				{
					return obj.KnifeUnlock;
				}).Count;
			}
			unlockKnifeCounterLbl.text = unlockCount + "/" + shopKnifeList.Count;

			if (unlockCount == shopKnifeList.Count)
			{
				unlockNowBtn.interactable = false;
				unlockRandomBtn.interactable = false;
			}

			GameManager.selectedKnifePrefab = shopKnifeList[GameManager.SelectedKnifeIndex];
		}
        else
        {
			int unlockCount = 0;
			if (shopItems.FindAll((obj) => { return obj.KnifeUnlock; }) != null)
			{
				unlockCount = shopItems.FindAll((obj) =>
				{
					return obj.KnifeUnlock;
				}).Count;
			}
			unlockKnifeCounterLbl.text = unlockCount + "/" + shopKnifeListVip.Count;

			if (unlockCount == shopKnifeListVip.Count)
			{
				unlockNowBtn.interactable = false;
				unlockRandomBtn.interactable = false;
			}

			GameManager.selectedKnifePrefab = shopKnifeListVip[GameManager.SelectedKnifeIndex];
		}
			if (MainMenu.intance != null)
			{
				MainMenu.intance.selectedKnifeImage.sprite = GameManager.selectedKnifePrefab.GetComponent<SpriteRenderer>().sprite;
			}
		
	}

	public void UnlockKnife()
	{
		if (unlockingRandom)
			return;
		
		 if (selectedShopItem.KnifeUnlock) 
		{
			if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
			{
				Toast.instance.ShowMessage(just_unlock_en);

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
			{
				Toast.instance.ShowMessage(just_unlock_ru);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
			{
				Toast.instance.ShowMessage(just_unlock_sp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
			{
				Toast.instance.ShowMessage(just_unlock_pt);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
			{
				Toast.instance.ShowMessage(just_unlock_ar);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
			{
				Toast.instance.ShowMessage(just_unlock_jp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
			{
				Toast.instance.ShowMessage(just_unlock_cn);
			}

			SoundManager.instance.PlaybtnSfx ();
			return;
		}


		else if (GameManager.Candy < UnlockPrice)
		{
			if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
			{
				Toast.instance.ShowMessage(no_candy_en);

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
			{
				Toast.instance.ShowMessage(no_candy_ru);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
			{
				Toast.instance.ShowMessage(no_candy_sp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
			{
				Toast.instance.ShowMessage(no_candy_pt);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
			{
				Toast.instance.ShowMessage(no_candy_ar);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
			{
				Toast.instance.ShowMessage(no_candy_jp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
			{
				Toast.instance.ShowMessage(no_candy_cn);
			}

			SoundManager.instance.PlaybtnSfx();
			return;
		}
		 
		GameManager.Candy -= UnlockPrice;
		
			selectedShopItem.KnifeUnlock = true;
			selectedShopItem.UpdateUIColor();
			GameManager.SelectedKnifeIndex = selectedShopItem.index;

		UpdateUI ();
		PlayFabManager.instance.SetUserUnlockedKnifes();   // <<<<<<<<>>>>>>>>
		SoundManager.instance.PlaySingle (onUnlocksfx);
	/*	if (GameManager.OffFeedBacks < 3)
		{
			GameManager.OffFeedBacks += 1;
			feedback.SetActive(true);
		}
	*/
	}
	public void UnlockVipKnife()
    {
		int currIndex = GameManager.SelectedKnifeIndex;
		GameObject[] allGo = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (var o in allGo)
		{
			if (o.scene.name == "GameScene" && o.name == "VipKnifePanel")
			{
				vipKnifePanel = o;
			} else if (o.scene.name == "GameScene" && o.name == "TakeKnife")
            {
				equipVipKnife = o;
            }
			else if (o.scene.name == "GameScene" && o.name == "CloseVipPanel")
			{
				closeVipPanel = o;
			}
			else if (o.scene.name == "GameScene" && o.name == "SelectedKnifeImageUnlockVip")
            {
				selectedKnifeImageVip = o;
			}
		}
		vipKnifePanel.SetActive(true);
		inShopTrigger = true;
		int counter = PlayerPrefs.GetInt("VipKnifeCounter", 19);
		if (counter != shopKnifeListVip.Count)
		{
			counter++;
			PlayerPrefs.SetInt("VipKnifeCounter", counter);
			PlayFabManager.instance.SetUserVipKnifeCounter(counter);

			ShopItem select = shopItems[counter];

			select.KnifeUnlock = true;
			select.UpdateUIColor();
			selectV = select;
			PlayFabManager.instance.SetUserUnlockedKnifes();   // <<<<<<<<>>>>>>>>
		}
		selectedKnifeImageVip.GetComponent<Image>().sprite = shopKnifeListVip[counter].gameObject.GetComponent<SpriteRenderer>().sprite;
		equipVipKnife.GetComponent<Button>().onClick.AddListener(delegate
		{
			foreach (Transform knife in GamePlayManager.instance.KnifeSpawnPoint)
            {
				knife.gameObject.GetComponent<SpriteRenderer>().sprite = selectV.knifeImage.sprite;
            }
			GameManager.SelectedKnifeIndex = selectV.index;
			GameManager.selectedKnifePrefab = shopKnifeListVip[GameManager.SelectedKnifeIndex];
			UpdateUI();
			inShopTrigger = false;
			vipKnifePanel.SetActive(false);
			GamePlayManager.instance.PauseOff();
		});
		closeVipPanel.GetComponent<Button>().onClick.AddListener(delegate
		{
			GameManager.SelectedKnifeIndex = currIndex;
			GameManager.selectedKnifePrefab = shopKnifeListVip[GameManager.SelectedKnifeIndex];
			UpdateUI();
			inShopTrigger = false;
			vipKnifePanel.SetActive(false);
			GamePlayManager.instance.PauseOff();
		});

	}

	bool unlockingRandom=false;

	public void UnlockRandomKnife()
	{
		if (GameManager.Candy < UnlockRandomPrice) 
		{
			if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "English")
			{
				Toast.instance.ShowMessage(no_candy_en);

			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Russian")
			{
				Toast.instance.ShowMessage(no_candy_ru);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Spanish")
			{
				Toast.instance.ShowMessage(no_candy_sp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Portuguese")
			{
				Toast.instance.ShowMessage(no_candy_pt);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Arabic")
			{
				Toast.instance.ShowMessage(no_candy_ar);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Japanese")
			{
				Toast.instance.ShowMessage(no_candy_jp);
			}
			else if (Lean.Localization.LeanLocalization.currentLanguage.ToString() == "Chinese")
			{
				Toast.instance.ShowMessage(no_candy_cn);
			}

			SoundManager.instance.PlaybtnSfx ();
			return;
		}
		if(unlockingRandom)
		{
			return;
		}
			StartCoroutine(UnlockRandomCoKnife());
		
	}

	public void UnlockRandomKnifeAfterBoss()
	{
		
		HideGameView();

        if (!CheckHidePanel)
        {
			ActivateGameView();
		}
		else
        {
			shopUIParent.transform.Find("Back_Btn").gameObject.GetComponent<Button>().interactable = false;
			shopUIParent.transform.Find("UnlockBtn").gameObject.GetComponent<Button>().interactable = false;
			shopUIParent.transform.Find("UnlockRandomBtn").gameObject.GetComponent<Button>().interactable = false;
			shopUIParent.transform.Find("ViewAdsButton").gameObject.GetComponent<Button>().interactable = false;
			checkAfterBoss = true;
			CheckHidePanel = false;
			inShopTrigger = true;
			StartCoroutine(UnlockRandomCoKnife());

		}
		

	}

	void HideGameView()
	{
		GameObject[] allGo = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (var o in allGo)
		{
			if (o.scene.name == "GameScene" && (o.name == "gameView" || o.name == "circleSpawnPoint" || o.name == "KnifeSpawnPoint"))
			{
				o.SetActive(false);
			}

		}
	
	}
	
	void ActivateGameView()
    {
		GameObject[] allGo = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (var o in allGo)
		{
			if (o.scene.name == "GameScene" && (o.name == "gameView" || o.name == "circleSpawnPoint" || o.name == "KnifeSpawnPoint"))
			{
				o.SetActive(true);
			}

		}
	}

	IEnumerator UnlockRandomCoKnife()
	{
			if (!checkAfterBoss)
			{
				GameManager.Candy -= UnlockRandomPrice;
			}

			for (int i = 0; i < shopKnifeList.Count; i++)
				shopItems[i].GetComponent<Button>().interactable = false;
			unlockingRandom = true;
			List<ShopItem> lockedItems = shopItems.FindAll((obj) => { return !obj.KnifeUnlock; });
			List<ShopItem> allItemsInFpage = new List<ShopItem>();
			for (int i = 0; i < lockedItems.Count; i++)
			{
				if (lockedItems[i].index < 20)
				{
					allItemsInFpage.Add(lockedItems[i]);
				}
			}

			ShopItem randomSelect = null;
			for (int i = 0; i < allItemsInFpage.Count * 2; i++)
			{
				randomSelect = allItemsInFpage[Random.Range(0, allItemsInFpage.Count)];

				if (!randomSelect.selected)
				{
					randomSelect.selected = true;
					SoundManager.instance.PlaySingle(RandomUnlockSfx);
				}
				yield return new WaitForSeconds(.2f);
			}

			randomSelect.KnifeUnlock = true;
			randomSelect.UpdateUIColor();
			
			GameManager.SelectedKnifeIndex = randomSelect.index;
			GameManager.selectedKnifePrefab = shopKnifeList[GameManager.SelectedKnifeIndex];
			UpdateUI();

			PlayFabManager.instance.SetUserUnlockedKnifes();   // <<<<<<<<>>>>>>>>

			unlockingRandom = false;
			SoundManager.instance.PlaySingle(onUnlocksfx);
			if (!checkAfterBoss)
			{
				for (int i = 0; i < shopKnifeList.Count; i++)
					shopItems[i].GetComponent<Button>().interactable = true;
			}

			yield return new WaitForSeconds(2f);

			
			if (checkAfterBoss)
			{
				Debug.LogWarning("Current Stage " + GameManager.Stage);
				checkAfterBoss = false;

				shopUIParent.transform.Find("Back_Btn").gameObject.GetComponent<Button>().interactable = true;
				shopUIParent.transform.Find("UnlockBtn").gameObject.GetComponent<Button>().interactable = true;
				shopUIParent.transform.Find("UnlockRandomBtn").gameObject.GetComponent<Button>().interactable = true;
				shopUIParent.transform.Find("ViewAdsButton").gameObject.GetComponent<Button>().interactable = true;

				foreach (Transform knife in GamePlayManager.instance.KnifeSpawnPoint)
				{
					knife.gameObject.GetComponent<SpriteRenderer>().sprite = randomSelect.knifeImage.sprite;
				}

				shopUIParent.SetActive(false);
				UnlockRandomKnifeAfterBoss();
				inShopTrigger = false;
				GamePlayManager.instance.PauseOff();

				GamePlayManager.instance.UpdateLable();

			}
		

	}
	
}
