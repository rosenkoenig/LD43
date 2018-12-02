using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour {

    Transform cam;

	// Use this for initialization
	void Start () {
        cam = GameMaster.Instance.player.GetPlayerHeadBehaviour.GetCamera.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.position;
        transform.rotation = cam.rotation;
	}
}
