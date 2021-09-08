using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    public ScrollRect scrollRect;

    [Header("Все карточки миров")]
    public List<GameObject> allPages;
    [Range(0f,40f)]public int ползунок;
   
    [Range(0f, 20f)] public float snapSpeed;

    private Vector2[] pagePos;
    private GameObject[] worlds;
  
    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedWorldID;
    private bool isScrolling;
    public Text vipText;
    private void Start()
    {
        scrollRect.inertia = false;
        contentRect = GetComponent<RectTransform>();
        worlds = new GameObject[allPages.Count];
        pagePos = new Vector2[allPages.Count];

        for (int i = 0; i < allPages.Count; i++)
        {
            worlds[i] = allPages[i];
            if (i == 0) continue;
            worlds[i].transform.localPosition = new Vector2(worlds[i - 1].transform.localPosition.x +
                allPages[i].GetComponent<RectTransform>().sizeDelta.x + ползунок, worlds[i].transform.localPosition.y);
            pagePos[i] = -worlds[i].transform.localPosition;
            
        }
    }

    private void FixedUpdate()
    {
        float nearestPos = float.MaxValue;  

       for (int i = 0; i < allPages.Count; i++)
       {

           float distance = Mathf.Abs(contentRect.anchoredPosition.x - pagePos[i].x);
           if (distance != 0) scrollRect.inertia = true;

            if (distance < nearestPos)
                {
                    nearestPos = distance;

                    selectedWorldID = i;
                    PlayerPrefs.SetInt("panelID", i);
                    PlayerPrefs.SetInt("ShopPage", i);
                }

            vipText.text = selectedWorldID == 1 ? "VIP" : Lean.Localization.LeanLocalization.GetTranslationText("getforcandy");
        }
        if (isScrolling) return;

        contentVector.x = Mathf.SmoothDamp(contentRect.anchoredPosition.x, pagePos[selectedWorldID].x, ref snapSpeed, 0.25f);
        contentRect.anchoredPosition = contentVector;

        if (Mathf.Abs(contentRect.anchoredPosition.x - pagePos[selectedWorldID].x) < ползунок - 10)
        {
            scrollRect.inertia = false;
        }
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }

}
