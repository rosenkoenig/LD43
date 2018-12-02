using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BasixAnimates/3D/AnimateColor")]
public class AnimateColor : BasixAnimate<Color>
{
    Material mat = null;

    public bool alphaOnly = false;

    public override void ApplyAnimate(float factor)
    {
        if(mat == null)
        {
            mat = GetComponent<Renderer>().material;
        }

        if (alphaOnly)
        {
            Color targetColor = mat.color;
            targetColor.a = Mathf.Lerp(startState.a, endState.b, factor);

            mat.color = targetColor;
        }
        else
        {
            mat.color = Color.Lerp(startState, endState, factor);
        }
    }

    [ContextMenu("Use Current As Start")]
    void UseCurrentAsStart()
    {
        Material tempMat = null;
        if (mat == null)
        {
            tempMat = GetComponent<Renderer>().material;
            startState = tempMat.color;
        }
        else
        {
            startState = mat.color;
        }
    }

    [ContextMenu("Use Current As Target")]
    void UseCurrentAsTarget()
    {
        Material tempMat = null;
        if (mat == null)
        {
            tempMat = GetComponent<Renderer>().material;
            endState = tempMat.color;
        }
        else
        {
            endState = mat.color;
        }
    }
}
