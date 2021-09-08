using System;
using System.Collections;
using UnityEngine;

public class LTweenEnDis : MonoBehaviour
{
   [Header("Родительский объект, перед закрытием которого будет выполняться анимация")]
   [SerializeField] private GameObject parentGO;
    public bool isParentNotNull = false;

    private void OnEnable()
    {
        gameObject.transform.localScale = Vector2.zero;
        gameObject.LeanScale(Vector2.one, .5f).setEaseOutBack();
    }

    public void Disable() { StartCoroutine(WaitCloseAnim()); }

    private IEnumerator WaitCloseAnim()
    {
        gameObject.LeanScale(Vector2.zero, .4f).setEaseInBack();
        yield return new WaitForSeconds(.4f);
        if (isParentNotNull) { parentGO.SetActive(false); }
        else { gameObject.SetActive(false); }
    }
}
