using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour {

    [SerializeField]
    float fadeTime, displayDuration;

    float lastDisplayTime;

    [SerializeField]
    Text text;

    [SerializeField] Color _logStandardColor;
    [SerializeField] Color _logUrgentColor;

    bool isDisplayed = false;

    public void SetText(string newText, bool isUrgent = false)
    {
        if (isUrgent)
        {
            text.color = _logUrgentColor;
            text.fontStyle = FontStyle.Bold;
        }
        else
        {
            text.color = _logStandardColor;
            text.fontStyle = FontStyle.Normal;
        }

        text.text = newText;
        Display();
    }

    void Display()
    {
        isDisplayed = true;
        lastDisplayTime = Time.time;
        foreach (Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(1f, fadeTime, true);
        }
    }

    void Hide()
    {
        foreach (Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(0f, fadeTime, true);
        }
        isDisplayed = false;

        if (coroutine == null) coroutine = StartCoroutine(waitAndDestroy());
    }

    Coroutine coroutine;
    IEnumerator waitAndDestroy ()
    {
        yield return new WaitForSecondsRealtime(fadeTime);

        Destroy(gameObject);
    }

    void Update()
    {
        if (isDisplayed && Time.time - lastDisplayTime >= displayDuration)
        {
            Hide();
        }
    }
}
