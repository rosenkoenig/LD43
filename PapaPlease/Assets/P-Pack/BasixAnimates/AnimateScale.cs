using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/AnimateScale", 10)]
public class AnimateScale : BasixAnimate<Vector3> {

    public override void ApplyAnimate(float factor)
    {
        transform.localScale = Vector3.Lerp(startState, endState, factor);
    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        startState = transform.localScale;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        endState = transform.localScale;
    }

}
