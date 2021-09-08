using UnityEngine;
using UnityEditor;

public class MoanaWindowEditor
{
    [MenuItem("Bogdanum Games/Clear all playerprefs")]
    static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [MenuItem("Bogdanum Games/Add 1000 candys")]
    static void AddCandy()
    {
        GameManager.Candy += 1000;
        PlayerPrefs.Save();
    }

    [MenuItem("Bogdanum Games/Set candy to 0")]
    static void SetBalanceZero()
    {
        GameManager.Candy = 0;
        PlayerPrefs.Save();
    }
}