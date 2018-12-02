using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/3D/AnimatePosition")]
public class AnimatePosition : BasixAnimate<Vector3>
{
    public bool local = true;

    public override void ApplyAnimate(float factor)
    {
        if (local)
        {
            transform.localPosition = Vector3.Lerp(startState, endState, factor);
        }
        else
        {
            transform.position = Vector3.Lerp(startState, endState, factor);
        }

    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        startState = local ? transform.localPosition : transform.position;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        endState = local ? transform.localPosition : transform.position;
    }
}
