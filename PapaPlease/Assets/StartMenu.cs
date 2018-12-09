using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    Animator charAnimator;
    [SerializeField] AK.Wwise.Event MenuMusicStartEvent = null;
    [SerializeField] AK.Wwise.Event MenuMusicStopEvent = null;
    [SerializeField] AK.Wwise.Event PlayGameEvent = null;

    [SerializeField] UnityEngine.Object _menuScene;
    [SerializeField] UnityEngine.Object _mainScene;
    [SerializeField] UnityEngine.Object _graphismScene;
    [SerializeField] UnityEngine.Object _soundScene;

    [SerializeField] string _menuSceneName;
    [SerializeField] string _mainSceneName;
    [SerializeField] string _graphismSceneName;
    [SerializeField] string _soundSceneName;
    private void Awake()
    {
        SceneManager.LoadScene(_soundSceneName, LoadSceneMode.Additive);
    }
    void Start()
    {
        StartCoroutine(PlayMusic());
        charAnimator.Play("HipHop");
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(0.2f);

        if (MenuMusicStartEvent != null)
            MenuMusicStartEvent.Post(gameObject);
    }


    public void OnClick()
    {
        if (MenuMusicStopEvent != null)
            MenuMusicStopEvent.Post(gameObject);
        if (PlayGameEvent != null)
            PlayGameEvent.Post(gameObject);
        Load();
    }

    void Load()
    {
        SceneManager.LoadScene(_mainSceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene(_graphismSceneName, LoadSceneMode.Additive);
        //SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }
}
