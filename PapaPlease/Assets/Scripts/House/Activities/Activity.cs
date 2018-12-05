using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
            if (curActMod._isFromMaxToMin)
            {
                toReturn += curActMod._factor * (1 - character.statsContainer.GetAChildStatValueRatio(curActMod._childStat));
                Debug.Log("ActivityModifier MAXFROMMIN: " + toReturn + "__ratio: " + character.statsContainer.GetAChildStatValueRatio(curActMod._childStat));
            }
            else
            {
                toReturn += curActMod._factor * (character.statsContainer.GetAChildStatValueRatio(curActMod._childStat));
                Debug.Log("ActivityModifier NORMAL: " + toReturn);
            }
        }
        if (toReturn < -0.8f)
            toReturn = -0.8f;

        Debug.Log("ActivityModifiers result: " + toReturn);

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

    public float money = 0f;

    [SerializeField] float _activityResetDelay = 60f;

    [SerializeField] float curActivityResetDelay;

    [SerializeField] List<GameObject> _itemsToActivateOnRunning;

    [SerializeField] List<Renderer> _dirtyVisualsRenderers;
    [SerializeField] List<GameObject> _dirtyVisualsToActivate;

    protected IPType _inheritedIPType;

    public void SetInheritedIPType(IPType ipType) { _inheritedIPType = ipType; }

    public float GetCompletionRatio { get; protected set; }

    public System.Action OnBegin, OnEnd;

    [SerializeField] AK.Wwise.Event _akEventPlay;
    [SerializeField] AK.Wwise.Event _akEventStop;

    private void Start()
    {
        GameMaster.Instance.gf.dm.onDayStarts += MakeResetActivity;
        MakeResetActivity();
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

        if (_akEventPlay != null && _akEventPlay.IsValid())
            _akEventPlay.Post(gameObject);

        SetState(ActivityState.RUNNING);

        ChildCharacter child = character.GetComponent<ChildCharacter>();
        if (child)
        {
            child.StartAnimState(animStateName);
        }

        foreach (var item in _itemsToActivateOnRunning)
            item.SetActive(true);

        if (OnBegin != null) OnBegin();
    }

    protected virtual void End(Character character)
    {
        ActivityHolder holder = GetHolderForCharacter(character);
        if (holder != null)
        {
            holders.Remove(holder);

            if (_akEventStop != null && _akEventStop.IsValid())
                _akEventStop.Post(gameObject);
        }


        if (doOnce)
        {
            SetState(ActivityState.COMPLETE);
            curActivityResetDelay = _activityResetDelay;
            if (money > 0) GameMaster.Instance.wallet.EarnMoney(money);
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

        foreach (var item in _itemsToActivateOnRunning)
            item.SetActive(false);

        if (OnEnd != null) OnEnd();
    }

    ActivityHolder GetHolderForCharacter(Character character)
    {
        return holders.Find(x => x.character == character);
    }

    protected virtual void SetState(ActivityState newState)
    {
        switch (newState)
        {
            case ActivityState.COMPLETE:
                foreach (var curRend in _dirtyVisualsRenderers)
                {
                    foreach (var curMat in curRend.materials)
                        curMat.SetFloat("_DirtBlending", 0f);
                }
                foreach (var curObj in _dirtyVisualsToActivate)
                    curObj.SetActive(false);
                break;
            case ActivityState.WAITING:
                foreach (var curRend in _dirtyVisualsRenderers)
                {
                    foreach (var curMat in curRend.materials)
                        curMat.SetFloat("_DirtBlending", 1f);
                }
                foreach (var curObj in _dirtyVisualsToActivate)
                    curObj.SetActive(true);
                break;
        }
        if (curActState != newState)
            curActState = newState;
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
        foreach (var item in holders)
        {

        }
    }

    public void CancelActivity(Character character)
    {

        if (_akEventStop != null && _akEventStop.IsValid())
            _akEventStop.Post(gameObject);

        Debug.Log("Cancel Activity");
        SetState(ActivityState.WAITING);
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
    public bool _isFromMaxToMin;
}
