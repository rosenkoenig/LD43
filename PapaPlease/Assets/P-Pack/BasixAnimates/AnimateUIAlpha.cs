using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BasixAnimates/UI/AnimateUIAlpha")]
public class AnimateUIAlpha : BasixAnimate<float> {

    public bool includeChilds = false;

    List<Graphic> graphics = null;

    override public void ApplyAnimate(float factor)
    {
        //populate the list of targets only once
        if(graphics == null)
        {
            graphics = new List<Graphic>();

            if(!includeChilds)
            {
                foreach (Graphic gr in GetComponents<Graphic>())
                {
                    graphics.Add(gr);
                }
            }
            else
            {
                foreach (Graphic gr in GetComponentsInChildren<Graphic>())
                {
                    graphics.Add(gr);
                }
            }            
        }

        //apply the animation to this list
        foreach(Graphic gr in graphics)
        {
            gr.CrossFadeAlpha(Mathf.Lerp(startState, endState, factor), 0f, true);
        }
    }

}
