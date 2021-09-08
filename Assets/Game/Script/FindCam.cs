using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCam : MonoBehaviour
{
    private Camera cam;
    void FixedUpdate()
    {
        if (cam == null)
        {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            GetComponent<Canvas>().worldCamera = cam;
        }
    }
}
