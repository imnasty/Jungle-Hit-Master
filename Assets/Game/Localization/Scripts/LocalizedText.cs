using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocalizedText : MonoBehaviour
{
    private Text text;
    private string key;


    // Start is called before the first frame update
    void Start()
    {
        Localize();
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }

    private void Init() 
    {
        text = GetComponent<Text>();
        key = text.text;
    }

    public void Localize(string newKey = null)
    {
        if (text == null)
            Init();

        if (newKey != null)
            key = newKey;
        text.text = LocalizationManager.GetTranslate(key);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
