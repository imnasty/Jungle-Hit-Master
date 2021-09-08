using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayFabManager : MonoBehaviour
{
    [Header("Название ключа, в котором лежит лучший счет")] public string BestScoreKey = "Player's HighScore";
    [Header("Название таблицы лидеров")] public string leaderboardName = "BestPlayers";
    [Header("Название таблицы лидеров для читеров")] public string cheatersLeaderboardName = "Cheaters";

    #region Менеджер
    private const string playManagerObjName = "MainMenu";
    private MainMenu playManager;
    #endregion

    #region UI

    #region Всплывающее окно для ввода ника после первой смерти
    private GameObject inputName;
    private InputField inputFieldWidow;
    private GameObject inpWindowErrorText;
    private GameObject inpWindowForb;
    private Button SubmtBtn;
    #endregion

    #region Поле для ввода/отображения ника в меню
    private GameObject menuInpFieldObj;
    private InputField inputField;
    private Text nameMenu;
    private Text tryInpName;
    #endregion

    #region Лидерборд
    private GameObject leaderboard;
    private GameObject controlBlockPanel;
    [Header("префаб строки(Player)")] public GameObject rowPrefab;
    private Transform leaderboardContent;
    [Header("Количество игроков в таблице")] [Range(10, 1000)] public int playersCountAll = 100;
    [Header("Количество ближних игроков в таблице")] [Range(5, 1000)] public int playersCountAround = 100;
    [Header("Цвет выделения игрока в таблице")] public Color color;
    private Color GoldColor = new Color32(255, 215, 0, 255);
    private Color SilverColor = new Color32(192, 192, 192, 255);
    private Color BronzeColor = new Color32(205, 127, 50, 255);
    private Button showAroundPlayers;
    private Button showAllPlayers;
    #endregion

    private Button Leaderboard;
    private Text PlayerPosButton;
    private Button ReloadSceneBTN;
    private GameObject CheckIntCon;

    #endregion

    #region Временные массивы и тп
    private GameObject[] allGO;
    private string[] forbiddenWords;
    private TextAsset textAsset;
    string loggedInPlayfabId;
    private List<int> playerKnifes;
    public bool IAP;
    #endregion
    [HideInInspector] public string userPlayFabID;
    public static PlayFabManager instance;

    public static int OldPosition
    {
        get { return PlayerPrefs.GetInt("OldPosition"); }
        private set { PlayerPrefs.SetInt("OldPosition", value); }
       
    }

    private void Awake()
    {
        instance = this;
        FindObjects();
    }

    void Start()
    {
        controlBlockPanel.SetActive(true);
        StartCoroutine(LoadDataError());
        Login();
        ReloadSceneBTN.onClick.AddListener(delegate { ReloadApp();/*ReloadScene(0);*/ });
        SubmtBtn.onClick.AddListener(delegate { SubmitNameButtonWindow(); });
        Leaderboard.onClick.AddListener(delegate
        {
            if (PlayerPrefs.GetInt("Cheater", 0) == 1) { GetCheatersLeaderboard(); } else { GetLeaderboard(); }
        });
        showAroundPlayers.onClick.AddListener(delegate
        {
            if (PlayerPrefs.GetInt("Cheater", 0) == 1) { GetCheatersLeaderboardAroundPlayer(); } else { GetLeaderboardAroundPlayer(); }
            showAllPlayers.gameObject.SetActive(true);
            showAroundPlayers.gameObject.SetActive(false);
        });
        showAllPlayers.onClick.AddListener(delegate
        {
            if (PlayerPrefs.GetInt("Cheater", 0) == 1) { GetCheatersLeaderboard(); } else { GetLeaderboard(); }
            showAroundPlayers.gameObject.SetActive(true);
            showAllPlayers.gameObject.SetActive(false);
        });

        if (PlayerPrefs.GetInt("Cheater", 0) == 1) leaderboardName = "ABOBA";
    }

    void FindObjects()
    {
        allGO = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject go in allGO)
        {
            if (go.gameObject.scene.name != null)
            {
                switch (go.name)
                {
                    case "leaderboardPanel":
                        leaderboard = go;
                        break;
                    case "leaderboardContent":
                        leaderboardContent = go.GetComponent<Transform>();
                        break;
                    case "InputName":
                        inputField = go.GetComponent<InputField>();
                        break;
                    case "playerName":
                        nameMenu = go.GetComponent<Text>();
                        break;
                    case "InputNameWindow":
                        inputName = go;
                        break;
                    case "InputField":
                        inputFieldWidow = go.GetComponent<InputField>();
                        break;
                    case "Leaderboard":
                        Leaderboard = go.GetComponent<Button>();
                        break;
                    case "showAroundPlayers":
                        showAroundPlayers = go.GetComponent<Button>();
                        break;
                    case "showAllPlayers":
                        showAllPlayers = go.GetComponent<Button>();
                        break;
                    case "SubmtBtn":
                        SubmtBtn = go.GetComponent<Button>();
                        break;
                    case "ControlBlockPanel":
                        controlBlockPanel = go;
                        break;
                    case "inpWindowError":
                        inpWindowErrorText = go;
                        break;
                    case "inpWindowForb":
                        inpWindowForb = go;
                        break;
                    case playManagerObjName:
                        playManager = go.GetComponent<MainMenu>();
                        break;
                    case "tryInpName":
                        tryInpName = go.GetComponent<Text>();
                        break;
                    case "Player Name":
                        menuInpFieldObj = go;
                        break;
                    case "PlayerPosButton":
                        PlayerPosButton = go.GetComponent<Text>();
                        break;
                    case "ReloadSceneCBP":
                        ReloadSceneBTN = go.GetComponent<Button>();
                        break;
                    case "CheckIntCon":
                        CheckIntCon = go;
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public string CheckUserID()
    {
        string deviceID;
        if (PlayerPrefs.GetString("DeviceID", "") == "")
        {
            deviceID = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("DeviceID", deviceID);
        }
        else
        {
            deviceID = PlayerPrefs.GetString("DeviceID");
        }
        return deviceID;
    }

    bool CheckHighOrLowIqUser(string input)
    {
        return input.IndexOf(' ') > -1 ? true : false;
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = CheckUserID(),
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        loggedInPlayfabId = result.PlayFabId;
        Debug.LogWarning("Successful login/accaunt create!");

        if (result.InfoResultPayload.PlayerProfile != null)
        {
            if (nameMenu == null) { FindObjects(); }
            nameMenu.text = result.InfoResultPayload.PlayerProfile.DisplayName;
            if (result.InfoResultPayload.PlayerProfile.DisplayName == null) { inputName.SetActive(true); }
        }
        else
        {
            if (inputField.text == "" && playManager.menuMaxScore.text != "0")
            {
                inputName.SetActive(true);
            }
        }

        CheckVipSub();
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = result.PlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Vip"))
            {
                nameMenu.gameObject.GetComponent<Animator>().enabled = true;
                inputField.gameObject.transform.Find("tryInpName").gameObject.GetComponent<Animator>().enabled = true;
            }
        }, (error) =>
        {
        });
        //remove_ads.instance.SetDefault(result.PlayFabId);
        GetVipStatus(result.PlayFabId);
        GetRemoveAds(result.PlayFabId);
        GetUserData(result.PlayFabId);
        GetUserCandy(result.PlayFabId);
        userPlayFabID = result.PlayFabId;
        GetUserMaxStage(result.PlayFabId);
        GetUserVipKnifeCounter(result.PlayFabId);
        GetFirstTimeBoss(result.PlayFabId);
        GetUserUnlockedKnifes(result.PlayFabId);
        GetUnlockedKnifesList();
        //        Debug.LogError("Vip состояние в success: " + PlayerPrefs.GetInt("Vip"));
        //      Debug.LogError("CheckByStateVip: " + PurchaseManager.CheckBuyState("vip"));
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
        inpWindowErrorText.SetActive(true);
        inputField.text = "" + NameGenerator();
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = PlayerPrefs.GetInt("Cheater", 0) == 1 ? 0 : score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    public void SendCheatersLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = cheatersLeaderboardName,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }


    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Sucessfull leaderboard send");
    }

    #region ЛИДЕРБОРД ДЛЯ НОРМАЛЬНЫХ ЛЮДЕЙ

    public void GetLeaderboard()
    {
        leaderboard.SetActive(true);

        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = playersCountAll
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in leaderboardContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, leaderboardContent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Image trofImage = newGo.GetComponentInChildren<Image>();

            if (item.Position == 0) { trofImage.color = GoldColor; }
            else
            if (item.Position == 1) { trofImage.color = SilverColor; }
            else
            if (item.Position == 2) { trofImage.color = BronzeColor; }
            if (item.PlayFabId == loggedInPlayfabId)
            {
#if UNITY_ANDROID
                if (PlayerPrefs.HasKey("OldPosition") && item.Position > OldPosition) NotificationManager.instance.SendPositionNotification();
                OldPosition = item.Position;
#endif
            }

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = item.PlayFabId,
                Keys = null
            }, result =>
            {
                if (result.Data != null && result.Data.ContainsKey("Vip"))
                {
                    if (item.PlayFabId == loggedInPlayfabId)
                    {
                        texts[0].color = color;
                        texts[2].color = color;
                    }
                    newGo.GetComponentInChildren<Animator>().enabled = true;
                }
                else
                {
                    if (item.PlayFabId == loggedInPlayfabId)
                    {
                        texts[0].color = color;
                        texts[1].color = color;
                        texts[2].color = color;
                    }
                }
            }, (error) =>
            {
            });

        }
    }

    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = playersCountAround
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        if (leaderboardContent == null) { FindObjects(); }
        if (loggedInPlayfabId == "") { Login(); }

        foreach (Transform item in leaderboardContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, leaderboardContent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Image trofImage = newGo.GetComponentInChildren<Image>();

            if (item.Position == 0) { trofImage.color = GoldColor; }
            else
            if (item.Position == 1) { trofImage.color = SilverColor; }
            else
            if (item.Position == 2) { trofImage.color = BronzeColor; }

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = item.PlayFabId,
                Keys = null
            }, result =>
            {
                if (result.Data != null && result.Data.ContainsKey("Vip"))
                {
                    if (item.PlayFabId == loggedInPlayfabId)
                    {
                        texts[0].color = color;
                        texts[2].color = color;
                    }
                    newGo.GetComponentInChildren<Animator>().enabled = true;
                }
                else
                {
                    if (item.PlayFabId == loggedInPlayfabId)
                    {
                        texts[0].color = color;
                        texts[1].color = color;
                        texts[2].color = color;
                    }
                }
            }, (error) =>
            {
            });
        }
    }

