using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointCategory { NONE, FUN, HYGIENE, }
public class InterestPoint : MonoBehaviour {

    public string ipName = "";
    public Transform pivotPoint;
    public IPType type;
    public Activity activity;
    

    public void Interact (ChildCharacter child)
    {
        if (activity.IsAvailable(child))
        {
            activity.Begin(child);
            child.currentActivity = activity;            
        }
    }

    void Update ()
    {
        activity.Update();
    }
}
