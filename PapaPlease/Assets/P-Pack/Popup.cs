using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {

    [SerializeField]
    protected float duration = 1f;

    [SerializeField]
    protected bool awaitsClick = false;

    [SerializeField]
    protected bool ledByButtons = false;

    [SerializeField]
    protected float fadeDuration = 0.3f;

    [SerializeField]
    protected bool pauseTime = false;

    [SerializeField]
    protected AutoInitOptions autoInit = AutoInitOptions.NoAutoInit;

    protected enum AutoInitOptions { NoAutoInit, OnEnable, OnStart }

    public System.Action OnPopupEnds = null;

	// Use this for initialization
	void Start () {
        if (autoInit == AutoInitOptions.OnStart) Init(null);
	}

    void OnEnable ()
    {
        if (autoInit == AutoInitOptions.OnEnable) Init(null);
    }

    void Update ()
    {
        if(awaitsClick)
        {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ClosePopup();
            }
        }
    }

    public void ClosePopup ()
    {
        if (fadeAndDeactivateCoroutine != null) StopCoroutine(fadeAndDeactivateCoroutine);
        fadeAndDeactivateCoroutine = StartCoroutine(fadeAndDeactivate());

        Debug.Log("Close Popup end");
    }

    public virtual void Init(object[] args)
    {
        if (fadeAndDeactivateCoroutine != null) StopCoroutine(fadeAndDeactivateCoroutine);
        SetOpaque();
        gameObject.SetActive(true);

        if (pauseTime) Time.timeScale = 0f;

        if(!awaitsClick && !ledByButtons)
        {
            if (fadeAndDeactivateCoroutine != null) StopCoroutine(fadeAndDeactivateCoroutine);
            fadeAndDeactivateCoroutine = StartCoroutine(fadeAndDeactivate());
        }
        
    }

    protected Coroutine fadeAndDeactivateCoroutine = null;
    protected IEnumerator fadeAndDeactivate ()
    {
        if( duration > 0f)
            yield return new WaitForSecondsRealtime(duration);
        
        FadeOut();

        if (OnPopupEnds != null)
        {
            OnPopupEnds();
            OnPopupEnds = null;
        }

        if (pauseTime) Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(fadeDuration);
        
        gameObject.SetActive(false);
    }

    void SetOpaque ()
    {
        foreach (Graphic graphic in GetComponentsInChildren<Graphic>())
        {
            graphic.CrossFadeAlpha(1f, 0f, true);
        }
    }

    void FadeOut ()
    {
        foreach(Graphic graphic in GetComponentsInChildren<Graphic>())
        {
            graphic.CrossFadeAlpha(0f, fadeDuration, true);
        }
    }
}
