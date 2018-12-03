using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {

    [SerializeField]
    AK.Wwise.Event soundEvent;

	// Use this for initialization
	void Start () {
        soundEvent.Post(gameObject);
    }
}
