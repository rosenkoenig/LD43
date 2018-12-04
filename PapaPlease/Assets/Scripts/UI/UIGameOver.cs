using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour {

    public void Display ()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

	public void OnClick ()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
