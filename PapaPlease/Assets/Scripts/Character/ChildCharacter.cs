using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ChildAIState { WAITING, ROAMING, MOVING_TO_ACTIVITY, IN_ACTIVITY }
public class ChildCharacter : Character {
    public HouseMaster hm = null;

    [SerializeField]
    NavMeshAgent navAgent;


    InterestPoint currentInterestPoint;
    InterestPoint lastInterestPoint;

    public string childName = "";
    public bool isMale = false;

    ChildAIState state = ChildAIState.ROAMING;

    [SerializeField]
    Vector2 inactivityDurationRange;
    float currentInactivityDuration = 0f;
    float stateBeginTime = 0f;


    [SerializeField]
    float waitingDistance = 2f;

    [SerializeField]
    IPType IpTypeFun = null;

    [SerializeField]
    float slapFallDownDistance = 1f;
    [SerializeField]
    float delayForHSlapHit = 0.3f;

    [SerializeField]
    float slapFallDownSpeed = 6f;

    [SerializeField]
    float slapKODuration = 1f;

    [SerializeField]
    AnimationCurve slapFallDownCurve;


    // Use this for initialization
    void Start () {
        SetState(ChildAIState.WAITING);
	}
	
	// Update is called once per frame
    void Update ()
    {
        UpdateStates();
    }

    public void Freeze(bool state)
    {
        Debug.Log("freeze set to " + state);
        if (state)
            navAgent.isStopped = true;
        else
        {
            navAgent.isStopped = false;
        }
    }

    public void GiveOrder (IPType ipType)
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(ipType, lastInterestPoint));
        Freeze(false);
    }

    void SetCurrentInterestPoint(IPType ipType)
    {
        hm.GetRandomInterestPoint(ipType, lastInterestPoint);
    }
    void SetCurrentInterestPoint(InterestPoint interestPoint)
    {
        currentInterestPoint = interestPoint;
        if (currentInterestPoint)
        {
            Debug.Log(childName + "has received a new IP");
            SetState(ChildAIState.MOVING_TO_ACTIVITY, true);
        }
        else
        {
            Debug.LogWarning(childName + "has received a null IP");
            SetState(ChildAIState.WAITING);
        }
            
    }



    public void IsSlapped ()
    {

        Freeze(true);
        StartCoroutine(waitAndApplySlapHit());
    }

    IEnumerator waitAndApplySlapHit ()
    {
        yield return new WaitForSeconds(delayForHSlapHit);

        float animFactor = 0f;

        Vector3 targetPos = GetHitPosition();
        Vector3 startPos = transform.position;

        while(animFactor < 1f)
        {
            animFactor += Time.deltaTime * slapFallDownSpeed;

            Vector3 pos = Vector3.Lerp(startPos, targetPos, slapFallDownCurve.Evaluate(animFactor));

            transform.position = pos;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(slapKODuration);

        Freeze(false);
        SetState(ChildAIState.WAITING);
    }

    Vector3 GetHitPosition ()
    {
        Vector3 fallDownDest = transform.position + (transform.position - GameMaster.Instance.player.transform.position) * slapFallDownDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(fallDownDest, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }

    #region PathFinding


    void MoveTo (Vector3 destination)
    {
        navAgent.SetDestination(destination);
       /* _targetPoint = destination;
        CalculateNavMesh();*/
    }
    void MoveTo (Transform transform)
    {
        MoveTo(transform.position);
    }
    #endregion 

    #region State Management
    void SetState (ChildAIState newState)
    {
        SetState(newState, false);
    }
    void SetState (ChildAIState newState, bool force)
    {
        if (force || state != newState)
        {
            ChildAIState oldState = state;
            state = newState;
            OnStateChange(state, newState);
        }
    }

    void OnStateChange (ChildAIState oldState, ChildAIState newState)
    {
        currentInactivityDuration = Mathf.Lerp(inactivityDurationRange.x, inactivityDurationRange.y, Random.Range(0f, 1f));
        stateBeginTime = Time.time;

        Debug.Log(childName + " changes state for : " + newState);

        switch(newState)
        {
            case ChildAIState.WAITING:
                StartWaiting();
                break;
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
    

    void StartWaiting  ()
    {
        Vector3 waitingDestination = transform.position + Random.insideUnitSphere * waitingDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(waitingDestination, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        navAgent.SetDestination(finalPosition);
    }

    void StartRoaming ()
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(IpTypeFun, lastInterestPoint));
    }

    void StartMovingToActivity ()
    {
        MoveTo(currentInterestPoint.pivotPoint);
    }

    void StartInActivity ()
    {
        currentInterestPoint.Interact(this);
        
    }

    void UpdateStates ()
    {
        switch(state)
        {
            case ChildAIState.WAITING:
                UpdateWaiting();
                break;
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

    void UpdateWaiting ()
    {
        if (Time.time - stateBeginTime >= currentInactivityDuration)
        {
            //wait a little time before starting a new activity
            SetState(ChildAIState.ROAMING);
        }
    }

    void UpdateRoaming ()
    {
        
        
        
    }

    void UpdateMovingToActivity ()
    {
        //Debug.Log("remaining distance = " + navAgent.remainingDistance);
       // MoveAlongPath();

        if (navAgent.remainingDistance <= 1f)
        {
            //has reached IP
            HasReachedIP();
        }
        
    }

    void HasReachedIP ()
    {
        if(currentInterestPoint.activity.IsAvailable(this))
        {

            Debug.Log(childName +" begins activity on " + currentInterestPoint.ipName);
            SetState(ChildAIState.IN_ACTIVITY);
            IsInRangeForSnap();
        }
        else
        {
            SetState(ChildAIState.WAITING);
        }
    }

    void IsInRangeForSnap()
    {
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpCoroutine());
    }

    Coroutine lerpCoroutine = null;
    IEnumerator LerpCoroutine()
    {
        float factor = 0f;
        float lerpSpeed = 3f;
        Vector3 startPosition;
        Quaternion startRot;

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

    void UpdateInActivity ()
    {
        
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        currentActivity = null;

        Debug.Log("ends");

        lastInterestPoint = currentInterestPoint;
        currentInterestPoint = null;

        SetState(ChildAIState.WAITING);
    }
    #endregion
}
