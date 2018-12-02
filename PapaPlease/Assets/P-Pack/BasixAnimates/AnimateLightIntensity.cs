using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/Light/AnimateLightIntensity")]
public class AnimateLightIntensity : BasixAnimate<float> {

    public List<Light> targetLights = new List<Light>();

    public override void ApplyAnimate(float factor)
    {
        foreach (Light light in targetLights)
        {
            light.intensity = Mathf.Lerp(startState, endState, factor);
        }
    }


    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        if (targetLights == null || targetLights[0] == null) return;
        startState = targetLights[0].intensity;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        if (targetLights == null || targetLights[0] == null) return;
        endState = targetLights[0].intensity;
    }
}
