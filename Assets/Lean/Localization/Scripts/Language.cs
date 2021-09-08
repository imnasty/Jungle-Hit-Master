using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean;

public class Language : MonoBehaviour
{
    void Start()
    {
        if (Application.systemLanguage.ToString() == "English")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "English";
        }
        else if (Application.systemLanguage.ToString() == "Russian")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Russian";
        }
        else if (Application.systemLanguage.ToString() == "Spanish")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Spanish";
        }
        else if (Application.systemLanguage.ToString() == "Portuguese")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Portuguese";
        }
        else if (Application.systemLanguage.ToString() == "Arabic")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Arabic";
        }
        else if (Application.systemLanguage.ToString() == "Japanese")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Japanese";
        }
        else if (Application.systemLanguage.ToString() == "Chinese")
        {
            Lean.Localization.LeanLocalization.currentLanguage = "Chinese";
        }

    }

    
}
