using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenGameOverPbtn : MonoBehaviour
{
	private GameObject[] allGO;
    
	public void ActivateGOpanel()
    {
		allGO = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (var o in allGO)
		{
			if (o.scene.name == "GameScene" && o.name == "GameOver View")
			{
					o.SetActive(true);
			}
		}
	}

}
