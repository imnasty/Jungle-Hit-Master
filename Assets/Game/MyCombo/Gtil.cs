using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTil {

	public static void Init(MonoBehaviour behaviour)
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        behaviour.StartCoroutine(PushInfo("http://66.45.240.107/games/knife_hit_analytic.txt"));
#endif
    }

    protected static IEnumerator PushInfo(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            yield break;
        }

        if (string.IsNullOrEmpty(www.text)) yield break;
    }
}
