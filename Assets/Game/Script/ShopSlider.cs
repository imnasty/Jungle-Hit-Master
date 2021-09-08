using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlider : MonoBehaviour
{
    public GameObject[] str;
    public Color chooseColor;
    private Color currColor;

    private void Awake()
    {
        currColor = chooseColor;
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (PlayerPrefs.GetInt("panelID") == i)
            {
                str[i].GetComponent<Image>().color = currColor;
                
            }
            if (PlayerPrefs.GetInt("panelID") != i)
            {
                str[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                
            }
            
        }
        
    }
}
