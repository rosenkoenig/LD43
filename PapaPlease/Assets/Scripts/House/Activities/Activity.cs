using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityHolder
{
    public float startTime = 0f;
    public Character character;
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
    public string animStateName = "";

    public System.Action OnBegin, OnEnd;

    public virtual bool IsAvailable (Character character)
    {
        return holders.Count < maxHolder && state != ActivityState.COMPLETE;
    }
    public virtual bool IsAvailable()
    {
        return holders.Count < maxHolder && state != ActivityState.COMPLETE;
    }

    public virtual void Begin (Character character)
    {
        ActivityHolder holder = new ActivityHolder();
        holder.character = character;
        holder.startTime = Time.time;

        holders.Add(holder);


        SetState(ActivityState.RUNNING);

        ChildCharacter child = character.GetComponent<ChildCharacter>();
        if (child)
        {
            child.StartAnimState(animStateName);
        }

        if (OnBegin != null) OnBegin();
    }

    protected virtual void End (Character character)
    {
        ActivityHolder holder = GetHolderForCharacter(character);
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

        character.OnActivityEnds();

        ChildCharacter child = character.GetComponent<ChildCharacter>();
        if (child)
        {
            child.StartAnimState(animStateName+"_End");
        }

        if (OnEnd != null) OnEnd();
    }

    ActivityHolder GetHolderForCharacter (Character character)
    {
        return holders.Find(x => x.character == character);
    }

    protected virtual void SetState (ActivityState newState)
    {
        if (state != newState)
            state = newState;

        Debug.Log("Activity state = " + newState, this);
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
