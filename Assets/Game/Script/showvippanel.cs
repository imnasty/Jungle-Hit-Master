using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showvippanel : MonoBehaviour
{
    public GameObject vippanel;
    public static UnityEngine.Object Find(string name, System.Type type)
    {
        UnityEngine.Object[] objects = Resources.FindObjectsOfTypeAll(type);
        foreach (GameObject obj in objects)
        {
            if (obj.name == name && obj.scene.name == "DontDestroyOnLoad")
            {
                return obj;
            }
        }
        return null;

    }
    public static GameObject Find(string name)
    {
        return Find(name, typeof(GameObject)) as GameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        vippanel=Find("VIPpanelScene");
    }
public void Show()
{
vippanel.SetActive(true);
}

}
