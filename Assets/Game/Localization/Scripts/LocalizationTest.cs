using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTest : MonoBehaviour
{
    //Options
    [SerializeField]
    private LocalizedText Options;
    [SerializeField]
    private LocalizedText Zvuk;
    [SerializeField]
    private LocalizedText Vibro;
    [SerializeField]
    private LocalizedText Language;
    [SerializeField]
    private LocalizedText Remove_ads;
    [SerializeField]
    private LocalizedText Restore;
    //Main menu
    [SerializeField]
    private LocalizedText Level;
    [SerializeField]
    private LocalizedText Play;
    [SerializeField]
    private LocalizedText Ochki;

    public void LocalizeText()
    {
        Options.Localize("Options");
        Zvuk.Localize("Zvuk");
        Vibro.Localize("Vibro");
        Language.Localize("Language");
        Remove_ads.Localize("Remove");
        Restore.Localize("Restore");
        Level.Localize("Level");
        Play.Localize("Play");
        Ochki.Localize("Ochki");
    }
}
