using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField]
    public Text label;
    // Start is called before the first frame update
    void Start()
    {
        label.text = Application.systemLanguage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
