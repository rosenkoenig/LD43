using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeLauncher : MonoBehaviour {

    public bool useDefault = false, shakeOnEnable = false;
    public ShakeOptions shakeOptions = new ShakeOptions();

    void OnEnable ()
    {
        if (shakeOnEnable) LaunchShake();
    }

    public void LaunchShake ()
    {
        CameraShaker camShaker = Camera.main.GetComponent<CameraShaker>();

        if (camShaker) camShaker.LaunchShake(useDefault ? null : shakeOptions);
    }
}
