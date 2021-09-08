using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	public static DontDestroy instance;
	 private void Awake()
	{
		if (instance != null)
		{
			DestroyImmediate(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);


		}
	}
}
