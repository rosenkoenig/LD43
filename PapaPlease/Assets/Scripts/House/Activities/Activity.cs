using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityHolder
{
    public float startTime = 0f;
    public ChildCharacter child;
}

public enum ActivityState { WAITING, RUNNING, COMPLETE }
[System.Serializable]
public class Activity : MonoBehaviour {

    ActivityState state = ActivityState.WAITING;
    public ActivityState State {  get { return state; } }
    public bool isRunning { get { return state == ActivityState.RUNNING; } }
    public List<ActivityHolder> holders = new List<ActivityHolder>();
    public int maxHolder = 1;
    public bool doOnce = false;

    public System.Action OnBegin, OnEnd;

    public virtual bool IsAvailable (ChildCharacter child)
    {
        return holders.Count < maxHolder && state != ActivityState.COMPLETE;
    }

    public virtual void Begin (ChildCharacter child)
    {
        ActivityHolder holder = new ActivityHolder();
        holder.child = child;
        holder.startTime = Time.time;

        holders.Add(holder);


        SetState(ActivityState.RUNNING);


        if (OnBegin != null) OnBegin();
    }

    protected virtual void End (ChildCharacter child)
    {
        ActivityHolder holder = GetHolderForChild(child);
        if(holder != null)
        {
            holders.Remove(holder);
        }


        if(doOnce)
        {
            SetState(ActivityState.COMPLETE);
        }
        else
        {
            if (holders.Count <= 0)
            {
                SetState(ActivityState.WAITING);
            }
        }

        child.OnActivityEnds();
        if (OnEnd != null) OnEnd();
    }

    ActivityHolder GetHolderForChild (ChildCharacter child)
    {
        return holders.Find(x => x.child == child);
    }

    protected virtual void SetState (ActivityState newState)
    {
        if (state != newState)
            state = newState;
    }

    public void Update ()
    {
        UpdateState();
    }

    protected virtual void UpdateState ()
    {
        switch(state)
        {
            case ActivityState.RUNNING:
                UpdateRunningState();
                break;
        }
    }

    protected virtual void UpdateRunningState ()
    {
        
    }
}
