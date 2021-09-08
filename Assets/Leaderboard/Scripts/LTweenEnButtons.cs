using System.Collections;
using UnityEngine;

public class LTweenEnButtons : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.localScale = Vector2.zero;
        StartCoroutine(EnDelay());
    }

    private IEnumerator EnDelay()
    {
        yield return new WaitForSeconds(.3f);
        gameObject.LeanScale(Vector2.one, .5f).setEaseOutBack();
    }
}
