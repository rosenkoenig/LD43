using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour {

    public Transform needle = null;

    public void UpdateDayCompletion (float ratio)
    {
        float zRot = Mathf.Lerp(0f, 180f, ratio) ;
        needle.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRot));
    }

    public void SetActive (bool state)
    {
        foreach(Graphic gr in GetComponentsInChildren<Graphic>())
        {
            gr.CrossFadeAlpha(state ? 1f : 0f, .5f, true);
        }
    }
}
