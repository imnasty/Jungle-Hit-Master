using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LocalizationManager : MonoBehaviour
{
    public static int SelectedLanguage { get; private set; }

    public static event LanguageChangeHandler OnLanguageChange;
    public delegate void LanguageChangeHandler();
    
    private static Dictionary<string, List<string>> localization;

    [SerializeField]
    private TextAsset textfile;
    // Start is called before the first frame update
    private void Awake()
    {
        if (localization == null)
            Loadlocalization();
    }

    public void SetLanguage(int id) 
    {
        SelectedLanguage = id;
        OnLanguageChange?.Invoke();
    }

    private void Loadlocalization() 
    {
        localization = new Dictionary<string, List<string>>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textfile.text);

        foreach (XmlNode key in xmlDocument["Keys"].ChildNodes)
        {
            string keyStr = key.Attributes["Name"].Value;

            var values = new List<string>();
            foreach (XmlNode traslate in key["Translates"].ChildNodes)
                values.Add(traslate.InnerText);

            localization[keyStr] = values;
        }
    }
    public static string GetTranslate(string key, int languageId = -1) 
    {
        if (languageId == -1)
            languageId = SelectedLanguage;

        if (localization.ContainsKey(key))
            return localization[key][languageId];

        return key;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
