using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas : MonoBehaviour
{
    public GameObject Panel;
    public void openPanel()
    {
       if (Panel != null)
        {
           
            Panel.SetActive(true);
        }

    }
    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
        }

    }

}