using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : MonoBehaviour {

    [SerializeField]
    float fadeDuration = 1f;
    

    public void Begin ()
    {
        gameObject.SetActive(true);
        foreach (Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(0f, 0f, true);
            gr.CrossFadeAlpha(1f, fadeDuration, true);
        }
    }

    public void End ()
    {
        gameObject.SetActive(true);

        foreach (Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(1f, 0f, true);
            gr.CrossFadeAlpha(0f, fadeDuration, true);
        }

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(waitAndDeactivate());
    }

    Coroutine coroutine;
    IEnumerator waitAndDeactivate ()
    {
        yield return new WaitForSeconds(fadeDuration);

        gameObject.SetActive(false);
    }
}
