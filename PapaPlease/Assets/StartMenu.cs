using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    [SerializeField]
    Animator charAnimator;
    [SerializeField] AK.Wwise.Event MenuMusicStartEvent = null;
    [SerializeField] AK.Wwise.Event MenuMusicStopEvent = null;
    [SerializeField] AK.Wwise.Event PlayGameEvent = null;
    void Start ()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        if (MenuMusicStartEvent != null)
            MenuMusicStartEvent.Post(gameObject);
        charAnimator.Play("HipHop");
    }

	public void OnClick ()
    {
        if (MenuMusicStopEvent != null)
            MenuMusicStopEvent.Post(gameObject);
        if (PlayGameEvent != null)
            PlayGameEvent.Post(gameObject);
        Load();
    }

    void Load()
    {
        
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }
}
