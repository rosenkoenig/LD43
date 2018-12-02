using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/AnimateCameraFoV", 12)]
public class AnimateCameraFoV : BasixAnimate<float>
{
    public Camera cam = null;
    public override void ApplyAnimate(float factor)
    {
        if (cam == null) cam = GetComponent<Camera>();

        cam.fieldOfView = Mathf.Lerp(startState, endState, factor);
    }



    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        startState = cam.fieldOfView;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        endState = cam.fieldOfView;
    }
}
