using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityHolder
{
    public float startTime = 0f;
    public float completionPercentage = 0f;
    public Character character;

    public float GetActivityModifiersRatios(List<ActivityModifier> actMods)
    {
        float toReturn = 0;
        foreach (var curActMod in actMods)
        {
            toReturn += curActMod._factor * (character.statsContainer.GetAChildStatValueRatio(curActMod._childStat));
        }
        return toReturn;
    }

}

public enum ActivityState { WAITING, RUNNING, COMPLETE }
[System.Serializable]
public class Activity : MonoBehaviour
{

    ActivityState curActState = ActivityState.WAITING;
    public ActivityState State { get { return curActState; } }
    public bool isRunning { get { return curActState == ActivityState.RUNNING; } }
    public List<ActivityHolder> holders = new List<ActivityHolder>();
    public int maxHolder = 1;
    public bool doOnce = false;
    public string animStateName = "";

    [SerializeField] float _activityResetDelay = 60f;
    
    [SerializeField] float curActivityResetDelay;

    protected IPType _inheritedIPType;

    public void SetInheritedIPType(IPType ipType) { _inheritedIPType = ipType; }

    public float GetCompletionRatio { get; protected set; }

    public System.Action OnBegin, OnEnd;

    [SerializeField] AK.Wwise.Event _akEventPlay;
    [SerializeField] AK.Wwise.Event _akEventStop;

    private void Start()
    {
        GameMaster.Instance.gf.dm.onDayStarts += MakeResetActivity;
    }

    private void OnDestroy()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.gf.dm.onDayStarts -= MakeResetActivity;
    }

    public void MakeResetActivity()
    {
        SetState(ActivityState.WAITING);
    }

    public virtual bool IsAvailable(Character character)
    {
        return holders.Count < maxHolder && curActState != ActivityState.COMPLETE;
    }
    public virtual bool IsAvailable()
    {
        return holders.Count < maxHolder && curActState != ActivityState.COMPLETE;
    }

    public virtual void Begin(Character character)
    {
        ActivityHolder holder = new ActivityHolder();
        holder.character = character;
        holder.startTime = Time.time;

        holders.Add(holder);

        if (_akEventPlay != null)
            _akEventPlay.Post(gameObject);

        SetState(ActivityState.RUNNING);

        ChildCharacter child = character.GetComponent<ChildCharacter>();
        if (child)
        {
            child.StartAnimState(animStateName);
        }

        if (OnBegin != null) OnBegin();
    }

    protected virtual void End(Character character)
    {
        ActivityHolder holder = GetHolderForCharacter(character);
        if (holder != null)
        {
            holders.Remove(holder);

            if (_akEventStop != null)
                _akEventStop.Post(gameObject);
        }


        if (doOnce)
        {
            SetState(ActivityState.COMPLETE);
            curActivityResetDelay = _activityResetDelay;
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
            child.StartAnimState(animStateName + "_End");
        }

        if (OnEnd != null) OnEnd();
    }

    ActivityHolder GetHolderForCharacter(Character character)
    {
        return holders.Find(x => x.character == character);
    }

    protected virtual void SetState(ActivityState newState)
    {
        if (curActState != newState)
            curActState = newState;

        Debug.Log("Activity state = " + newState, this);
    }

    public void Update()
    {
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        switch (curActState)
        {
            case ActivityState.RUNNING:
                UpdateRunningState();
                break;
            case ActivityState.COMPLETE:
                if (GameMaster.Instance.gf.GetGameState == GameState.DAY)
                {
                    if (curActivityResetDelay > 0)
                    {
                        curActivityResetDelay -= (1 + GameMaster.Instance.vm.GetChildsIpDegradationAddedFactor) * Time.deltaTime;
                    }
                    else
                        SetState(ActivityState.WAITING);
                }
                break;
        }
    }

    protected virtual void UpdateRunningState()
    {

    }

    public void CancelActivity(Character character)
    {

        Debug.Log("Cancel Activity");
        ActivityHolder holder = GetHolderForCharacter(character);
        if (holder != null)
        {
            holders.Remove(holder);
        }

        ChildCharacter child = character.GetComponent<ChildCharacter>();
        if (child)
        {
            child.StartAnimState(animStateName + "_End");
        }
    }
}

[System.Serializable]
public class ActivityModifier
{
    public ChildStatID _childStat;
    public float _factor;
}
