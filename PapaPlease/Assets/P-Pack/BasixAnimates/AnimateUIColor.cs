using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateUIColor : BasixAnimate<Color>
{
    [SerializeField]
    bool applyToChildren = false;

    Graphic[] targets;

    void Start ()
    {
        if (applyToChildren)
        {
            targets = GetComponentsInChildren<Graphic>();
        }
        else
        {
            targets = GetComponents<Graphic>();
        }
    }

    public override void ApplyAnimate(float factor)
    {
        if (targets == null) return;

        foreach (Graphic gr in targets)
            RealApplyAnimate(gr, factor);
    }

    void RealApplyAnimate (Graphic target, float factor)
    {
        target.color = Color.Lerp(startState, endState, factor);
    }
}
