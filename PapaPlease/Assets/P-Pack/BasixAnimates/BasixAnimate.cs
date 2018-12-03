using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasixAnimate<T> : MonoBehaviour {
    
    enum Status { Waiting, Running, Paused, Finished }
    Status status = Status.Waiting;

    public enum BasixAnimateStyle
    {
        Once,
        Loop,
        PingPong
    }
    public BasixAnimateStyle style = BasixAnimateStyle.Once;

    public bool startOnEnable, setStartStateOnAwake, ignoreTimeScale = false;

    public float duration = 1f;

    public T startState, endState;

    public AnimationCurve curve = AnimationCurve.EaseInOut(0f,0f,1f,1f);

    public System.Action OnAnimationEnds = null;

    void Awake ()
    {
        if (setStartStateOnAwake) ApplyAnimate(0f);
    }

    void OnEnable ()
    {
        if (startOnEnable) StartAnimating();
    }

    public void StartAnimating ()
    {
        status = Status.Running;
        lerpFactor = 0f;
        sign = 1f;
    }

    float sign = 1f;
    float lerpFactor = 0f;
    void SimultateLerpFactor ()
    {
        lerpFactor = Mathf.Clamp01(lerpFactor + (((ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) / duration) * sign));

        if(lerpFactor >= 1f)
        {
            switch(style)
            {
                case BasixAnimateStyle.Loop:
                    lerpFactor = 0f;
                    break;
                case BasixAnimateStyle.PingPong:
                    sign *= -1f;
                    break;
                case BasixAnimateStyle.Once:
                    status = Status.Finished;
                    if (OnAnimationEnds != null) OnAnimationEnds();
                    break;
            }
        }
        else if (lerpFactor <= 0f && style == BasixAnimateStyle.PingPong)
        {
            sign *= -1f;
        }
    }

    void Update ()
    {
        if(status == Status.Running)
        {
            SimultateLerpFactor();
            ApplyAnimate(curve.Evaluate(lerpFactor));
        }
    }

    public abstract void ApplyAnimate(float factor);

    public void StopAnimating ()
    {
        if (status != Status.Waiting)
        {
            status = Status.Finished;
            if (OnAnimationEnds != null) OnAnimationEnds();
        }
    }

    public void ResetAnimating ()
    {
        StopAnimating();
        StartAnimating();
    }

    public void PauseAnimating ()
    {
        if (status != Status.Finished && status != Status.Waiting)
            status = Status.Paused;
    }

    public void ResumeAnimating ()
    {
        if(status != Status.Finished && status != Status.Waiting)
            status = Status.Running;
    }

    //debug
    /*void OnGUI()
    {

        if (GUI.Button(new Rect(new Vector2(0f, 0f), new Vector2(200f, 30f)), "PauseAnimating"))
        {
            PauseAnimating();
        }
        if (GUI.Button(new Rect(new Vector2(0f, 35f), new Vector2(200f, 30f)), "ResumeAnimating"))
        {
            ResumeAnimating();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 70f), new Vector2(200f, 30f)), "StopAnimating"))
        {
            StopAnimating();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 70f + 35f), new Vector2(200f, 30f)), "ResetAnimating"))
        {
            ResetAnimating();
        }

        if (GUI.Button(new Rect(new Vector2(0f, 70f + 35f + 35f), new Vector2(200f, 30f)), "StartAnimating"))
        {
            StartAnimating();
        }
    }*/
}
