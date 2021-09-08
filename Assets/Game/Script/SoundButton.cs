using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { SoundManager.instance.PlaybtnSfx(); });
    }
}
