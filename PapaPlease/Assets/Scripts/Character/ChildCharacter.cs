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

    bool isFrozen = false;
    bool isSlaped = false;

    private void Awake()
    {
        statsContainer.Init();
    }

    [SerializeField]
    Vector2 timeBetweenAnger = new Vector2(1f, 2f);

    [SerializeField]
    float timeBetweenAngerIncPerObeissance = 1f;

    float nextAngerTime = 0f;
    float lastAngerTime = 0f;

    bool isDoingAnger = false;

    [SerializeField]
    ChildStatID obeissanceStatID = null;

    // Use this for initialization
    void Start () {
        SetState(ChildAIState.WAITING);
	}
	
	// Update is called once per frame
    void Update ()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        extraRotation();
        UpdateStates();
    }

    public void Freeze(bool state)
    {
        Debug.Log("freeze set to " + state);
        if (state)
        {
            isFrozen = true;
            navAgent.isStopped = true;
        }
        else
        {
            isFrozen = false;
            navAgent.isStopped = false;
        }
    }

    public void GiveOrder (IPType ipType)
    {
        if (isDoingAnger) return;

        SetCurrentInterestPoint(hm.GetRandomInterestPoint(ipType, lastInterestPoint));
        if(!isSlaped)
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


    #region Slap
    public void IsSlapped ()
    {
        Freeze(true);
        isSlaped = true;
        if (waitAndApplySlapHitCoroutine != null) StopCoroutine(waitAndApplySlapHitCoroutine);
        waitAndApplySlapHitCoroutine = StartCoroutine(waitAndApplySlapHit());
        LaunchSlapAnim();

        StopPotentialAnger();

        if (state == ChildAIState.IN_ACTIVITY)
        {
            SetState(ChildAIState.WAITING);
        }
        

    }

    Coroutine waitAndApplySlapHitCoroutine = null;
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

        transform.forward = GameMaster.Instance.player.transform.position - transform.position;


        yield return new WaitForSeconds(slapKODuration);

        SetNextAngerTime();
        isSlaped = false;
        Freeze(false);
    }

    Vector3 GetHitPosition ()
    {
        Vector3 fallDownDest = transform.position + (transform.position - GameMaster.Instance.player.transform.position) * slapFallDownDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(fallDownDest, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }
    #endregion

    #region PathFinding


    void MoveTo (Vector3 destination)
    {
        navAgent.SetDestination(destination);
        SetIsWalking(true);
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
            OnStateChange(oldState, newState);
        }
    }

    void OnStateChange (ChildAIState oldState, ChildAIState newState)
    {
        currentInactivityDuration = Mathf.Lerp(inactivityDurationRange.x, inactivityDurationRange.y, Random.Range(0f, 1f));
        stateBeginTime = Time.time;

        Debug.Log(childName + " changes state for : " + newState);

        switch(oldState)
        {
            case ChildAIState.IN_ACTIVITY:
                CancelActivity();
                break;
        }

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

    void UpdateStates()
    {
        CheckIsWalking();

        switch (state)
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


    /// <summary>
    /// WAITING STATE WAITING
    /// </summary>
    void StartWaiting  ()
    {
        StartWaitIdleCoroutine();
    }

    void StartWaitIdleCoroutine ()
    {
        if (waitForIdleCoroutine != null) StopCoroutine(waitForIdleCoroutine);
        waitForIdleCoroutine = StartCoroutine(waitForOutOfAnimToBeEnded());
    }

    Coroutine waitForIdleCoroutine = null;
    IEnumerator waitForOutOfAnimToBeEnded()
    {
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
        {
            yield return new WaitForEndOfFrame();
        }

        switch (state)
        {
            case ChildAIState.WAITING:
                RealStartWaiting();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                RealStartMovingToActivity();
                break;
        }
    }

    void RealStartWaiting ()
    {
        Vector3 waitingDestination = transform.position + Random.insideUnitSphere * waitingDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(waitingDestination, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        MoveTo(finalPosition);
    }

    void UpdateWaiting()
    {
        if (Time.time - stateBeginTime >= currentInactivityDuration && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
        {
            //wait a little time before starting a new activity
            SetState(ChildAIState.ROAMING);
        }
    }

    /// <summary>
    /// ROAMING STATE ROAMING
    /// </summary>
    void StartRoaming ()
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(IpTypeFun, lastInterestPoint));
    }

    void UpdateRoaming()
    {



    }

    /// <summary>
    /// MOVING TO ACTIVITY STATE MOVING TO ACTIVITY
    /// </summary>
    void StartMovingToActivity ()
    {
        SetNextAngerTime();
        StartWaitIdleCoroutine();
    }

    void RealStartMovingToActivity ()
    {
        lastAngerTime = Time.time;
        SetNextAngerTime();
        MoveTo(currentInterestPoint.pivotPoint);
    }

    void UpdateMovingToActivity()
    {
        //Debug.Log("remaining distance = " + navAgent.remainingDistance);
        // MoveAlongPath();

        if(currentInterestPoint && currentInterestPoint.iPtype != IpTypeFun)
            CheckAnger();

        if (!isDoingAnger && !isFrozen && Vector3.Distance(transform.position, currentInterestPoint.transform.position) <= 1f)
        {
            //has reached IP
            HasReachedIP();
        }

    }

    void SetNextAngerTime ()
    {
        float rand = Random.Range(0f, 1f);

        //TODO : get la state obeissance
        nextAngerTime = Mathf.Lerp(timeBetweenAnger.x, timeBetweenAnger.y, rand);
        Debug.Log("next anger time = " + nextAngerTime);
        lastAngerTime = Time.time;
    }

    void CheckAnger()
    {
        if(Time.time - lastAngerTime > nextAngerTime && !isFrozen && !isLerping && !isSlaped)
        {
            StartAnger();
        }
    }

    void StartAnger ()
    {
        Debug.Log("ANGER ANGER ANGER ANGER");
        isDoingAnger = true;

        Freeze(true);
        animator.Play("Anger");
        animator.SetBool("IsDoingAnger", isDoingAnger);
    }

    void UpdateAngerBehaviour ()
    {

    }

    void StopPotentialAnger ()
    {
        isDoingAnger = false;

        animator.SetBool("IsDoingAnger", isDoingAnger);
        SetNextAngerTime();
    }

    void HasReachedIP()
    {
        if (currentInterestPoint.activity.IsAvailable(this))
        {
            if (!isLerping)
                StartLerpingToSnap();
        }
        else
        {
            SetState(ChildAIState.WAITING);
        }
    }

    void StartLerpingToSnap()
    {
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpCoroutine());
    }

    bool isLerping = false;
    Coroutine lerpCoroutine = null;
    IEnumerator LerpCoroutine()
    {
        float factor = 0f;
        float lerpSpeed = 1f;
        Vector3 startPosition;
        Quaternion startRot;

        factor = 0f;
        startPosition = transform.position;
        startRot = transform.rotation;
        isLerping = true;
        while (factor < 1f)
        {

            factor += Time.deltaTime * lerpSpeed;

            factor = Mathf.Clamp01(factor);

            transform.position = Vector3.Lerp(startPosition, currentInterestPoint.pivotPoint.position, factor);
            transform.rotation = Quaternion.Lerp(startRot, currentInterestPoint.pivotPoint.rotation, factor);
            yield return new WaitForEndOfFrame();
        }

        isLerping = false;
        Debug.Log(childName + " begins activity on " + currentInterestPoint.ipName);
        SetState(ChildAIState.IN_ACTIVITY);
    }


    /// <summary>
    /// IN ACTIVITY STATE IN ACTIVITY
    /// </summary>
    void StartInActivity ()
    {
        currentInterestPoint.Interact(this);
        
    }

    void CancelActivity ()
    {
        if(currentActivity)
        {
            currentActivity.CancelActivity(this);
            currentActivity = null;
        }
    }

    void UpdateInActivity()
    {
        if (isLerping) return;
        currentInterestPoint.iPtype.TryModifyStats(IPType.StatModificationType.DURING_ACTIVITY, statsContainer);
                
        transform.position = currentInterestPoint.pivotPoint.position;
        transform.rotation = currentInterestPoint.pivotPoint.rotation;
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        Debug.Log(childName + " ends activity on " + currentInterestPoint.ipName);
        currentActivity = null;

        currentInterestPoint.iPtype.TryModifyStats(IPType.StatModificationType.END_ACTIVITY, statsContainer);
        currentInterestPoint.TryMakeGlobalModification(InterestPointModification.ON_COMPLETED);

        lastInterestPoint = currentInterestPoint;
        currentInterestPoint = null;

        SetState(ChildAIState.WAITING);
    }

    
    Vector3 lastPosition;
    bool isWalking = false;
    void CheckIsWalking ()
    {
        
        isWalking = Vector3.Distance(lastPosition, transform.position) > 0.005f;

        lastPosition = transform.position;

        SetIsWalking(isWalking);
    }
    
    
    #endregion


    #region Animation
    [SerializeField]
    Animator animator;

    void SetIsWalking (bool state)
    {
        animator.SetBool("IsWalking", state);
    }

    public float extraRotationSpeed = 8f;
    void extraRotation()
    {
        if (!isWalking) return;
        Vector3 lookrotation = navAgent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

    }

    void LaunchSlapAnim ()
    {
        animator.Play("SlapHit");
    }

    public void StartAnimState(string stateName)
    {
       
        animator.Play(stateName);
    }
    #endregion
}