#endregion ЛИДЕРБОРД ДЛЯ НОРМАЛЬНЫХ ЛЮДЕЙ

    public void GetCheatersLeaderboard()
    {
        leaderboard.SetActive(true);

        var request = new GetLeaderboardRequest
        {
            StatisticName = cheatersLeaderboardName,
            StartPosition = 0,
            MaxResultsCount = playersCountAll
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public void GetCheatersLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = cheatersLeaderboardName,
            MaxResultsCount = playersCountAround
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }



    public void SubmitNameButton()
    {
        if (inputField == null) { FindObjects(); }
        if (inputField.text == "" || CheckHighOrLowIqUser(inputField.text))
        {
            inputField.text = "" + NameGenerator();
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = CheckForbiddenWords(inputField.text),
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name");
        inputField.text = result.DisplayName;
        nameMenu.text = result.DisplayName;
        tryInpName.text = result.DisplayName;
    }

    public void SubmitNameButtonWindow()
    {
        if (inputFieldWidow == null) { FindObjects(); }

        if (inputFieldWidow.text == "" || CheckHighOrLowIqUser(inputFieldWidow.text))
        {
            inputFieldWidow.text = "" + NameGenerator();
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = CheckForbiddenWords(inputFieldWidow.text),
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateWindow, OnError);

    }

    void OnDisplayNameUpdateWindow(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated window display name");
        Debug.LogWarning(result.DisplayName);
        if (result.DisplayName == inputFieldWidow.text)
            inputName.GetComponentInChildren<LTweenEnDis>().Disable();
        //inputName.SetActive(false);
        inputField.text = result.DisplayName;
        nameMenu.text = result.DisplayName;
        tryInpName.text = result.DisplayName;
    }


    void GetFuckingBestScore()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = playersCountAll
        };
        PlayFabClientAPI.GetLeaderboard(request, GetFuckingNigerBestScore, OnError);
    }

    void GetFuckingNigerBestScore(GetLeaderboardResult result)
    {
        if (loggedInPlayfabId == "") { Login(); }

        foreach (var item in result.Leaderboard)
        {
            if (item.PlayFabId == loggedInPlayfabId)
            {
                PlayerPrefs.SetInt(BestScoreKey, item.StatValue);
                playManager.menuMaxScore.text = PlayerPrefs.GetInt(BestScoreKey).ToString();

                PlayerPosButton.text = (item.Position + 1).ToString();
            }
        }
        //        controlBlockPanel.SetActive(false);
    }

    void GetFuckingCheaterBestScore()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = cheatersLeaderboardName,
            StartPosition = 0,
            MaxResultsCount = playersCountAll
        };
        PlayFabClientAPI.GetLeaderboard(request, GetFuckingNigerBestScore, OnError);
    }


    public string NameGenerator()
    {
        char[] vowels = "aeuoyi".ToCharArray();
        char[] consonants = "qwrtpsdfghjklzxcvbnm".ToCharArray();

        char[] newNickLength = new char[Random.Range(3, 10)];
        StringBuilder newNick = new StringBuilder();

        while (newNick.Length < newNickLength.Length)
        {
            bool firstVowel = Random.Range(0, 2) == 0 ? true : false;

            if (firstVowel)
            {
                newNick.Append(vowels[Random.Range(0, vowels.Length)]);
                newNick.Append(consonants[Random.Range(0, consonants.Length)]);
            }
            else
            {
                newNick.Append(consonants[Random.Range(0, consonants.Length)]);
                newNick.Append(vowels[Random.Range(0, vowels.Length)]);
            }
        }
        if (newNickLength.Length % 2 != 0) newNick.Remove(newNick.Length - 1, 1);
        newNick[0] = char.ToUpper(newNick[0]);
        nameMenu.text = newNick.ToString();
        return CheckForbiddenWords(newNick.ToString());
    }

    public string CheckForbiddenWords(string username)
    {
        textAsset = Resources.Load("ForbiddenWords/ForbiddenWords") as TextAsset;
        forbiddenWords = textAsset.text.Split('\n');
        foreach (string line in forbiddenWords)
        {
            string pattern = @"\S*" + line + @"\S*";
            if (Regex.IsMatch(username, pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled))
            {
                Debug.LogWarning("Ник " + username + " запрещен!");
                inpWindowForb.SetActive(true);
                return NameGenerator();

            }
        }
        textAsset = Resources.Load("ForbiddenWords/ForbiddenWordsArab") as TextAsset;
        forbiddenWords = textAsset.text.Split('\n');
        foreach (string line in forbiddenWords)
        {
            if (username.Contains(line))
            {
                Debug.LogWarning("Ник " + username + " запрещен!");
                inpWindowForb.SetActive(true);
                return NameGenerator();
            }
        }
        return username;
    }

    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"Cheater", "true"}
            }
        },
        result => PlayerPrefs.SetInt("Cheater", 1),
        error =>
        {
        });
    }

    void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey("Cheater"))
            {
                Debug.LogWarning("No Cheater");
                PlayerPrefs.SetInt("Cheater", 0);
                GetFuckingBestScore();
            }
            else
            {
                PlayerPrefs.SetInt("Cheater", 1);
                GetFuckingCheaterBestScore();
            }
        }, (error) =>
        {
        });
    }

    public void SetUserUnlockedKnifes()
    {
        GetUnlockedKnifesList();
        string stringPlKnifes = string.Join("", playerKnifes);

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "Knifes", stringPlKnifes}
            }
        },
        result => { },
        error =>
        {
        });
    }

    void GetUserUnlockedKnifes(string myPayFabID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPayFabID,
            Keys = null
        },
        result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Knifes"))
            {
                string val = result.Data["Knifes"].Value;
                if (!KnifeShop.setupTrigger)
                {
                    KnifeShop.intance.SetupShop();
                    KnifeShop.setupTrigger = true;
                    KnifeShop.setupBlockTrigger = true;
                }
                Debug.LogWarning("Открытые ножи на сервере: " + val);
                var i = -1;
                foreach (char c in val)
                {
                    i++;
                    KnifeShop.intance.shopItems[i].KnifeUnlock = c == '0' ? false : true;
                    KnifeShop.intance.shopItems[i].setup(i, KnifeShop.intance);
                    // Debug.LogWarning("Knife " + i + " unlocked: " + KnifeShop.intance.shopItems[i].KnifeUnlock);
                }

            }
            if (!KnifeShop.setupTrigger && !KnifeShop.setupBlockTrigger)
            {
                KnifeShop.intance.SetupShop();
                KnifeShop.setupTrigger = true;
                KnifeShop.setupBlockTrigger = true;
            }

            controlBlockPanel.SetActive(false);
            StopCoroutine(LoadDataError());
        },
        error =>
        {
        });
    }

    public void SetUserCandy()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "Candy", GameManager.Candy.ToString() }
            }
        },
      result => { },
      error =>
      {
      });
    }

    void GetUserCandy(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Candy"))
            {
                if (!PlayerPrefs.HasKey("Player's Candy"))
                {
                    string candyOnServer = result.Data["Candy"].Value;
                    GameManager.Candy = int.Parse(candyOnServer);
                }
            }
        }, (error) =>
        {
        });
    }

    public void SetUserMaxStage(int stage)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "MaxStage", stage.ToString() },
               { "VipKnifeCounter", PlayerPrefs.GetInt("VipKnifeCounter", 19).ToString() }
            }
        },
      result => { },
      error =>
      {
      });
    }

    void GetUserMaxStage(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("MaxStage"))
            {
                if (!PlayerPrefs.HasKey("Player's MaxStage"))
                {
                    string maxStageOnServer = result.Data["MaxStage"].Value;
                    //GameManager.MaxStage = int.Parse(maxStageOnServer);
                    PlayerPrefs.SetInt("Player's MaxStage", int.Parse(maxStageOnServer));
                    playManager.menuMaxStage.text = GameManager.MaxStage.ToString();
                }
                if (!PlayerPrefs.HasKey("VipKnifeCounter"))
                {
                    string vipKnifeCounterOnServer = result.Data["VipKnifeCounter"].Value;
                    PlayerPrefs.SetInt("VipKnifeCounter", int.Parse(vipKnifeCounterOnServer));
                }
            }
        }, (error) =>
        {
        });
    }

    public void SetUserVipKnifeCounter(int counter)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "VipKnifeCounter", counter.ToString() }
            }
        },
      result => { },
      error =>
      {
      });
    }

    void GetUserVipKnifeCounter(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("VipKnifeCounter"))
            {
                if (!PlayerPrefs.HasKey("VipKnifeCounter"))
                {
                    string vipKnifeCounterOnServer = result.Data["VipKnifeCounter"].Value;
                    PlayerPrefs.SetInt("VipKnifeCounter", int.Parse(vipKnifeCounterOnServer));
                }
            }
        }, (error) =>
        {
        });
    }

    public void SetFirstTimeBoss()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "FirstTimeBoss", "Награда получена" }
            }
        },
      result => { },
      error =>
      {
      });
    }

    void GetFirstTimeBoss(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("FirstTimeBoss"))
            {
                GameManager.FirstTimeBoss = 1;
                GameManager.OffFeedBacks = 3;
            }
        }, (error) =>
        {
        });
    }

    public void SetVipStatus(bool trigger)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Permission = UserDataPermission.Public,
            Data = new Dictionary<string, string>()
            {
               { "Vip", trigger.ToString() }
            }
        },
      result => { },
      error =>
      {
      });
    }

    public void GetVipStatus(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Vip"))
            {
                string vipStatusOnServer = result.Data["Vip"].Value;
                PlayerPrefs.SetInt("Vip", vipStatusOnServer.Equals("True") ? 1 : 0);
            }
        }, (error) =>
        {
        });
    }

    public void SetRemoveAds()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               { "Remove Ads", "true" }
            }
        },
      result => { },
      error =>
      {
      });
    }

    public void GetRemoveAds(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Remove Ads"))
            {
                PlayerPrefs.SetInt("ads", 0);
            }
        }, (error) =>
        {
        });
    }

    void GetUnlockedKnifesList()
    {
        // Debug.LogError("Вызов GetUnlockedKnifes. VIP: " + PlayerPrefs.GetInt("Vip", 0));

        if (PlayerPrefs.GetInt("Vip", 0) == 1)
        {
            playerKnifes = new List<int>(40);
            for (int i = 0; i < 40; i++)
            {
                if (i == 0) { playerKnifes.Add(1); }
                else
                {
                    if (PlayerPrefs.HasKey("KnifeUnlock_" + i))
                    {
                        int knife = PlayerPrefs.GetInt("KnifeUnlock_" + i, 0) % 2 == 0 ? 0 : 1;
                        playerKnifes.Add(knife);

                    }
                    else { playerKnifes.Add(0); }
                }
            }
        }
        else if (PlayerPrefs.GetInt("Vip", 0) == 0)
        {
            playerKnifes = new List<int>(20);
            for (int i = 0; i < 20; i++)
            {
                if (i == 0) { playerKnifes.Add(1); }
                else
                {
                    if (PlayerPrefs.HasKey("KnifeUnlock_" + i))
                    {
                        int knife = PlayerPrefs.GetInt("KnifeUnlock_" + i, 0) % 2 == 0 ? 0 : 1;
                        playerKnifes.Add(knife);
                    }
                    else { playerKnifes.Add(0); }
                }
            }

        }
    }

    public void RemoveData(string myPlayFabId, string Key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        },
        result =>
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                KeysToRemove = new List<string>()
                {
                    "Vip"
                }
            },
            result => { },
            error => { });
        },
        error => { });

    }

    public void ReloadScene(int index)
    {
        SceneManager.LoadScene(index);
    }


    void CheckVipSub()
    {
        if (IAP)
        if (!PurchaseManager.CheckBuyState("vip"))
        {
            // Debug.LogError("Вызов проверки подписки");
            RemoveData(userPlayFabID, "Vip");
            PlayerPrefs.SetInt("Vip", 0);
            //  SetUserUnlockedKnifes();
            //  SetUserVipKnifeCounter(PlayerPrefs.GetInt("VipKnifeCounter", 19));

        }
    }

    void ReloadApp()
    {
        //System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }

    private IEnumerator LoadDataError()
    {
        yield return new WaitForSeconds(10);
        if (controlBlockPanel.activeSelf)
        {
            ReloadSceneBTN.gameObject.SetActive(true);
            CheckIntCon.SetActive(true);
        }
    }
}