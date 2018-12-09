using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{

    Transform cam;

    // Use this for initialization
    void Start()
    {
        if (GameMaster.Instance != null)
            cam = GameMaster.Instance.player.GetPlayerHeadBehaviour.GetCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            transform.position = cam.position;
            transform.rotation = cam.rotation;
        }
        else
        { 
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
