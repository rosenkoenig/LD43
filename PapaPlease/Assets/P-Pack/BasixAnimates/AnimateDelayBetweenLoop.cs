using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundItem))]
[AddComponentMenu("BasixAnimates/Sound/AnimateDelayBetweenLoop")]
public class AnimateDelayBetweenLoop : BasixAnimate<float>
{
    public SoundItem item = null;

    public override void ApplyAnimate(float factor)
    {
        if (item == null) item = GetComponentInChildren<SoundItem>();

        item.delayBetweenLoop = Mathf.Lerp(startState, endState, factor);
    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        startState = item.delayBetweenLoop;
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        endState = item.delayBetweenLoop;
    }
}
