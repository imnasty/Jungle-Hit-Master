using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bear.j.easy_dialog
{
    public class changingthelanguage : MonoBehaviour
    {
        public static float fade_time = 0;
        // Start is called before the first frame update
        void Start()
        {

        }
        public static void confirm_box()
        {
            GameObject go = Resources.Load<GameObject>("changingthelanguage");
            go.GetComponent<changingthelanguage>();

            Instantiate(go);


        }
        public static IEnumerator show(GameObject obj)
        {
            float from = 0;
            float to = 1;

            obj.SetActive(true);

            CanvasGroup canvas_group = obj.GetComponent<CanvasGroup>();

            float duration = changingthelanguage.fade_time;

            float elaspedTime = 0f;
            while (elaspedTime <= duration)
            {
                elaspedTime += Time.deltaTime;
                canvas_group.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
                yield return null;
            }
            canvas_group.alpha = to;
        }
        // Update is called once per frame
        void Update()
        {

        }
        void OnEnable()
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main;

            //show the confirm box
            StartCoroutine(changingthelanguage.show(this.gameObject));
        }

    }
}

