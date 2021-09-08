using UnityEngine;

public class DonDestroy : MonoBehaviour
{
    private static DonDestroy instance;

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
