using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/Light/AnimateLightColor")]
public class AnimateLightColor : BasixAnimate<Color>
{
    public List<Light> targetLights = new List<Light>();

    public override void ApplyAnimate(float factor)
    {
       foreach(Light light in targetLights)
        {
            light.color = Color.Lerp(startState, endState, factor);
        }
    }


    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        if (targetLights == null || targetLights[0] == null) return;
        startState = targetLights[0].color;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        if (targetLights == null || targetLights[0] == null) return;
        endState = targetLights[0].color;
    }
}
