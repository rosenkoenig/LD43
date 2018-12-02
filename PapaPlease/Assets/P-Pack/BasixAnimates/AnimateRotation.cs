using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/AnimateRotation", 9)]
public class AnimateRotation : BasixAnimate<Vector3>
{
    public bool local = false;

    public override void ApplyAnimate(float factor)
    {
        if(local)
        {
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(startState, endState, factor));
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startState, endState, factor));
        }
    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        startState = local ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        endState = local ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
    }
}
