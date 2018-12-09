using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocked : MonoBehaviour {

    [SerializeField]
    GameObject visualOpen = null;

    [SerializeField]
    GameObject visualClosed = null;

    [SerializeField] float delayBeforeOverture;
    public void Open ()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    IEnumerator OpenDoorCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeOverture);
        visualOpen.SetActive(true);
        visualClosed.SetActive(false);
    }
    


    public void Close ()
    {
        visualOpen.SetActive(false);
        visualClosed.SetActive(true);
    }
}
