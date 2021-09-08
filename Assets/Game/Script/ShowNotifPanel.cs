using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNotifPanel : MonoBehaviour
{
    private GameObject notifPanel;

    private void Start()
    {
        GameObject[] allGO = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject go in allGO)
        {
            if (go.scene.name == "DontDestroyOnLoad" && go.name == "ShowGiftNotifPanel") notifPanel = go;
        }
    }

    public void Show(int delay)
    {
        StartCoroutine(ShowPanel(delay));
    }

    private IEnumerator ShowPanel(int delay)
    {
        yield return new WaitForSeconds(delay);
        notifPanel.SetActive(true);
    }
}
