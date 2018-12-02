using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/Sound/AnimateVolume")]
public class AnimateVolume : BasixAnimate<float>
{
    
    public AudioSource src = null;

    public override void ApplyAnimate(float factor)
    {
        if (src == null) src = GetComponentInChildren<AudioSource>();

        src.volume = Mathf.Lerp(startState, endState, factor);
    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        if (src == null) return;
        startState = src.volume;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        if (src == null) return;
        endState = src.volume;
    }
}
