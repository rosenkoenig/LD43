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
    InterestPoint lastInterestPoint;

    public string childName = "";
    public bool isMale = false;

    [SerializeField]
    ChildAIState state = ChildAIState.ROAMING;

    [SerializeField]
    Vector2 inactivityDurationRange;
    float currentInactivityDuration = 0f;
    float stateBeginTime = 0f;

    [SerializeField]
    IPType IpTypeFun = null;

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
        if(currentInterestPoint)
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

        Debug.Log("New state : " + newState);

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
        lastInterestPoint = currentInterestPoint;
        currentInterestPoint = null;
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
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(IpTypeFun, lastInterestPoint));
    }

    void UpdateMovingToActivity ()
    {
        //Debug.Log("remaining distance = " + navAgent.remainingDistance);
        if(navAgent.remainingDistance <= 1f)
        {
            //has reached IP
            HasReachedIP();
            IsInRangeForSnap();
        }
        
    }

    void IsInRangeForSnap ()
    {
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpCoroutine());
    }

    float factor = 0f;
    float lerpSpeed = 3f;
    Vector3 startPosition;
    Quaternion startRot;
    Coroutine lerpCoroutine = null;
    IEnumerator LerpCoroutine ()
    {
        navAgent.enabled = false;

        factor = 0f;
        startPosition = transform.position;
        startRot = transform.rotation;
        while (factor < 1f)
        {
            factor += Time.deltaTime * lerpSpeed;

            factor = Mathf.Clamp01(factor);

            transform.position = Vector3.Lerp(startPosition, currentInterestPoint.pivotPoint.position, factor);
            transform.rotation = Quaternion.Lerp(startRot, currentInterestPoint.pivotPoint.rotation, factor);
            yield return new WaitForEndOfFrame();
        }
    }

    void HasReachedIP ()
    {
        Debug.Log("begin");
        SetState(ChildAIState.IN_ACTIVITY);
    }

    void UpdateInActivity ()
    {
        
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        currentActivity = null;

        navAgent.enabled = true;

        Debug.Log("ends");
        SetState(ChildAIState.ROAMING);
    }
    #endregion
}
