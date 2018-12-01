using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointCategory { NONE, FUN, HYGIENE, }
public class InterestPoint : MonoBehaviour {

    public string ipName = "";
    public Transform pivotPoint;
    public IPType type;
    public Activity activity;
    public bool onlyUsableByChild = false;
    

    public bool Interact (Character character)
    {
        if(onlyUsableByChild && character.GetComponent<PlayerBehaviour>())
        {
            return false;
        }


        bool interacts = activity.IsAvailable(character);
        if (interacts)
        {
            activity.Begin(character);
            character.currentActivity = activity; 
        }

        return interacts;
    }

    void Update ()
    {
        activity.Update();
    }
}
