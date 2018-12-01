using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ChildAIState { ROAMING, MOVING_TO_ACTIVITY, IN_ACTIVITY }
public class ChildCharacter : Character {
    public HouseMaster hm = null;

    [SerializeField]
    NavMeshAgent navAgent;


    InterestPoint currentInterestPoint;

    public string childName = "";
    public bool isMale = false;

    [SerializeField]
    ChildAIState state = ChildAIState.ROAMING;

    [SerializeField]
    Vector2 inactivityDurationRange;
    float currentInactivityDuration = 0f;
    float stateBeginTime = 0f;

	// Use this for initialization
	void Start () {
        SetState(ChildAIState.ROAMING);
	}
	
	// Update is called once per frame
    void Update ()
    {
        UpdateStates();
    }
    

    public void SetCurrentInterestPoint (InterestPoint interestPoint)
    {
        currentInterestPoint = interestPoint;
        MoveTo(currentInterestPoint.pivotPoint);
    }

    void MoveTo (Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }
    void MoveTo (Transform transform)
    {
        MoveTo(transform.position);
    }

    #region State Management
    void SetState (ChildAIState newState)
    {
        if (state != newState)
        {
            ChildAIState oldState = state;
            state = newState;
            OnStateChange(state, newState);
        }
    }

    void OnStateChange (ChildAIState oldState, ChildAIState newState)
    {
        currentInactivityDuration = Mathf.Lerp(inactivityDurationRange.x, inactivityDurationRange.y, Random.Range(0f, 1f));

        switch(newState)
        {
            case ChildAIState.ROAMING:
                StartRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                StartMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                StartInActivity();
                break;
        }
    }

    void StartRoaming ()
    {
        stateBeginTime = Time.time;
    }

    void StartMovingToActivity ()
    {

    }

    void StartInActivity ()
    {
        currentInterestPoint.Interact(this);
        
    }

    void UpdateStates ()
    {
        switch(state)
        {
            case ChildAIState.ROAMING:
                UpdateRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                UpdateMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                UpdateInActivity();
                break;
        }
    }

    void UpdateRoaming ()
    {
        if (Time.time - stateBeginTime >= currentInactivityDuration)
        {
            //wait a little time before starting a new activity
            GetRoamingInterestPoint();
            SetState(ChildAIState.MOVING_TO_ACTIVITY);
        }
    }

    void GetRoamingInterestPoint()
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(InterestPointCategory.FUN));
    }

    void UpdateMovingToActivity ()
    {
        if(navAgent.remainingDistance == 0f)
        {
            //has reached IP
            SetState(ChildAIState.IN_ACTIVITY);
            Debug.Log("begin");
        }
    }

    void UpdateInActivity ()
    {
        
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        currentActivity = null;

        SetState(ChildAIState.ROAMING);
        Debug.Log("ends");
    }
    #endregion
}
